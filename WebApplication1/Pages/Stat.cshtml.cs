using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using Dapper;
using WebApplication1.Bd_Context;
using Npgsql;

namespace WebApplication1.Pages
{
    public class StatModel : PageModel
    {
        [BindProperty]
        public List<DistrictCount> districtcountall { get; set; }

        [BindProperty]
        public List<DistrictCount> districtCount { get; set; }

        [BindProperty]
        public List<DistrictCount> districtCountsMin { get; set; }

        [BindProperty]
        public List<DistrictCount> districtcount_date_and_min { get; set; }

        [BindProperty]
        public List<DistrictCount> district_count_subject_agregate { get; set; }

        [BindProperty]
        public List<Incident> agregate_with_subquery { get; set; }

        [BindProperty]
        public List<ViewIncidents> range_view_incidents { get; set; }

        public List<JoinPrava> join_prava {  get; set; }

        public List<Vehicle> vehicles { get; set; } // для join prava 

       // public List<BrandCount> brandCount { get; set; }
       // public List<InsuranceStats> insuranceStats { get; set; }

        public void OnGet()
        {
            // пустая загрузка
        }

        public IActionResult OnPostGetIncidentByVehicle(Vehicle vehicle)
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

                cmd.Parameters.AddWithValue("vehicle_id", vehicle.Id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        join_prava.Add(new JoinPrava
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
                return Page(); ;
            }
        }

        public IActionResult OnPostRangeView(DateOnly dateFrom, DateOnly dateTo)
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("select * from gai.incident_full_view where incident_date BETWEEN @dateFrom AND @dateTo", conn);
                cmd.Parameters.AddWithValue("dateFrom", dateFrom);
                cmd.Parameters.AddWithValue("dateTo", dateTo);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        range_view_incidents.Add(new ViewIncidents
                        {
                            incident_id = reader.GetGuid(0),
                            incident_class_id = reader.GetInt32(1),
                            incident_date = reader.GetFieldValue<DateOnly>(2),
                            description = reader.GetString(3),
                            repair_cost = reader.GetDecimal(4),
                            vehicle_id = reader.GetGuid(5),
                            serial_number = reader.GetInt32(6),
                            color = reader.GetString(7),
                            owner_id = reader.GetGuid(8),
                            car_brand = reader.GetString(9),
                            insurance_company = reader.GetString(10),
                            vin = reader.GetString(11)
                        });
                    }
                }
            }
            return Page();
        }

        public IActionResult OnPostAgregate_with_subquery()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
        select
            id,
            incident_class_id,
            incident_date,
            description,
            repair_cost
        from gai.incidents
        where repair_cost >
        (
            select avg(repair_cost)
            from gai.incidents
        )
        ", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        agregate_with_subquery.Add(new Incident
                        {
                            Id = reader.GetGuid(0),
                            IncidentClassId = reader.GetInt32(1),
                            IncidentDate = reader.GetFieldValue<DateOnly>(2),
                            Description = reader.GetString(3),
                            RepairCost = reader.IsDBNull(2) ? 0 : reader.GetDecimal(4)
                        });
                    }
                }
            }
            return Page();
        }

        public IActionResult OnPostSubjectAgregate()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select *
                from (
                        select 
                            ps.district_id,
                            count(i.id) as incidents_count,
                            sum(i.repair_cost) as total_damage
                        from gai.incidents i
                        inner join gai.police_station ps on i.police_station_id = ps.id
                        group by ps.district_id
                ) t
                order by total_damage desc", conn);

                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        district_count_subject_agregate.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
            }
            return Page();
        }

        public IActionResult OnPostGetAllDistrictCount()
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                select
                        ps.district_id,
                        count(i.id) as incident_count,
                        sum(i.repair_cost) as total_damage
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                group by ps.district_id", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        districtcountall.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
            }
            return Page();
        }

        public IActionResult OnPostDistrictDateToMin(DateOnly from, DateOnly to, decimal minDamage)
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select
                        ps.district_id,
                        count(i.id),
                        sum(i.repair_cost)
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                where i.incident_date between @from and @to
                group by ps.district_id 
                having sum(i.repair_cost) >= @minDamage", conn);

                cmd.Parameters.AddWithValue("from", from);
                cmd.Parameters.AddWithValue("to", to);
                cmd.Parameters.AddWithValue("minDamage", minDamage);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        districtcount_date_and_min.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
            }
            return Page();
        }


        public IActionResult OnPostDistrictMinGroup(int min)
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select 
                        ps.district_id,
                        count(i.id),
                        sum(i.repair_cost)
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                group by ps.district_id
                having count(i.id) >= @minIncidents", conn);

                cmd.Parameters.AddWithValue("minIncidents", min);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        districtCountsMin.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
            }
            return Page();
        }

        /// <summary>
        /// общая информация инцендентов по району
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        public IActionResult OnPostDistrictCount(DateOnly dateFrom)
        {
            var name = Request.Cookies["username"];
            var password = Request.Cookies["password"];
            var constring = $"Host=localhost;Database=postgres;Username={name};Password={password}";

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                select
                    ps.district_id,
                    count(i.id),
                    sum(i.repair_cost)
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                where i.incident_date >= @dateFrom
                group by ps.district_id
                ", conn);

                cmd.Parameters.AddWithValue("dateFrom", dateFrom);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        districtCount.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
            }

                return Page();
        }

       
    }
}
