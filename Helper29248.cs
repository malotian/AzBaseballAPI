using System.Data.SqlClient;

namespace CView.BaseballAPI {
    public class Helper29248 {
        public static List<Instructor> Get() {
            var conn = DB.Instance.GetConnection();

            List<Instructor> instructors = new List<Instructor>();
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
                            Instructor instructor = new Instructor {
                                PkInstructor = reader["pk_instructor"].ToString(),
                                FirstName = reader["instructor_first_name"].ToString(),
                                LastName = reader["instructor_last_name"].ToString(),
                                Expertise = reader["expertise"].ToString(),
                                InstructorBio = reader["instructor_bio"].ToString(),
                                GeneralAvailability = reader["general_availability"].ToString()
                            };
                            instructors.Add(instructor);

                            Console.WriteLine(instructor);
                        }
                    }
                }
            }
            return instructors;
        }
    }
}
