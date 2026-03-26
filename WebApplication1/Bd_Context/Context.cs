using System.Data;
using Npgsql;

namespace WebApplication1.Bd_Context
{
    public class Context
    {
        private readonly string _connectionString;

        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);
            Console.WriteLine(_connectionString);

            conn.Open();
            return conn;
        }
    }
}
