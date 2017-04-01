using EmployeeReview.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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