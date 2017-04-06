using System.Collections.Generic;

namespace EmployeeReview.Core.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string SkillsName { get; set; }
        public int SkillTypeID { get; set; }

        public SkillType SkillType { get; set; }
        public ICollection<EmployeeRating> EmployeeRatings { get; set; }
    }
}
