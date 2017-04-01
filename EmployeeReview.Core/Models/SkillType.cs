using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeReview.Core.Models
{
    public class SkillType
    {
        [Key]
        public int SkillTypeID { get; set; }
        public string SkillTypeName { get; set; }
    }
}
