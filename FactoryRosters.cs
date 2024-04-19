using CView.BaseballAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CView.BaseballAPI {
    public class FactoryRosters {
        public FactoryDB FactoryDB { get; private set; }
        public List<Player> Top100Roster { get; private set; } = new List<Player>();
        public List<Player> AllAmericanRoster { get; private set; } = new List<Player>();
        public List<Player> AllAmericanALRoster { get; private set; } = new List<Player>();
        public List<Player> AllAmericanNLRoster { get; private set; } = new List<Player>();
        public List<Player> PreSeasonRoster { get; private set; } = new List<Player>();

        public FactoryRosters() {
            FactoryDB = new FactoryDB();
        }

        public void SetTop100RosterByYear(string top100Year) {
            // Implementation here
        }

        public void GetTop100RosterByEventId(string eventId) {
            eventId = FactoryDB.ConvertToGuid(eventId);
            string top100Query = @"SELECT PublicPlayerPageId, player_first_name, player_last_name, player_city, player_state, playerid, grad_year, college_commitment, high_school, player_height, player_weight, bats_name, throws_name, player_number, player_event_id, position1, position2, pe.disable_evaluation, cast(overall as varchar(max)) as overall, pe.disable_video, COALESCE(dbo.empty2Null(dummy_field2), rs.cdn_url_ssl + '/' + cast(event_id as varchar(10)) + '-' + player_first_name + player_last_name + '.mp4' ) as cdn_url 
                                FROM tbl_player_event pe 
                                INNER JOIN tbl_player p ON p.pk_player=pe.fk_player 
                                LEFT OUTER JOIN tbl_player_data pd ON pd.fk_player=p.pk_player 
                                LEFT OUTER JOIN tbl_player_academic pa ON pa.fk_player=p.pk_player 
                                LEFT OUTER JOIN tbl_player_eval_overall ov ON ov.fk_player_event=pe.pk_player_event 
                                LEFT OUTER JOIN tbl_player_baseball pb ON pb.fk_player=p.pk_player 
                                LEFT OUTER JOIN tbl_throws t ON t.pk_throws = pb.fk_throws 
                                LEFT OUTER JOIN tbl_bats b ON b.pk_bats = pb.fk_bats 
                                INNER JOIN tbl_event e on e.pk_event=pe.fk_event 
                                LEFT OUTER JOIN tbl_event_rackspace_cloud_files_cdn_url rs ON rs.event_year_month = CONVERT(VARCHAR(7), event_start_date, 120) 
                                WHERE fk_player_event_status ='E4024702-66F7-4C1A-99BF-78D7B1AF847A' 
                                AND pe.fk_event = @EventID and p.active_flag=1 and isNumeric(player_number)=1 
                                ORDER BY dbo.CastAsInt(player_number), player_first_name, player_last_name";

            using (SqlConnection connection = FactoryDB.Conn) {
                // // connection.Open();
                SqlCommand command = new SqlCommand(top100Query, connection);
                command.Parameters.AddWithValue("@EventID", eventId);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    Player player = new Player {
                        PublicPlayerPageId = reader["PublicPlayerPageId"].ToString(),
                        PlayerFirstName = reader["player_first_name"].ToString(),
                        PlayerLastName = reader["player_last_name"].ToString(),
                        PlayerCity = reader["player_city"].ToString(),
                        PlayerState = reader["player_state"].ToString(),
                        PlayerId = reader["playerid"].ToString(),
                        GradYear = reader["grad_year"].ToString(),
                        CollegeCommitment = reader["college_commitment"].ToString(),
                        HighSchool = reader["high_school"].ToString(),
                        PlayerHeight = reader["player_height"].ToString(),
                        PlayerWeight = reader["player_weight"].ToString(),
                        BatsName = reader["bats_name"].ToString(),
                        ThrowsName = reader["throws_name"].ToString(),
                        PlayerNumber = reader["player_number"].ToString(),
                        PlayerEventId = reader["player_event_id"].ToString(),
                        Position1 = reader["position1"].ToString(),
                        Position2 = reader["position2"].ToString(),
                        DisableEvaluation = reader["disable_evaluation"].ToString(),
                        Overall = reader["overall"].ToString(),
                        DisableVideo = reader["disable_video"].ToString(),
                        CdnUrl = reader["cdn_url"].ToString(),
                        HeadshotUrl = $"https://members.baseballfactory.com/assets/PlayerHeadshots/{reader["player_last_name"].ToString()[0]}/{reader["player_first_name"].ToString()}{reader["player_last_name"].ToString()}{reader["playerid"].ToString()}.jpg"
                    };
                    Top100Roster.Add(player);
                }
            }
        }

        public void SetAllAmericanRoster(string eventId) {
            eventId = FactoryDB.ConvertToGuid(eventId);
            string allAmericanQuery = @"SELECT team, player_number, playerid, player_last_name, player_first_name, position1, position2, player_city + ', ' + player_state as hometown, high_school, 
                                    college_commitment, player_height, player_weight, grad_year, CONVERT(TEXT, overall) as overall, coalesce(dummy_field1,'') AS dummy_field1 
                                    FROM tbl_player_event pe 
                                    INNER JOIN tbl_player p ON p.pk_player = pe.fk_player 
                                    LEFT OUTER JOIN tbl_player_academic pa ON pa.fk_player=p.pk_player 
                                    LEFT OUTER JOIN tbl_player_eval_overall ov ON ov.fk_player_event = pe.pk_player_event 
                                    WHERE fk_player_event_status = 'E4024702-66F7-4C1A-99BF-78D7B1AF847A' 
                                    AND pe.fk_event = @EventID 
                                    ORDER BY player_last_name, player_first_name";

            using (SqlConnection connection = FactoryDB.Conn) {

                SqlCommand command = new SqlCommand(allAmericanQuery, connection);
                command.Parameters.AddWithValue("@EventID", eventId);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    Player player = new Player {
                        Team = reader["team"].ToString(),
                        PlayerNumber = reader["player_number"].ToString(),
                        PlayerId = reader["playerid"].ToString(),
                        PlayerLastName = reader["player_last_name"].ToString(),
                        PlayerFirstName = reader["player_first_name"].ToString(),
                        Position1 = reader["position1"].ToString(),
                        Position2 = reader["position2"].ToString(),
                        Hometown = reader["hometown"].ToString(),
                        HighSchool = reader["high_school"].ToString(),
                        CollegeCommitment = reader["college_commitment"].ToString(),
                        PlayerHeight = reader["player_height"].ToString(),
                        PlayerWeight = reader["player_weight"].ToString(),
                        GradYear = reader["grad_year"].ToString(),
                        Overall = reader["overall"].ToString().Replace('�', '\''),
                        DummyField1 = reader["dummy_field1"].ToString(),
                        HeadshotUrl = $"https://members.baseballfactory.com/assets/PlayerHeadshots/{reader["player_last_name"].ToString()[0]}/{reader["player_first_name"].ToString()}{reader["player_last_name"].ToString()}{reader["playerid"].ToString()}.jpg"
                    };
                    AllAmericanRoster.Add(player);
                }
            }
        }

        public void SetAllALNLRosters() {
            foreach (var player in AllAmericanRoster) {
                switch (player.Team) {
                    case "American":
                        AllAmericanALRoster.Add(player);
                        break;
                    case "National":
                        AllAmericanNLRoster.Add(player);
                        break;
                }
            }
        }

        public void SetPreseasonRoster(string eventId) {
            eventId = FactoryDB.ConvertToGuid(eventId);
            string query = @"select player_first_name, player_last_name, player_city + ', ' + player_state as hometown, grad_year, high_school FROM tbl_player_event pe 
                         INNER JOIN tbl_player p ON p.pk_player=pe.fk_player 
                         LEFT OUTER JOIN tbl_player_academic pa ON pa.fk_player=p.pk_player 
                         WHERE fk_player_event_status ='E4024702-66F7-4C1A-99BF-78D7B1AF847A' 
                         AND pe.fk_event = @EventID and p.active_flag=1 
                         ORDER BY player_last_name, player_first_name";

            using (SqlConnection connection = FactoryDB.Conn) {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventID", eventId);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    Player player = new Player {
                        PlayerFirstName = reader["player_first_name"].ToString(),
                        PlayerLastName = reader["player_last_name"].ToString(),
                        Hometown = reader["hometown"].ToString(),
                        GradYear = reader["grad_year"].ToString(),
                        HighSchool = reader["high_school"].ToString()
                    };
                    PreSeasonRoster.Add(player);
                }
            }
        }
    }
}