using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class PravaModel : PageModel
    {
        [BindProperty]
        public List<Prava> pravas {  get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPostPravaLikeCategory()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                
                var cmd = new NpgsqlCommand(@"
            SELECT *
            FROM gai.prava
            WHERE ""type"" = @type
            ORDER BY ""date"" DESC;
        ", conn);

               
                string categoryInput = Request.Form["Category"].ToString().Trim();

                
                string[] typeArray = string.IsNullOrWhiteSpace(categoryInput)
                    ? Array.Empty<string>()
                    : new[] { categoryInput };

                cmd.Parameters.AddWithValue("@type", typeArray);

                var pravaList = new List<Prava>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var prava = new Prava
                        {
                            id = reader.GetGuid("id"),
                            date = reader.GetDateTime("date"),
                            series = reader.GetString("series"),
                            number = reader.GetInt32("number"),
                            date_end = reader.GetDateTime("date_end"),
                            kod_podrazdeleniya = reader.GetString("kod_podrazdeleniya"),
                            type = reader.GetFieldValue<string[]>("type"),   
                            status = reader.GetBoolean("status")
                        };
                        pravaList.Add(prava);
                    }
                }

                
                pravas = pravaList;
            }

            return Page();   
        }
    }
}
