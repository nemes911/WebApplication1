using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace WebApplication1.Pages
{
    public class AddUserModel : PageModel
    {
        public void OnGet()
        {
        }


        public void OnPostaddUser(string newuser_name)
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];

            var constring =
                $"Host=localhost;Database=postgres;Username={name};Password={password}";
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"SELECT  gai.create_or_update_user_with_district(@username);", conn);

                cmd.Parameters.AddWithValue("username", newuser_name);

                var genpass = cmd.ExecuteScalar()?.ToString();
            }
        }
    }
}
