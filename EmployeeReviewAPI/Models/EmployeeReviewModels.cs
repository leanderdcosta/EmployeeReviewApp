using EmployeeReview.Core.Models;
using System.Collections.Generic;

namespace EmployeeReviewApp.Models
{
    public class EmployeeReviewModels
    {
        public int EmployeeID { get; set; }
        public int typeID { get; set; }
        public string SkillTypeName { get; set; }
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<EmployeeRating> EmployeeRatings { get; set; }

    }
}