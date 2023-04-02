using Microsoft.Extensions.Configuration;

namespace Azure.KeyVault.Reader.src
{
    public static class ConfigurationProvider
    {
        private static IConnectionStringProvider _connectionStringProvider;

        public static void Init()
        {
            var compromise = AzureSecretProvider.GetAppsettingByName("UseKeyVault");

            if(compromise != null)
            {
                if (bool.Parse(compromise))
                {
                    _connectionStringProvider = new AzureConnectionStringProvider();
                    _connectionStringProvider.AutoMapConnectionStrings();
                    _connectionStringProvider.AutoMapAppSettings();
                }
            }
        }

        public static string GetConnectionString(string connectionStringName)
        {
            return _connectionStringProvider.GetConnectionStringByName(connectionStringName);
        }
    }
}
