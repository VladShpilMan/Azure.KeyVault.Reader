namespace Azure.KeyVault.Reader.src
{
    public interface IConnectionStringProvider
    {
        string GetConnectionStringByName(string name);
        void AutoMapConnectionStrings();
        void AutoMapAppSettings();
    }
}
