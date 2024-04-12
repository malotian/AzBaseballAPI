using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CView.AzFunction
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class DB
    {
        private static DB? instance;
        private SqlConnection connection;

        private static readonly string dbhost = Environment.GetEnvironmentVariable("DB_HOST") ?? "4.157.175.137";
        private static readonly string dbname = Environment.GetEnvironmentVariable("DB_NAME") ?? "dugout";
        private static readonly string dbuser = Environment.GetEnvironmentVariable("DB_USER") ?? "harjinder";
        private static readonly string dbpwd = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Dutch72reappear91caches";

        // Private constructor to prevent instantiation
        private DB()
        {
        }

        // Public method to access the Singleton instance
        public static DB Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DB();
                    string connectionString = $"Server={dbhost};Database={dbname};User Id={dbuser};Password={dbpwd};Encrypt=true;TrustServerCertificate=true;";
                    instance.connection = new SqlConnection(connectionString);
                    instance.connection.Open();
                }
                return instance;
            }
        }

        // Public method to get the SqlConnection
        public SqlConnection GetConnection()
        {
            return connection;
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}