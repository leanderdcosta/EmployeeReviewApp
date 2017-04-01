using EmployeeReview.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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