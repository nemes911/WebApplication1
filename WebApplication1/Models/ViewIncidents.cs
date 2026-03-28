namespace WebApplication1.Models
{
    public class ViewIncidents
    {
        public Guid incident_id { get; set; }

        public int incident_class_id { get; set; }

        public DateOnly incident_date { get; set; }

        public string description { get; set; }

        public decimal repair_cost { get; set; }

        public Guid vehicle_id { get; set; }

        public int serial_number { get; set; }

        public string color { get; set; }

        public Guid owner_id { get; set; }

        public string car_brand { get; set; }

        public string insurance_company { get; set; }

        public string vin { get; set; }

        public ViewIncidents(Guid incident_id, int incident_class_id, DateOnly incident_date, string description, decimal repair_cost, Guid vehicle_id, int serial_number, string color, Guid owner_id, string car_brand, string insurance_company, string vin)
        {
            this.incident_id = incident_id;
            this.incident_class_id = incident_class_id;
            this.incident_date = incident_date;
            this.description = description;
            this.repair_cost = repair_cost;
            this.vehicle_id = vehicle_id;
            this.serial_number = serial_number;
            this.color = color;
            this.owner_id = owner_id;
            this.car_brand = car_brand;
            this.insurance_company = insurance_company;
            this.vin = vin;
        }

        public ViewIncidents() { }
    }


    public class view_onwer_car
    {
        public Guid Vehicle_id { get; set; }
        public int Serial_number { get; set; }
        public string color { get; set; }
        public string car_brand { get; set; }
        public string? insurance_company { get; set; }
        public string vin { get; set; }
        public Guid owner_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string? middle_name { get; set; }
        public int passport_series { get; set; }
        public int passport_number { get; set; }
        public int social_status_id { get; set; }

        public Guid? license_id { get; set; }
        public DateTime? license_issue_date { get; set; }
        public string? license_series { get; set; }
        public int? license_number { get; set; }
        public DateTime? license_expiry_date { get; set; }
        public string? kod_podrazdeleniya { get; set; }
        public string? license_type { get; set; }   // теперь строка
        public string? license_active { get; set; } // теперь строка ("active"/"inactive")
        public string owner_full_name { get; set; }
        public int? license_days_left { get; set; }

        public view_onwer_car(Guid vehicle_id, int serial_number, string color, string car_brand, string? insurance_company, string vin, Guid owner_id, string first_name, string last_name, string? middle_name, int passport_series, int passport_number, int social_status_id, Guid? license_id, DateTime? license_issue_date, string? license_series, int? license_number, DateTime? license_expiry_date, string? kod_podrazdeleniya, string? license_type, string? license_active, string owner_full_name, int? license_days_left)
        {
            Vehicle_id = vehicle_id;
            Serial_number = serial_number;
            this.color = color;
            this.car_brand = car_brand;
            this.insurance_company = insurance_company;
            this.vin = vin;
            this.owner_id = owner_id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.middle_name = middle_name;
            this.passport_series = passport_series;
            this.passport_number = passport_number;
            this.social_status_id = social_status_id;
            this.license_id = license_id;
            this.license_issue_date = license_issue_date;
            this.license_series = license_series;
            this.license_number = license_number;
            this.license_expiry_date = license_expiry_date;
            this.kod_podrazdeleniya = kod_podrazdeleniya;
            this.license_type = license_type;
            this.license_active = license_active;
            this.owner_full_name = owner_full_name;
            this.license_days_left = license_days_left;
        }

        public view_onwer_car() { }
    }

}
