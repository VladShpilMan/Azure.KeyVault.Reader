using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Azure.KeyVault.Reader.src
{
    internal class AzureConnectionStringProvider : IConnectionStringProvider
    {
        private readonly Dictionary<string, string> _cechedConnectionStrings;

        public AzureConnectionStringProvider()
        {
            _cechedConnectionStrings = new Dictionary<string, string>();
        }

        // This method retrieves a connection string by its name.
        // It first checks if the connection string is cached, and if so, returns the cached value.
        // Otherwise, it uses the AzureSecretProvider class to retrieve the connection string and caches the result for future use.
        public string GetConnectionStringByName(string name)
        {
            string result;

            if(_cechedConnectionStrings.TryGetValue(name, out result))
            {
                return result;
            }

            result = AzureSecretProvider.GetSecretByName(name);

            if(result != null)
            {
                _cechedConnectionStrings.Add(name, result);
            }

            return result;
        }

        // This method automatically configures a connection string from the "connectionstrings.json" configuration file.
        public void AutoMapConnectionStrings()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("connectionstrings.json", optional: true, reloadOnChange: true)
                .Build();

            foreach (var conn in config.GetSection("ConnectionStrings").GetChildren())
            {
                try
                {
                    string connectionString = ConfigurationProvider.GetConnectionString(conn.Key);

                    if(connectionString != null)
                    {
                        ConfigureConnectionString(conn.Key, connectionString);
                    }
                }
                catch(RequestFailedException ex)
                {
                    //TODO : To Describe possible causes of the problem.
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // This method automatically configures a app connections from the "connectionstrings.json" configuration file.
        public void AutoMapAppSettings()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            foreach (var settingKey in config.GetSection("AppSettings").GetChildren())
            {
                try
                {
                    string appSetting = ConfigurationProvider.GetConnectionString(settingKey.Key);
                    if(appSetting != null)
                    {
                        ConfigureAppSetting(settingKey.Key, appSetting);
                    }
                }
                catch (RequestFailedException ex)
                {
                    //TODO : To Describe possible causes of the problem.
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void ConfigureConnectionString(string key, string value)
        {
            //TODO : Adding a secret to the connection string key
        }

        private void ConfigureAppSetting(string key, string value)
        {
            //TODO : Adding a secret to the app setting key
        }
    }
}
