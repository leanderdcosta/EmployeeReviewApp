using EmployeeReview.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeReviewApp.Models
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }

        [Required]
        public string EmployeeName { get; set; }
        public int selectedDesignation { get; set; }
        public List<Designation> Designation { get; set; }
    }
}