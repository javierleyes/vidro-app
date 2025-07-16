using Npgsql;

namespace vidro.api.Extension
{
    public static class PostgresConnectionFactory
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            string? postgresConnectionString = string.Empty;

            // Get the connection string from the ENV variables (Heroku)
            string databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (!string.IsNullOrEmpty(databaseUrl))
            {
                var uri = new Uri(databaseUrl);
                var userInfo = uri.UserInfo.Split(':');

                var postgresConnectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = uri.LocalPath.TrimStart('/')
                };

                return postgresConnectionStringBuilder.ConnectionString;
            }

            return configuration.GetConnectionString("VidroConnection");
        }
    }
}
