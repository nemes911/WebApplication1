using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using WebApplication1.Bd_Context;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class AddIncidentModel : PageModel
    {
        [BindProperty]
        public Incident incident { get; set; }
        
        public void OnGet()
        {
        }

        public IActionResult OnPostAddIncident()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];

            var constring =
                $"Host=localhost;Database=postgres;Username={name};Password={password}";

            var sql = @"INSERT INTO gai.incidents
                (incident_class_id, incident_date, description, repair_cost, 
                 ""timestamp"", ""location"", police_station_id)
                VALUES (@incident_class_id, @incident_date, @description, @repair_cost, 
                        @timestamp, @location, @police_station_id)";

            try
            {
                using (var conn = new NpgsqlConnection(constring))
                {
                    conn.Open();

                    using var cmd = new NpgsqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("incident_class_id", incident.IncidentClassId);
                    cmd.Parameters.AddWithValue("incident_date", incident.IncidentDate);
                    cmd.Parameters.AddWithValue("description", incident.Description);
                    cmd.Parameters.AddWithValue("repair_cost", incident.RepairCost ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("timestamp", incident.Timestamp);
                    cmd.Parameters.AddWithValue("location", incident.Location);
                    cmd.Parameters.AddWithValue("police_station_id", incident.PoliceStationId);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToPage("/Main");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка добавления: {ex.Message}");
                return Page();
            }
        }

    }
}
