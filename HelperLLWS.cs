
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CView.AzFunction
{
    public class HelperLLWS
    {

        string llws_directory = "/little-league-events/llws_t/";

        Dictionary<string, string> wp_query = new Dictionary<string, string>
        {
            { "qy", "" },
            { "qt", "" },
            { "qp", "" },
        };

        string templateDirectory = "get_bloginfo('template_directory')";
        string qy = wp_query["qy"].Substring(0, Math.Min(wp_query["qy"].Length, 4));
        string qt = wp_query["qt"].Substring(0, Math.Min(wp_query["qt"].Length, 50)).ToLower();
        string qp = wp_query["qp"].Substring(0, Math.Min(wp_query["qp"].Length, 101)).ToLower();
        string llws_page_title = $"Baseball Factory - Little League Baseball World Series {qy}";
        string llws_directory = "/little-league-events/llws_t/";
        bool qy_found = false;
        string latest_year = "";

        Dictionary<string, Dictionary<string, string>> llws_info = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "2018", new Dictionary<string, string>
                {
                    { "dateDesc", "August 16 to August 26" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2018/08/LL_Logo2018.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2018/08/LLWS_Placeholder_2018.jpg" },
                    { "cdn_url", "" },
                    { "introText", "<h3>Official Baseball Factory Player Profiles</h3>Baseball Factory, the Official Player Development Partner of Little League Baseball<sup>&reg;</sup>, is thrilled to be back at the Little League Baseball<sup>&reg;</sup> World Series to create videos and scouting reports for each participant." }
                }
            },
            {
                "2017", new Dictionary<string, string>
                {
                    { "dateDesc", "August 17 to August 27" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2017/08/LL_Logo2017.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2017/08/LLWS_Placeholder_2017.jpg" },
                    { "cdn_url", "" },
                    { "introText", "<h3>Official Baseball Factory Player Profiles</h3>Baseball Factory, the Official Player Development Partner of Little League Baseball<sup>&reg;</sup>, is thrilled to be back at the Little League Baseball<sup>&reg;</sup> World Series to create videos and scouting reports for each participant." }
                }
            },
            {
                "2016", new Dictionary<string, string>
                {
                    { "dateDesc", "August 18 to August 28" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2016/07/LL_Logo2016.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2016/07/LLWS_Placeholder_2016.jpg" },
                    { "cdn_url", "" },
                    { "introText", "<h3>Official Baseball Factory Player Profiles</h3>Baseball Factory, the Official Player Development Partner of Little League Baseball<sup>&reg;</sup>, is thrilled to be back at the Little League Baseball<sup>&reg;</sup> World Series to create videos and scouting reports for each participant." }
                }
            },
            {
                "2015", new Dictionary<string, string>
                {
                    { "dateDesc", "August 20 to August 30" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LL_Logo2015.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LLWS_Placeholder_2015.jpg" },
                    { "cdn_url", "" },
                    { "introText", "<h3>Official Baseball Factory Player Profiles</h3>Baseball Factory, the Official Player Development Partner of Little League Baseball<sup>&reg;</sup>, is thrilled to be back at the Little League Baseball<sup>&reg;</sup> World Series to create videos and scouting reports for each participant." }
                }
            },
            {
                "2014", new Dictionary<string, string>
                {
                    { "dateDesc", "August 14 to August 24" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LL_Logo2014.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LLWS_Placeholder_2014.jpg" },
                    { "cdn_url", "" },
                    { "introText", "" }
                }
            },
            {
                "2013", new Dictionary<string, string>
                {
                    { "dateDesc", "August 15 to August 25" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LL_Logo2013.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LLWS_Placeholder_2013.jpg" },
                    { "cdn_url", "" },
                    { "introText", "" }
                }
            },
            {
                "2012", new Dictionary<string, string>
                {
                    { "dateDesc", "August 16 to August 26" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LL_Logo2012.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LLWS_Placeholder_2012.jpg" },
                    { "cdn_url", "" },
                    { "introText", "" }
                }
            },
            {
                "2011", new Dictionary<string, string>
                {
                    { "dateDesc", "August 18 to August 28" },
                    { "heroImg", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LamadeStadium.jpg" },
                    { "logo", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LL_Logo2011.png" },
                    { "placeholder", "https://www.baseballfactory.com/wp/wp-content/uploads/2015/07/LLWS_Placeholder_2011.jpg" },
                    { "cdn_url", "" },
                    { "introText", "" }
                }
            },
            {
                "playerinfo", null
            }
        };
        public object? Testllws()
        {

            List<PlayerInfo> playerInfos = new List<PlayerInfo>();

            // Database connection and data retrieval logic
            using (SqlConnection connection = DB.Instance.GetConnection())
            {
                connection.Open();

                string query_llws_years = @"select distinct year(event_start_date) as [year],  cdn_url_ssl from tbl_event e 
                                            left outer join tbl_event_rackspace_cloud_files_cdn_url rs on rs.event_year_month = CONVERT(VARCHAR(7), event_start_date, 120) 
                                            where fk_event_program=36 and fk_event_type='58EB0C5B-300A-40B4-BDC1-B262D25D9A1E' and month(event_start_date)=8 order by [year] desc";

                using (SqlCommand command = new SqlCommand(query_llws_years, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return (llws_info, playerInfos);
                        }

                        while (reader.Read())
                        {
                            string latest_year = reader.GetString(reader.GetOrdinal("year"));
                            llws_info[latest_year]["cdn_url"] = reader.GetString(reader.GetOrdinal("cdn_url_ssl"]);
                        }
                    }
                }

                string query_teams = $@"select event_name, pk_event, row_number() over(order by event_name)-1 as row from tbl_event 
                                        where fk_event_program=36 and fk_event_type='58EB0C5B-300A-40B4-BDC1-B262D25D9A1E' and month(event_start_date)=8 and year(event_start_date)='{qy}'";

                using (SqlCommand command = new SqlCommand(query_teams, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string pk_event = reader.GetString(reader.GetOrdinal("pk_event"));
                            string event_name = reader.GetString(reader.GetOrdinal("event_name"));
                            string event_name_f = FormatTeamPlayerName(event_name);
                            string teamSelected = "";

                            // Assuming stored procedure sp_get_llws_players_by_event is executed and returns the necessary data
                            string query_team_players = $"sp_get_llws_players_by_event '{pk_event}'";

                            using (SqlCommand cmdPlayers = new SqlCommand(query_team_players, connection))
                            {
                                using (SqlDataReader readerPlayers = cmdPlayers.ExecuteReader())
                                {
                                    while (readerPlayers.Read())
                                    {
                                        string player_first_name = readerPlayers.GetString(reader.GetOrdinal("player_first_name"));
                                        string player_last_name = readerPlayers.GetString(reader.GetOrdinal("player_last_name"));
                                        string player_name_f = FormatTeamPlayerName($"{player_first_name}-{player_last_name}");
                                        string playerSelected = "";
                                        string playing = "";

                                        if (event_name_f == qt && player_name_f == qp)
                                        {
                                            playerSelected = " pSelected";
                                            playing = "Playing";
                                            llws_info["playerinfo"]["team"] = event_name;
                                        }

                                        string PlayerPageLink = $"{llws_directory}{qy}/{event_name_f}/{player_name_f}/";
                                        PlayerPageLink = readerPlayers.GetString(reader.GetOrdinal("event_completed")) == "True" ? PlayerPageLink : "#";
                                        playing = readerPlayers.GetString(reader.GetOrdinal("event_completed")) == "True" ? playing : "";
                                        string player_no = readerPlayers.GetString(reader.GetOrdinal("player_number")).Trim();
                                        player_no = string.IsNullOrEmpty(player_no) ? "&nbsp;" : player_no;

                                        PlayerInfo playerInfo = new PlayerInfo
                                        {
                                            PE_ID = int.Parse(readerPlayers.GetString(reader.GetOrdinal("PE_ID"))),
                                            PlayerPageLink = PlayerPageLink,
                                            PlayerSelected = playerSelected,
                                            PlayerNo = player_no,
                                            PlayerFirstName = player_first_name,
                                            PlayerLastName = player_last_name,
                                            Playing = playing
                                        };

                                        playerInfos.Add(playerInfo);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in llws_info)
                {
                    if (int.TryParse(kvp.Key, out int year) && year != int.Parse(qy))
                    {
                        // Add other years to llws_info dictionary if needed
                        llws_info[kvp.Key]["newProperty"] = "newValue";
                    }
                }
            }

            return (llws_info, playerInfos);
        }

        public string FormatTeamPlayerName(string tp)
        {
            if (string.IsNullOrEmpty(tp))
            {
                return "";
            }

            tp = tp.ToLower();
            tp = System.Text.RegularExpressions.Regex.Replace(tp, @"[^a-z0-9-]", " ");
            tp = tp.Trim();
            tp = System.Text.RegularExpressions.Regex.Replace(tp, @"\s+", "-");
            
            return tp;
        }
    }
}
