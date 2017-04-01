using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeReview.Core.Models
{
    public class Rating
    {


        [Key]
        public int RatingsID { get; set; }
        public string RatingsName { get; set; }
        public Nullable<int> SkillTypeID { get; set; }

        public ICollection<EmployeeRating> EmployeeRatings { get; set; }
    }
}
