using EmployeeReview.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using EmployeeReviewAPI.Models;
using static EmployeeReviewAPI.Models.EmployeeViewModel;
using Newtonsoft.Json;
using EmployeeReviewApp.Models;

namespace EmployeeReviewAPI.Controllers
{
    public class EmployeeReviewController : ApiController
    {
        EmployeeReviewContext db = new EmployeeReviewContext();

        [HttpGet]
        public HttpResponseMessage GetReviewFormCompleteStatus(int employeeID)
        {
            var query = from c in db.Skills
                        where !(
                                from o in db.EmployeeRatings
                                where o.EmployeeID == employeeID
                                select o.SkillID)
                                .Contains(c.Id)
                        select c;

            int success = query.Count() == 0 ? 1 : -1;

            JToken json = JObject.Parse("{ 'success' : " + success + " }");
            return new HttpResponseMessage { Content = new JsonContent(json) };
        }

        [HttpGet]
        public HttpResponseMessage GetEmployeeReviewSummary(int employeeID)
        {
            var list = from er in db.EmployeeRatings
                       where er.EmployeeID == employeeID
                       join e in db.Employees on er.EmployeeID equals e.Id
                       join r in db.Ratings on er.RatingID equals r.Id
                       join s in db.Skills on er.SkillID equals s.Id
                       join st in db.SkillTypes on s.SkillTypeID equals st.Id
                       select new
                       {
                           SkillTypeID = st.Id,
                           SkillTypeName = st.SkillTypeName,
                           SkillsID = s.Id,
                           SkillsName = s.SkillsName,
                           RatingsName = r.RatingsName,
                           Comments = er.Comments
                       };

            EmployeeViewModel employeeViewModel = new EmployeeViewModel();

            employeeViewModel.skillType = db.SkillTypes.ToList();
            foreach (var item in db.SkillTypes.ToList())
            {
                List<SkillRating> skillRating = new List<SkillRating>();
                foreach (var item1 in list)
                {
                    SkillRating val = new SkillRating();
                    val.SkillID = item1.SkillsID;
                    val.Skill = item1.SkillsName;
                    val.SkillTypeID = item1.SkillTypeID;
                    val.Rating = item1.RatingsName;
                    val.Comments = item1.Comments;
                    skillRating.Add(val);
                }
                employeeViewModel.skillRating = skillRating;
            }

            string jsonEmployeeViewModel = JsonConvert.SerializeObject(employeeViewModel);

            JToken json = JObject.Parse(jsonEmployeeViewModel);
            return new HttpResponseMessage { Content = new JsonContent(json) };
        }

        [HttpPost]
        public HttpResponseMessage AddEmployeeRating(EmployeeRating employeeRating)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeRatings.Add(employeeRating);
                db.SaveChanges();
            }

            var response = Request.CreateResponse<EmployeeRating>(HttpStatusCode.Created, employeeRating);
            string uri = Url.Link("DefaultApi", new { id = employeeRating.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [HttpPut]
        public HttpResponseMessage UpdateEmployeeRating(EmployeeRating model)
        {
            EmployeeRating employeeRating = db.EmployeeRatings.First(x => x.Id == model.Id);
            if (employeeRating.EmployeeID == model.EmployeeID && !employeeRating.Comments.Equals(model.Comments))
            {
                employeeRating.RatingID = model.RatingID;
                employeeRating.Comments = model.Comments;
                db.SaveChanges();
            }

            var response = new HttpResponseMessage();
            response.Headers.Add("Message", "Succsessfuly Updated!!!");
            return response;
        }

        private bool EmployeeRatingExists(int id)
        {
            return db.EmployeeRatings.Count(e => e.Id == id) > 0;
        }

        [HttpGet]
        public HttpResponseMessage GetEmployeeReviewFormData(int typeID, int pageCounter, int employeeID)
        {
            var index = 1;
            var list = db.Skills.Where(x => x.SkillTypeID == typeID).ToList();
            var customSkills = list.Select(x => new
            {
                Row_number = index++,
                SkillsID = x.Id,
                SkillsName = x.SkillsName,
                SkillTypeID = x.SkillTypeID
            }).OrderBy(x => x.SkillsID).ToList();

            EmployeeReviewModels employeeReview = new EmployeeReviewModels();
            employeeReview.EmployeeID = employeeID;
            employeeReview.typeID = typeID;
            employeeReview.SkillTypeName = (db.SkillTypes.Single(x => x.Id == typeID).SkillTypeName).ToString();
            employeeReview.SkillID = Convert.ToInt32(customSkills.FirstOrDefault(x => x.SkillTypeID == typeID && x.Row_number == pageCounter).SkillsID);
            employeeReview.SkillName = customSkills.FirstOrDefault(x => x.SkillTypeID == typeID && x.Row_number == pageCounter).SkillsName.ToString();
            employeeReview.Ratings = db.Ratings.Where(x => x.SkillTypeID == typeID).ToList();
            employeeReview.EmployeeRatings = db.EmployeeRatings.Where(x => x.EmployeeID == employeeID && x.SkillID == employeeReview.SkillID).ToList();


            string jsonEmployeeViewModel = JsonConvert.SerializeObject(employeeReview, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });

            JToken json = JObject.Parse(jsonEmployeeViewModel);
            return new HttpResponseMessage { Content = new JsonContent(json) };
        }

        [HttpGet]
        public HttpResponseMessage GetEmployeeDesignationList()
        {
            DesignationModel designationModel = new DesignationModel();
            designationModel.Designations = db.Designations.ToList();

            string jsonModel = JsonConvert.SerializeObject(designationModel);

            JToken json = JObject.Parse(jsonModel);
            return new HttpResponseMessage { Content = new JsonContent(json) };
        }


        [HttpGet]
        public HttpResponseMessage IsValidEmployee(string EmployeeName)
        {
            int employeeID = 0;
            bool value = db.Employees.Any(x => x.EmployeeName == EmployeeName);
            if (value)
                employeeID = Convert.ToInt32(db.Employees.FirstOrDefault(y => y.EmployeeName == EmployeeName).Id);

            JToken json = JObject.Parse("{ 'EmployeeID' : " + employeeID + " }");
            return new HttpResponseMessage { Content = new JsonContent(json) };
        }
    }
}
