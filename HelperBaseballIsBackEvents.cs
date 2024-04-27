using System.Data;
using System.Data.SqlClient;

namespace CView.BaseballAPI {
    public class HelperBaseballIsBackEvents {
        public FactoryDB FactoryDB { get; }
        public HelperBaseballIsBackEvents() {
            FactoryDB = new FactoryDB();
        }
        public List<object> GetEvents(string ageGroup) {
            List<object> events = new List<object>();

            using (SqlConnection connection = FactoryDB.Conn) {
                SqlCommand command;
                if (ageGroup == "alums") {
                    command = new SqlCommand("sp_get_ctd_sched", connection);
                } else {
                    command = new SqlCommand("sp_get_tryout2020_sched", connection);
                    command.Parameters.AddWithValue("@ageGroup", ageGroup);
                }
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@sport", "Baseball");

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var newEvent = new {
                        EventStartDate = Convert.ToDateTime(reader["event_start_date"]).ToString("MM/dd/yy"),
                        EventStartWeekday = reader["event_start_weekday"].ToString(),
                        LocationState = reader["location_state"].ToString(),
                        MarketCity = reader["market_city"].ToString(),
                        LocationName = reader["location_name"].ToString(),
                        LocationAddress = reader["location_address"].ToString(),
                        LocationZip = reader["location_zip"].ToString(),
                        AvailTimeSlotStart = reader["avail_time_slot_start"].ToString(),
                        AvailTimeSlotEnd = reader["avail_time_slot_end"].ToString(),
                        EventNote = reader["event_note"].ToString(),
                        SellablePublic = Convert.ToBoolean(reader["sellable_public"]),
                        G = reader["g"].ToString()
                    };

                    events.Add(newEvent);
                }
            }

            return events;
        }
    }
}
