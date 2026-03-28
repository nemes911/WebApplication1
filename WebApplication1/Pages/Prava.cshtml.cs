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

                // Используем оператор ANY для поиска элемента в массиве
                var cmd = new NpgsqlCommand(@"
            SELECT *
            FROM gai.prava
            WHERE @type = ANY(""type"")
            ORDER BY ""date"" DESC;
        ", conn);

                string categoryInput = Request.Form["Category"].ToString().Trim();

                // Передаём строку напрямую, а не массив
                cmd.Parameters.AddWithValue("@type", categoryInput);

                var pravaList = new List<Prava>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var prava = new Prava
                        {
                            id = reader.GetGuid(reader.GetOrdinal("id")),
                            date = reader.GetDateTime(reader.GetOrdinal("date")),
                            series = reader.GetString(reader.GetOrdinal("series")),
                            number = reader.GetInt32(reader.GetOrdinal("number")),
                            date_end = reader.GetDateTime(reader.GetOrdinal("date_end")),
                            kod_podrazdeleniya = reader.GetString(reader.GetOrdinal("kod_podrazdeleniya")),
                            type = reader.GetFieldValue<string[]>(reader.GetOrdinal("type")),
                            status = reader.GetBoolean(reader.GetOrdinal("status"))
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
