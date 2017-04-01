using EmployeeReview.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeReviewAPI.Models
{
    public class EmployeeViewModel
    {
        public List<SkillType> skillType { get; set; }

        public List<SkillRating> skillRating { get; set; }
        public class SkillRating
        {
            public int SkillTypeID { get; set; }
            public string Skill { get; set; }
            public int SkillID { get; set; }
            public string Rating { get; set; }
            public string Comments { get; set; }
        }
    }
}