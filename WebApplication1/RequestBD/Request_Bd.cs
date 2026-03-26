namespace WebApplication1.RequestBD
{
    public class Request_Bd
    {
        /// <summary>
        /// Выбрать все районы
        /// </summary>
        public string select_district = @"SELECT FROM * gai.district";
        /// <summary>
        /// все полицейские участки
        /// </summary>
        public string select_police_station = @"SELECT FROM * gai.police_station";

        /// <summary>
        /// выбрать все инценденты
        /// </summary>
        public string select_incidents = @"SELECT FROM * gai.incidents";
        /// <summary>
        /// выбрать всех людей
        /// </summary>
        public string select_people = @"SELECT FROM * gai.people";
        /// <summary>
        /// выбрать все машины
        /// </summary>
        public string select_vehicle = @"SELECT FROM * gai.vehicles";
        /// <summary>
        /// выбрать все инценденты из представления
        /// </summary>
        public string select_view_incidents = @"SELECT FROM * gai.incident_full_view";


    }
}
