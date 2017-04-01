using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeReview.Core.Models
{
    public class Skill
    {
        [Key]
        public int SkillsID { get; set; }
        public string SkillsName { get; set; }
        public Nullable<int> SkillTypeID { get; set; }

        public  ICollection<EmployeeRating> EmployeeRatings { get; set; }
    }
}
