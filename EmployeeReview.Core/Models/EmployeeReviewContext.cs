using System.Configuration;
using System.Data.Entity;


namespace EmployeeReview.Core.Models
{
    public class EmployeeReviewContext : DbContext
    {
        private static readonly string ConString = ConfigurationManager.ConnectionStrings["EmployeeReviewContext"].ToString();

        public EmployeeReviewContext() : base(ConString)
        {

        }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeRating> EmployeeRatings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillType> SkillTypes { get; set; }

    }

    public class ToDoDbCreator : DropCreateDatabaseIfModelChanges<EmployeeReviewContext>
    {

    }
}
