using System.Data;
using System.Data.SqlClient;

namespace CView.BaseballAPI {
    public class HelperNationalShowcases {
        public FactoryDB FactoryDB { get; }
        public HelperNationalShowcases() {
            FactoryDB = new FactoryDB();
        }

        public static string GetGradYears(string ag) {
            ag = ag.ToLower();
            ag = (new string[] { "hs", "ms" }.Contains(ag)) ? ag : "hs";
            int gradYearStart = DateTime.Now.Year;
            gradYearStart += (ag == "hs") ? 0 : 4;
            int gradYearAdd = 3;
            int currentMonth = DateTime.Now.Month;
            gradYearStart += (currentMonth >= 6) ? 1 : 0;
            int gradYearEnd = gradYearStart + gradYearAdd;
            string gradYears = gradYearStart.ToString() + "-" + gradYearEnd.ToString();
            return gradYears;
        }

        public List<object> GetShowcases(string ageGroup, string st) {
            List<object> events = new List<object>();

            bool isValidAgeGroup = false; // Implement your logic to validate age group
            bool isStatePage = !string.IsNullOrEmpty(st);
            string stateAbbr = ""; // Implement logic to get state abbreviation based on st
            bool isValidState = false; // Implement your logic to validate state abbreviation

            using (SqlConnection connection = FactoryDB.Conn) {
                SqlCommand command = new SqlCommand("sp_get_national_showcases_sched", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@sport", "Baseball");
                command.Parameters.AddWithValue("@ageGroup", ageGroup);
                command.Parameters.AddWithValue("@isStatePage", isStatePage);
                command.Parameters.AddWithValue("@State", stateAbbr);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var newEvent = new {
                        IsPd = Convert.ToBoolean(reader["isPd"]),
                        IsNationalShowcase = Convert.ToBoolean(reader["isNationalShowcase"]),
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
                        G = reader["g"].ToString(),
                        AvailTimeSlotStart = reader["avail_time_slot_start"].ToString(),
                        AvailTimeSlotEnd = reader["avail_time_slot_end"].ToString()
                    };

                    events.Add(newEvent);
                }
            }

            return events;
        }
    }
}
