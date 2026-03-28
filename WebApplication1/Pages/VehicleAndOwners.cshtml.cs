using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using Npgsql;

namespace WebApplication1.Pages
{
    public class VehicleAndOwnersModel : PageModel
    {
        [BindProperty]
        public Vehicle current_vehicle { get; set; } = null;

        [BindProperty]
        public Person current_person { get; set; } = null;

        [BindProperty]
        public Prava current_prava { get; set; } = null;

        [BindProperty]
        public List<Vehicle> vehicles { get; set; }

        [BindProperty]
        public List<Person> perssones { get; set;}

        [BindProperty]
        public List<Prava> pravas { get; set; }

        [BindProperty]
        public List<view_onwer_car> view_Onwer_Cars { get; set; }
        public void OnGet()
        {
            string name = Request.Cookies["username"];
            string password = Request.Cookies["password"];

            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";
            var list = new List<view_onwer_car>();

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"SELECT * FROM gai.vehicle_owner_license", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new view_onwer_car
                        {
                            Vehicle_id = reader.GetGuid(0),
                            Serial_number = reader.GetInt32(1),
                            color = reader.GetString(2),
                            car_brand = reader.GetString(3),
                            insurance_company = reader.IsDBNull(4) ? null : reader.GetString(4),
                            vin = reader.GetString(5),
                            owner_id = reader.GetGuid(6),
                            first_name = reader.GetString(7),
                            last_name = reader.GetString(8),
                            middle_name = reader.IsDBNull(9) ? null : reader.GetString(9),
                            passport_series = reader.GetInt32(10),
                            passport_number = reader.GetInt32(11),
                            social_status_id = reader.GetInt32(12),

                            license_id = reader.IsDBNull(13) ? null : reader.GetGuid(13),
                            license_issue_date = reader.IsDBNull(14) ? (DateTime?)null : reader.GetDateTime(14),
                            license_series = reader.IsDBNull(15) ? null : reader.GetString(15),
                            license_number = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                            license_expiry_date = reader.IsDBNull(17) ? (DateTime?)null : reader.GetDateTime(17),
                            kod_podrazdeleniya = reader.IsDBNull(18) ? null : reader.GetString(18),

                            license_type = reader.IsDBNull(19) ? null : reader.GetString(19),   // теперь строка
                            license_active = reader.IsDBNull(20) ? null : reader.GetString(20), // теперь строка ("active"/"inactive")

                            owner_full_name = reader.GetString(21),
                            license_days_left = reader.IsDBNull(22) ? (int?)null : reader.GetInt32(22)
                        });

                    }
                }
            }
            view_Onwer_Cars = list;
        }
        /// <summary>
        /// права и владельцы
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            string name = Request.Cookies["username"];
            string password = Request.Cookies["password"];

            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";
            using(var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"SELECT *
                                                FROM gai.people p 
                                                inner join gai.prava pr
                                                on p.id_prav = pr.id", conn);

            }

            return Page();
        }

    }
}
