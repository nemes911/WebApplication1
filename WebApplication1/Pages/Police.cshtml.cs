using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using Npgsql;

namespace WebApplication1.Pages
{
    public class PoliceModel : PageModel
    {
        [BindProperty]
        public PoliceDepartment department {  get; set; }

        [BindProperty]
        public List<DistrictAndPoliceStation> districtAndPoliceStations { get; set; }

        [BindProperty]
        public List<PravaAndOwnercs> pravaAndOwnercs { get; set; }

        [BindProperty]
        public List<JoinPrava> joinPravas { get; set; }

        [BindProperty]
        public List<IncidentOfficerDto> incidentOfficerDtos { get; set; }

        [BindProperty]
        public List<Officer> oficers { get; set; }
        public void OnGet()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";
            var list = new List<DistrictAndPoliceStation>();
            var dep = new PoliceDepartment();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"SELECT 
    d.id AS district_id,
    d.""name"" AS district_name,
    ps.id AS police_station_id,
    ps.address,
    ps.phone
FROM gai.district d
INNER JOIN gai.police_station ps 
    ON d.id = ps.district_id;", conn);

                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        list.Add(new DistrictAndPoliceStation
                        {
                            Id = reader.GetInt32(0),          // district_id
                            Name = reader.GetString(1),       // district_name
                            idPolice = reader.GetInt32(2),    // police_station_id
                            idstrict = reader.GetInt32(0),    // если ты хотел хранить district_id ещё раз
                            address = reader.GetString(3),    // ps.address
                            phone = reader.GetString(4)       // ps.phone
                        });

                    }
                }
            }
            districtAndPoliceStations = list;
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
        SELECT 
            pd.id AS department_id,
            pd.district_id,
            d.""name"" AS district_name,
            pd.chief_first_name,
            pd.chief_last_name,
            pd.chief_middle_name,
            pd.address
        FROM gai.police_department pd
        INNER JOIN gai.district d 
            ON pd.district_id = d.id
        LIMIT 1;", conn);   

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())   
                    {
                        
                        dep.District ??= new District();

                        dep.Id = reader.GetInt32(0);   
                        dep.DistrictId = reader.GetInt32(1);
                        dep.ChiefFirstName = reader.GetString(3);
                        dep.ChiefLastName = reader.GetString(4);
                        dep.ChiefMiddleName = reader.IsDBNull(5) ? null : reader.GetString(5);
                        dep.Address = reader.GetString(6);

                        dep.District.Id = reader.GetInt32(1);   
                        dep.District.Name = reader.GetString(2);  
                    }
                }
            }
            department = dep;
        }


        /// <summary>
        /// right join 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostGetRightJoinOfficersWithIncidents()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                var cmd = new NpgsqlCommand(@"
                SELECT o.id, o.first_name, o.last_name, o.middle_name, o.rank_id,
                       o.birth_date, o.passport_number, o.passport_series
                FROM gai.incidents i
                RIGHT JOIN gai.incident_officers io ON i.id = io.incident_id
                RIGHT JOIN gai.officers o ON io.officer_id = o.id", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oficers.Add(new Officer
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            MiddleName = reader.GetString(3),
                            RankId = reader.GetInt32(4),
                            BirthDate = reader.GetFieldValue<DateOnly>(5),
                            PassportNumber = reader.GetInt32(6),
                            PassportSeries = reader.GetInt32(7)
                        });
                    }
                }
            }
            return Page();
        }
        /// <summary>
        /// запрос на запросе по ринципу left 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostJoinjoinLeft()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
        SELECT 
            sub.id AS incident_id,
            sub.incident_date,
            o.first_name || ' ' || o.last_name AS officer_name,
            r.rank_name
        FROM (
            SELECT i.id, i.incident_date, io.officer_id
            FROM gai.incidents i
            LEFT JOIN gai.incident_officers io ON i.id = io.incident_id
        ) AS sub
        LEFT JOIN gai.officers o ON sub.officer_id = o.id
        LEFT JOIN gai.ranks r ON o.rank_id = r.id
        WHERE sub.officer_id = @officer_id", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        incidentOfficerDtos.Add(new IncidentOfficerDto
                        {
                            IncidentId = reader.GetGuid(0),
                            IncidentDate = reader.GetFieldValue<DateOnly>(1),
                            OfficerName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            RankName = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }
            return Page();
        }

        /// <summary>
        /// incidentofficerdto 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostIncidentOfficer()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"SELECT i.id, i.incident_date,
                       o.first_name || ' ' || o.last_name AS officer_name,
                       r.rank_name
                FROM gai.incidents i
                LEFT JOIN gai.incident_officers io ON i.id = io.incident_id
                LEFT JOIN gai.officers o ON io.officer_id = o.id
                LEFT JOIN gai.ranks r ON o.rank_id = r.id", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        incidentOfficerDtos.Add(new IncidentOfficerDto
                        {
                            IncidentId = reader.GetGuid(0),
                            IncidentDate = reader.GetFieldValue<DateOnly>(1),
                            OfficerName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            RankName = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }
            return Page();
        }


        public IActionResult OnPostPravas()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                    select
                        p.date, p.series, p.number, p.kod_podrazdeleniya, p.type, p.status,
                        per.first_name, per.last_name, per.middle_name,
                        per.passport_number, per.passport_series,
                        v.serial_number, v.color, v.car_brand, v.insurance_company, v.vin
                    from gai.incidents i
                    inner join gai.incident_vehicles iv on i.id = iv.incident_id
                    inner join gai.vehicles v on iv.vehicle_id = v.id
                    inner join gai.people per on v.owner_id = per.id
                    inner join gai.prava p on per.id_prav = p.id
                    where v.id = @vehicle_id", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        joinPravas.Add(new JoinPrava
                        {
                            prava = new Prava
                            {
                                date = reader.GetDateTime(0),
                                series = reader.GetString(1),
                                number = reader.GetInt32(2),
                                kod_podrazdeleniya = reader.GetString(3),
                                type = reader.GetFieldValue<string[]>(4),
                                status = reader.GetBoolean(5)
                            },
                            Person = new Person
                            {
                                FirstName = reader.GetString(6),
                                LastName = reader.GetString(7),
                                MiddleName = reader.GetString(8),
                                PassportNumber = reader.GetInt32(9),
                                PassportSeries = reader.GetInt32(10)
                            },
                            Vehicle = new Vehicle
                            {
                                SerialNumber = reader.GetInt32(11),
                                Color = reader.GetString(12),
                                CarBrand = reader.GetString(13),
                                Insurance_company = reader.GetString(14),
                                Vin = reader.GetString(15)
                            }
                        });
                    }
                }
            }
            return Page();   
        }

        public IActionResult OnPost()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";
            using(var conn = new NpgsqlConnection( constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"SELECT *  
                                                FROM gai.people p 
                                                    inner join gai.prava pr
                                                    on p.id_prav = pr.id",conn);
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pravaAndOwnercs.Add(new PravaAndOwnercs
                        {
                            id = reader.GetGuid(0),
                            date = reader.GetDateTime(1),
                            series = reader.GetString(2),
                            number = reader.GetInt32(3),
                            date_end = reader.GetDateTime(4),
                            kod_podrazdeleniya = reader.GetString(5),
                            type = reader.GetFieldValue<string[]>(6),
                            person  = new Person
                            {
                                Id = reader.GetGuid(7),
                                FirstName = reader.GetString(8),
                                LastName = reader.GetString(9),
                                MiddleName = reader.GetString(10),
                                PassportNumber = reader.GetInt32(11),
                                PassportSeries = reader.GetInt32(12),
                                SocialStatusId = reader.GetInt32(13),
                                id_prav = reader.GetGuid(14)
                            }
                        });
                    }
                }
            }
            return Page();
        }


    }
}
