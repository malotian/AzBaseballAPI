using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CView.BaseballAPI {

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

        // Other methods (SetRelatedEvents, GetEventsByMonth, GetEventsByMonthProc, GetEventZipsByLocation) can be similarly converted.
    }

}