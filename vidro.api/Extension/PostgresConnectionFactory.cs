using Npgsql;

namespace vidro.api.Extension
{
    public static class PostgresConnectionFactory
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // Try to get connection string from configuration
            var connectionString = configuration.GetConnectionString("VidroConnection");

            if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgresql://"))
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

                Console.WriteLine($"Host: {uri.Host}");
                Console.WriteLine($"Database: {uri.AbsolutePath.TrimStart('/')}");
                Console.WriteLine($"Username: {uri.UserInfo.Split(':')[0]}");
                Console.WriteLine($"Password: {uri.UserInfo.Split(':')[1]}");
                Console.WriteLine($"Port: {uri.Port}");

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
