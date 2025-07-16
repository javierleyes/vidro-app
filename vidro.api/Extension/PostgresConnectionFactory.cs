using Npgsql;

namespace vidro.api.Extension
{
    public static class PostgresConnectionFactory
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // Try to get connection string from configuration (localhost)
            var connectionString = configuration.GetConnectionString("VidroConnection");

            // If not found, try to get it from environment variables (Render)
            if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgresql://"))
            {
                connectionString = ConvertPostgresUrlToConnectionString(connectionString);
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No database connection string found. Check your configuration or environment variables.");
            }

            return connectionString;
        }

        private static string ConvertPostgresUrlToConnectionString(string postgresUrl)
        {
            try
            {
                var uri = new Uri(postgresUrl);
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = 5432,
                    Database = uri.AbsolutePath.TrimStart('/'),
                    Username = uri.UserInfo.Split(':')[0],
                    Password = uri.UserInfo.Split(':')[1],
                };

                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse PostgreSQL URL: {ex.Message}");
            }
        }
    }
}
