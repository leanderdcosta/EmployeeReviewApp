namespace EmployeeReview.Core.Models
{
    public class EmployeeRating
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int SkillID { get; set; }
        public int RatingID { get; set; }
        public string Comments { get; set; }
        public System.DateTime CreateDate { get; set; }

        public Employee Employee { get; set; }
        public Rating Rating { get; set; }
        public Skill Skill { get; set; }
    }
}
