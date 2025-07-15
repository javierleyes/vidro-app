using Npgsql;

namespace vidro.api.Extension
{
    public static class PostgresConnectionFactory
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            return configuration.GetConnectionString("VidroConnection");
        }
    }
}
