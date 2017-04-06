using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeReview.Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public int DesignationID { get; set; }

        public Designation Designation { get; set; }
        public ICollection<EmployeeRating> EmployeeRatings { get; set; }
    }
}
