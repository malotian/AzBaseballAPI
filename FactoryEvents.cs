using System.Data;
using System.Data.SqlClient;

namespace AzBaseballAPI {

    public class FactoryEvents {
        public FactoryDB FactoryDB { get; }
        public List<object> EventsByMonth { get; set; }
        public List<object> ProgramTypes { get; set; }
        public List<object> RelatedEvents { get; set; }
        public Dictionary<int, string> HtmlClassMap { get; }

        public FactoryEvents() {
            FactoryDB = new FactoryDB();
            EventsByMonth = new List<object>();
            ProgramTypes = new List<object>();
            RelatedEvents = new List<object>();
            HtmlClassMap = new Dictionary<int, string>
            {
                { 59, "tryout" },
                { 28, "tryout" },
                { 27, "tryout" },
                { 34, "tryout" },
                { 46, "training-team" },
                { 47, "training-team" },
                { 51, "training-team" },
                { 52, "training-team" }
            };
        }

        public void SetAllEvents(bool isInvitePage = false, string locationState = "") {
            locationState = FactoryDB.ConvertToAlphanumeric(locationState);
            DateTime currentDate = DateTime.Now;
            DateTime toDate = currentDate.AddMonths(11).AddDays(-currentDate.Day + 1).AddMonths(1).AddDays(-1);

            string allEventsQuery = $@"
            SELECT *
            FROM (
                SELECT 
                    ev.event_id, 
                    ev.event_name, 
                    ev.event_start_date, 
                    ev.event_start_time, 
                    ev.event_arrival_time, 
                    ev.event_end_date, 
                    loc.market_city, 
                    loc.location_city, 
                    loc.location_state, 
                    loc.location_zip, 
                    loc.location_name, 
                    ed.session_id, 
                    ev.event_ages_desc, 
                    ev.event_ages_id, 
                    ed.easton_sched_icon, 
                    ed.sched_public_event,  
                    pro.event_program_id, 
                    pro.program_name_friendly, 
                    ed.event_link, 
                    ed.event_note, 
                    ed.event_location_note, 
                    ev.event_price, 
                    ev.fk_location, 
                    ROW_NUMBER() OVER (PARTITION BY fk_event_grouping, CASE WHEN event_ages_id = 2 THEN 2 ELSE 1 END ORDER BY fk_event_program) AS r 
                FROM dbo.tbl_event ev
                JOIN dbo.tbl_location loc ON loc.pk_location = ev.fk_location
                JOIN dbo.tbl_event_data ed ON ed.fk_event = ev.pk_event
                JOIN dbo.tbl_event_program pro ON ev.fk_event_program = pro.event_program_id
                WHERE 
                    pro.program_sport = 'Baseball' 
                    AND ev.add2_public_schedule = 1 
                    AND IsPrepTo = 1 
                    AND isPrepTo2020 = 0 
                    AND ev.event_end_date BETWEEN '{currentDate:yyyy/MM/dd}' AND '{toDate:yyyy/MM/dd}'
            ) AS E 
            WHERE R = 1 
            ORDER BY event_start_date ASC";

            using (SqlConnection connection = FactoryDB.Conn) {
                SqlCommand command = new SqlCommand(allEventsQuery, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    // Process each event
                }
            }
        }

        public List<object> GetEvents(string ageGroup, string sport) {
            List<object> events = new List<object>();

            using (SqlConnection connection = FactoryDB.Conn) {
                // connection.Open();

                string storedProcedureName = (ageGroup == "alums") ? "[dugout].[dbo].[sp_get_ctd_sched]" : "[dugout].[dbo].[sp_get_tryout2020_sched]";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sport", sport);

                    if (ageGroup != "alums") {
                        cmd.Parameters.AddWithValue("@ageGroup", ageGroup);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var @event = new {
                                location_address = reader["location_address"].ToString(),
                                sellable_public = (bool)reader["sellable_public"],
                                event_note = reader["event_note"].ToString(),
                                event_start_date = (DateTime)reader["event_start_date"],
                                event_start_weekday = reader["event_start_weekday"].ToString(),
                                location_state = reader["location_state"].ToString(),
                                location_zip = reader["location_zip"].ToString(),
                                market_city = reader["market_city"].ToString(),
                                location_name = reader["location_name"].ToString(),
                                avail_time_slot_start = reader["avail_time_slot_start"].ToString(),
                                avail_time_slot_end = reader["avail_time_slot_end"].ToString(),
                                g = reader["g"].ToString()

                            };
                            events.Add(@event);
                        }
                    }
                }
            }

            return events;
        }

        public Dictionary<string, object> GetEventZipsByLocation(string zip, string lat, string lng) {
            string procedureName = "[dugout].[dbo].[sp_get_event_registration_location_zips]";
            List<string> eventLocationZips = new List<string>();
            string closestGeolocationZip = string.Empty;

            using (SqlCommand cmd = new SqlCommand(procedureName, FactoryDB.Conn)) {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@zip", zip);
                cmd.Parameters.AddWithValue("@lat", lat);
                cmd.Parameters.AddWithValue("@lng", lng);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        eventLocationZips.Add(reader["LocationZip"].ToString());
                        closestGeolocationZip = zip != null ? zip : reader["ClosestGeolocationZip"].ToString();
                    }
                }

                reader.Close();
            }

            return new Dictionary<string, object> {
                { "EventLocationZips", eventLocationZips },
                { "ClosestGeolocationZip", closestGeolocationZip }
            };
        }


        // Other methods (SetRelatedEvents, GetEventsByMonth, GetEventsByMonthProc, GetEventZipsByLocation) can be similarly converted.
    }

}