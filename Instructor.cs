namespace CView.BaseballAPI {
    public class Instructor {
        public string PkInstructor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Expertise { get; set; }
        public string InstructorBio { get; set; }
        public string GeneralAvailability { get; set; }

        public override string ToString() {
            return $"{PkInstructor}, {FirstName} {LastName}, {Expertise}, {InstructorBio}, {GeneralAvailability}";
        }
    }
}
