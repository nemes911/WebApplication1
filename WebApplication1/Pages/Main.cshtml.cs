using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bd_Context;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class MainModel : PageModel
    {
        public List<ViewIncidents> viewincidents { get; set; } = null;

        public void OnGet()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];

            var connectionString =
                $"Host=localhost;Database=postgres;Username={name};Password={password}";

            var context = new Context(connectionString);

            using var conn = context.GetConnection();

            var sql = @"SELECT * FROM gai.incident_full_view";

            viewincidents = conn.Query<ViewIncidents>(sql).ToList();
        }

        public IActionResult OnPostAddIncident()
        {
            return RedirectToPage("/AddIncident");
        }

        public IActionResult OnPostDirectToStat()
        {
            return RedirectToPage("/Stat");
        }
    }
}
