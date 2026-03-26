using WebApplication1.Bd_Context;
using WebApplication1.Models;
using Dapper;

namespace WebApplication1.Login
{
    public class LoginRepository
    {
        private readonly Context _context;

        public LoginRepository(Context context)
        {
            _context = context;
        }

        public Users Login(string name, string password)
        {
            using var conn = _context.GetConnection();

            var user = conn.QueryFirstOrDefault<Users>(
                "SELECT name, password, role FROM users WHERE name=@name AND password=@password",
                new { name = name, password = password });

            if (user != null)
            {
                user.is_conect = true;
                return user;
            }

            return new Users(false);
        }
    }
}
