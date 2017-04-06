using EmployeeReview.Core.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace EmployeeReviewAPI.Controllers
{
    public class HomeController : ApiController
    {
        EmployeeReviewContext db = new EmployeeReviewContext();

        public HttpResponseMessage GetEmployeeDesignation(string employeeName)
        {
            int DesignationID = 0;
            if (!string.IsNullOrEmpty(employeeName))
            {
                if (db.Employees.Any(x => x.EmployeeName == employeeName))
                    DesignationID = Convert.ToInt32(db.Employees.FirstOrDefault(y => y.EmployeeName == employeeName).DesignationID);
                else
                    DesignationID = -1;
            }

            JToken json = JObject.Parse("{ 'DesignationID' : " + DesignationID + " }");
            return new HttpResponseMessage { Content = new JsonContent(json) };
        }

    }


}
