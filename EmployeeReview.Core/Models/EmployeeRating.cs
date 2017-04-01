using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeReview.Core.Models
{
    public class EmployeeRating
    {
        [Key]
        public int EmployeeRatingsID { get; set; }
        public int EmployeeID { get; set; }
        public Nullable<int> SkillsID { get; set; }
        public Nullable<int> RatingsID { get; set; }
        public string Comments { get; set; }
        public System.DateTime CreateDate { get; set; }

        public Employee Employee { get; set; }
        public Rating Rating { get; set; }
        public Skill Skill { get; set; }
    }
}
