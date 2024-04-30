using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AzBaseballAPI {

    public class FactoryDB {
        public SqlConnection Conn { get; private set; }
        //private readonly string _dbhost = "10.0.0.5";
        //private readonly string _dbname = "dugout";
        //private readonly string _dbuser = "FA.WpUser";
        //private readonly string _dbpwd = "8HzKrwf39Srj";

        private static readonly string _dbhost = Environment.GetEnvironmentVariable("DB_HOST") ?? "4.157.175.137";
        //private static readonly string _dbhost = Environment.GetEnvironmentVariable("DB_HOST") ?? "10.0.0.5";
        private static readonly string _dbname = Environment.GetEnvironmentVariable("DB_NAME") ?? "dugout";
        private static readonly string _dbuser = Environment.GetEnvironmentVariable("DB_USER") ?? "FA.WpUser"; //"harjinder";
        private static readonly string _dbpwd = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "8HzKrwf39Srj"; //"Dutch72reappear91caches";

        public FactoryDB(bool newLink = false) {
            string connectionString = $"Server={_dbhost};Database={_dbname};User Id={_dbuser};Password={_dbpwd};Encrypt=true;TrustServerCertificate=true;";
            Conn = new SqlConnection(connectionString);
            try {
                Conn.Open();
            } catch (SqlException e) {
                Console.WriteLine($"ERROR: Failed to connect to db server: {e.Message}");
                Conn = null;
            }
        }

        ~FactoryDB() {
            if (Conn != null && Conn.State == System.Data.ConnectionState.Open) {
                // Conn.Close();
            }
        }

        public static string ConvertToGuid(string guid) {
            guid = guid.Substring(0, 38);
            Regex regex = new Regex(@"^(\{)?[a-f\d]{8}(-[a-f\d]{4}){4}[a-f\d]{8}(?(1)\})$", RegexOptions.IgnoreCase);
            return regex.IsMatch(guid) ? guid : "{00000000-0000-0000-0000-000000000000}";
        }

        public static string ConvertToAlphanumeric(string alphanum) {
            alphanum = Regex.IsMatch(alphanum, @"[^a-z0-9]", RegexOptions.IgnoreCase) ? "" : alphanum;
            return alphanum;
        }

        public static string CleanURL01(string name) {
            name = name.ToLower();
            name = Regex.Replace(name, @"[^\w\d ]", " ");
            name = Regex.Replace(name, @"\s+", " ");
            name = name.Trim();
            name = name.Replace(" ", "-");
            return name;
        }

        //public List<Dictionary<string, object>> Execute(string sql) {
        //    using (var cmd = new SqlCommand(sql, Conn))
        //        return Execute(cmd);
        //}

        //public List<Dictionary<string, object>> Execute(SqlCommand cmd) {
        //    if (Conn == null || Conn.State != ConnectionState.Open)
        //        return null;

        //    cmd.Connection = Conn;

        //    var results = new List<Dictionary<string, object>>();
        //    using (var reader = cmd.ExecuteReader()) {
        //        while (reader.Read()) {
        //            var row = new Dictionary<string, object>();
        //            for (int i = 0; i < reader.FieldCount; i++)
        //                row[reader.GetName(i)] = reader.GetValue(i);
        //            results.Add(row);
        //        }
        //    }
        //    return results;
        //}
    }
}