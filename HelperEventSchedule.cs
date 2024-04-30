using System.Data;
using System.Data.SqlClient;

namespace AzBaseballAPI {
    public class HelperEventSchedule {
        public FactoryDB FactoryDB { get; }
        public HelperEventSchedule() {
            FactoryDB = new FactoryDB();
        }
        public List<object> GetEvents() {
            List<object> events = new List<object>();
            using (SqlConnection connection = FactoryDB.Conn) {

                SqlCommand command = new SqlCommand("sp_get_upcoming_event_sched", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@sport", "Baseball");

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var newEvent = new {
                        IsPd = Convert.ToBoolean(reader["isPd"]),
                        IsNationalShowcase = Convert.ToBoolean(reader["isNationalShowcase"]),
                        AgeGroup = reader["AgeGroup"].ToString(),
                        EventName = reader["EventName"].ToString(),
                        EventStartDate = Convert.ToDateTime(reader["event_start_date"]).ToString("MM/dd/yy"),
                        LocationAddress = reader["location_address"].ToString(),
                        MarketCity = reader["market_city"].ToString(),
                        LocationState = reader["location_state"].ToString(),
                        LocationName = reader["location_name"].ToString(),
                        LocationZip = reader["location_zip"].ToString(),
                        EventNote = reader["event_note"].ToString(),
                        SellablePublic = Convert.ToBoolean(reader["sellable_public"]),
                        EventLinkPub = reader["event_link_pub"].ToString(),
                        G = reader["g"].ToString()
                    };

                    events.Add(newEvent);
                }
            }

            return events;
        }
    }
}
