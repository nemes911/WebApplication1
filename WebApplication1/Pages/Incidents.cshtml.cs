using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using Npgsql;

namespace WebApplication1.Pages
{
    public class IncidentsModel : PageModel
    {
        [BindProperty]
        public List<Vehicle> incident_vehicle { get; set; }


        public void OnGet()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];

            var connectionString =
                $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"SELECT *
FROM gai.vehicles v
WHERE v.id NOT IN (
    SELECT iv.vehicle_id
    FROM gai.incident_vehicles iv
);", conn);
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        incident_vehicle.Add(new Vehicle
                        {
                            Id = reader.GetGuid(0),
                            SerialNumber = reader.GetInt32(1),
                            Color = reader.GetString(2),
                            OwnerId = reader.GetGuid(3),
                            CarBrand = reader.GetString(4),
                            Insurance_company = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Vin = reader.GetString(6)
                        });
                    }
                }
            }
        }



    }
}
