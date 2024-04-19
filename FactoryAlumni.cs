using System.Collections.Generic;
using System.Data.SqlClient;

namespace CView.BaseballAPI {

    public class FactoryAlumni {
        public FactoryDB FactoryDB { get; }
        public List<object> Commitments { get; set; }
        public List<object> College { get; set; }
        public List<object> Drafted { get; set; }

        public FactoryAlumni() {
            FactoryDB = new FactoryDB();
            Commitments = new List<object>();
            College = new List<object>();
            Drafted = new List<object>();
        }

        public void SetCommitmentsData(int currentYear) {
            string divisionClause = currentYear > 2012 ? "AND division='Baseball'" : "";

            string commitmentsQuery = $@"
            SELECT player_last_name, player_first_name, player_city, player_state, high_school, college_commitment, p1.position_name as position1
            FROM tbl_player a
            LEFT OUTER JOIN tbl_player_baseball_position pb ON pb.fk_player=a.pk_player AND primary_flag='1'
            LEFT OUTER JOIN tbl_position p1 ON p1.pk_position=pb.fk_position, tbl_player_academic b
            WHERE a.pk_player=b.fk_player AND b.grad_year='{currentYear}' AND college_commitment<>'' {divisionClause}
            ORDER BY player_last_name, player_first_name, player_city, player_state";

            using (SqlConnection connection = FactoryDB.Conn) {
                SqlCommand command = new SqlCommand(commitmentsQuery, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    Commitments.Add(new {
                        player_last_name = reader["player_last_name"],
                        player_first_name = reader["player_first_name"],
                        player_city = reader["player_city"],
                        player_state = reader["player_state"],
                        high_school = reader["high_school"],
                        college_commitment = reader["college_commitment"],
                        position1 = reader["position1"]
                    });
                }
            }
        }

        public void SetCollegeData() {
            string collegeQuery = @"
            SELECT player_last_name, player_first_name, player_city, player_state, high_school, college_commitment, p1.position_name as position1
            FROM tbl_player a
            LEFT OUTER JOIN tbl_player_baseball_position pb ON pb.fk_player=a.pk_player AND primary_flag='1'
            LEFT OUTER JOIN tbl_position p1 ON p1.pk_position=pb.fk_position, tbl_player_academic b
            WHERE a.pk_player=b.fk_player AND b.grad_year='2015' AND college_commitment<>'' AND division='Baseball'
            ORDER BY player_last_name, player_first_name, player_city, player_state";

            using (SqlConnection connection = FactoryDB.Conn) {
                // connection.Open();
                SqlCommand command = new SqlCommand(collegeQuery, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    College.Add(new {
                        player_last_name = reader["player_last_name"],
                        player_first_name = reader["player_first_name"],
                        player_city = reader["player_city"],
                        player_state = reader["player_state"],
                        high_school = reader["high_school"],
                        college_commitment = reader["college_commitment"],
                        position1 = reader["position1"]
                    });
                }
            }
        }

        public void SetDraftedPlayers(int draftYear) {
            if (draftYear <= 0) {
                draftYear = 0;
            }

            string query = $@"
            SELECT player_first_name, player_last_name, playerid, round_selected, pick_selected, team_selected, 
                   player_city + ', ' + player_state as hometown, high_school, cast(Events as varchar(max)) as Events
            FROM tbl_player_draft pd
            INNER JOIN tbl_player p ON pd.fk_player=p.pk_player
            LEFT OUTER JOIN tbl_player_academic pa ON pa.fk_player=p.pk_player
            LEFT OUTER JOIN [tbl_player_events_registered_without_datetime] per ON per.player_id=pd.fk_player
            WHERE draft_year='{draftYear}'
            ORDER BY dbo.CastAsInt(round_order), dbo.CastAsInt(pick_selected), player_first_name, player_last_name";

            using (SqlConnection connection = FactoryDB.Conn) {
                // connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    Drafted.Add(new {
                        player_first_name = reader["player_first_name"],
                        player_last_name = reader["player_last_name"],
                        playerid = reader["playerid"],
                        round_selected = reader["round_selected"],
                        pick_selected = reader["pick_selected"],
                        team_selected = reader["team_selected"],
                        hometown = reader["hometown"],
                        high_school = reader["high_school"],
                        Events = reader["Events"]
                    });
                }
            }
        }

        ~FactoryAlumni() {
            // Clean up resources
        }
    }

}