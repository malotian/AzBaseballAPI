using System.Data.SqlClient;

namespace CView.BaseballAPI {
    public class Helper29248 {
        public FactoryDB FactoryDB { get; }
        public Helper29248() {
            FactoryDB = new FactoryDB();
        }
        public List<object> Get() {
            var conn = FactoryDB.Conn;

            List<object> instructors = new List<object>();
            if (conn != null && conn.State == System.Data.ConnectionState.Open) {
                using (SqlCommand cmd = new SqlCommand()) {
                    cmd.Connection = conn;
                    cmd.CommandText = @"select cast(pk_instructor as varchar(38)) as pk_instructor,
                                            instructor_first_name, instructor_last_name,
                                            expertise, convert(text, instructor_bio) as instructor_bio,
                                            general_availability from tbl_instructor
                                            where active_flag=1 and VirtualLessonsInstructor=1 and sports like '%Baseball%'
                                            order by instructor_last_name, instructor_first_name";
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            instructors.Add(new {
                                pk_instructor = reader["pk_instructor"].ToString(),
                                instructor_first_name = reader["instructor_first_name"].ToString(),
                                instructor_last_name = reader["instructor_last_name"].ToString(),
                                expertise = reader["expertise"].ToString(),
                                instructor_bio = reader["instructor_bio"].ToString(),
                                general_availability = reader["general_availability"].ToString()
                            });
                        }
                    }
                }
            }
            return instructors;
        }
    }
}
