using EmployeeReview.Core.Models;

namespace EmployeeReview.Core
{
    public class Initialize
    {
        public void Start()
        {
            EmployeeReviewContext db = new EmployeeReviewContext();

            string[] skillTypeArray = { "Developer", "Technical" };
            foreach (var item in skillTypeArray)
            {
                SkillType skillType = new SkillType();
                skillType.SkillTypeName = item;
                db.SkillTypes.Add(skillType);
                db.SaveChanges();
            }

            string[] designationArray = { "Junior Developer", "Developer", "Senior Developer", "Team Lead", "Senior Team Lead", "Project Manager" };
            foreach (var item in designationArray)
            {
                Designation desgination = new Designation();
                desgination.DesignationName = item;
                db.Designations.Add(desgination);
                db.SaveChanges();
            }

            string[] skillArray1 = { "Coding Skills", "Troubleshooting Skills", "Quality Assurance", "Time Logging" };
            foreach (var item in skillArray1)
            {
                Skill skill = new Skill();
                skill.SkillsName = item;
                skill.SkillTypeID = 1;
                db.Skills.Add(skill);
                db.SaveChanges();
            }
            string[] skillArray2 = { "Source Versioning - TFS", "Source Versioning - GIT", "c#", "ASP.NET CORE", "Ruby", "Angular JS" };
            foreach (var item in skillArray2)
            {
                Skill skill = new Skill();
                skill.SkillsName = item;
                skill.SkillTypeID = 2;
                db.Skills.Add(skill);
                db.SaveChanges();
            }

            string[] ratingsArray1 = { "Outstanding", "Above Expectations", "Meets Expectations", "Needs Improvement", "Unsactisfactory" };
            foreach (var item in ratingsArray1)
            {
                Rating rating = new Rating();
                rating.RatingsName = item;
                rating.SkillTypeID = 1;
                db.Ratings.Add(rating);
                db.SaveChanges();
            }
            string[] ratingsArray2 = { "Beginner", "Intermediate", "Proficient", "Expert" };
            foreach (var item in ratingsArray2)
            {
                Rating rating = new Rating();
                rating.RatingsName = item;
                rating.SkillTypeID = 2;
                db.Ratings.Add(rating);
                db.SaveChanges();
            }

            Employee employee = new Employee();
            employee.EmployeeName = "Leander Dcosta";
            employee.DesignationID = 1;
            db.Employees.Add(employee);
            db.SaveChanges();
        }
    }


}
