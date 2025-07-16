using Npgsql;

namespace vidro.api.Extension
{
    public static class PostgresConnectionFactory
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // Try to get connection string from configuration
            var connectionString = configuration.GetConnectionString("VidroConnection");

            // If not found, try alternative environment variable names
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            }

            // If we have a DATABASE_URL, it might be in PostgreSQL URL format, convert it
            if (!string.IsNullOrEmpty(connectionString))
            {
                connectionString = ConvertPostgresUrlToConnectionString(connectionString);
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No database connection string found. Check your configuration or environment variables.");
            }

            Console.WriteLine($"Using connection string: {connectionString}");

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
                    Port = uri.Port,
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
