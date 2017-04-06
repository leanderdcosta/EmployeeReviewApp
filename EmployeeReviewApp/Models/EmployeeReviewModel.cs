using EmployeeReview.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeReviewApp.Models
{
    public class EmployeeReviewModel
    {
        public int typeID { get; set; }
        public int EmployeeID { get; set; }
        public bool AddFlag { get; set; }

        [Required(ErrorMessage = "Please Select Ratings")]
        public int SelectedRating { get; set; }
        public List<Rating> Ratings { get; set; }

        public int SkillID { get; set; }
        public string SkillTypeName { get; set; }
        public string SkillName { get; set; }

        [Required]
        public string Comments { get; set; }

        public int EmployeeRatingsID { get; set; }
        public List<EmployeeRating> EmployeeRatings { get; set; }

    }
}