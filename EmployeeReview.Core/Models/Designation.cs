using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeReview.Core.Models
{
    public class Designation
    {
        [Key]
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
