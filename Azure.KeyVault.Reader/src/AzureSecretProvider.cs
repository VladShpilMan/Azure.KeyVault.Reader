using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;

namespace Azure.KeyVault.Reader.src
{
    public class AzureSecretProvider
    {
        public static string KeyVaultName = string.Format("kv-{0}", GetAppsettingByName("Client"));
        public static string KeyVaultUri = string.Format(@"http://{0}.vault.azure.net", KeyVaultName);

        // The flag indicating whether the application is running in Azure
        private static bool _isHostedOnAzure
        {
            get
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                var azure = config["Azure"];

                if (string.IsNullOrEmpty(azure))
                {
                    return false;
                }

                return bool.Parse(azure);
            }
        }

        private static SecretClient _clientInstance;

        public static string GetSecretByName(string secretName)
        {
            try
            {
                if (_clientInstance is null)
                    _clientInstance = GetClient(KeyVaultUri);

                string secret = null;
                secret = _clientInstance.GetSecret(secretName)?.Value.Value;

                return secret;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("The operation could not be performed. Make sure you've signed in to your corporate account in Visual Studio.", ex);
            }
            catch (RequestFailedException ex)
            {
                //TODO : Determine the reason for the failed request and write a comment
                throw new Exception("Failed.", ex);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed.", ex);
            }
        }

        // Method for creating a client instance to work with a key storage
        private static SecretClient GetClient(string keyVaultUri)
        {
            var secretClient = new SecretClient(new Uri(keyVaultUri), GetTokenCredential());

            return secretClient;
        }

        // Method for obtaining an object that implements the TokenCredential interface for Azure authentication
        private static TokenCredential GetTokenCredential()
        {
            if (_isHostedOnAzure)
                return new DefaultAzureCredential();

            // If someone tries to compile the application locally, a token is created based on the user profile
            // that has signed into their Visual Studio account.
            var tokenCredential = new ChainedTokenCredential(new VisualStudioCredential());

            return tokenCredential;
        }

        internal static string GetAppsettingByName(string name)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            return config[name];
        }
    }
}
