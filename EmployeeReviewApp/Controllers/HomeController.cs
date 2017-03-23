using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeReviewApp.Models;
using System.Web.Routing;
using static EmployeeReviewApp.Models.EmployeeViewModel;

namespace EmployeeReviewApp.Controllers
{
    public class HomeController : Controller
    {
        EmployeeContext db = new EmployeeContext();

        int pageCounter = 0;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Designations = new SelectList(db.Designations.ToList(), "DesignationID", "DesignationName");
            return View();
        }

        [HttpPost]
        public ActionResult Index(EmployeeModel model)
        {
            int employeeID = 0;
            if (!string.IsNullOrEmpty(model.EmployeeName))
            {
                if (!db.Employees.Any(x => x.EmployeeName == model.EmployeeName))
                {
                    ViewBag.Designations = new SelectList(db.Designations.ToList(), "DesignationID", "DesignationName");
                    ModelState.AddModelError("EmployeeName", "Employee Name Invalid");
                }
                else
                {
                    employeeID = Convert.ToInt32(db.Employees.FirstOrDefault(y => y.EmployeeName == model.EmployeeName).EmployeeID);
                    System.Web.HttpContext.Current.Session["EmployeeID"] = employeeID;
                    return RedirectToAction("Home");
                }
            }
            return View();
        }

        public ActionResult Home()
        {
            System.Web.HttpContext.Current.Session["PageCounter"] = 1;
            return View();
        }

        [HttpGet]
        public ActionResult ReviewForm(int typeID)
        {
            EmployeeReviewModel employeeReview = new EmployeeReviewModel();
            employeeReview = GetEmployeeReviewData(typeID);
            employeeReview.AddFlag = false;

            if (employeeReview.EmployeeRatings.Count == 0)
            {
                employeeReview.AddFlag = true;
                return View(employeeReview);
            }

            employeeReview.SelectedRating = Convert.ToInt32(db.EmployeeRatings.FirstOrDefault(x => x.EmployeeID == employeeReview.EmployeeID && x.SkillsID == employeeReview.SkillID).RatingsID);
            employeeReview.EmployeeRatingsID = Convert.ToInt32(db.EmployeeRatings.FirstOrDefault(x => x.EmployeeID == employeeReview.EmployeeID && x.SkillsID == employeeReview.SkillID).EmployeeRatingsID);
            employeeReview.Comments = (db.EmployeeRatings.FirstOrDefault(x => x.EmployeeID == employeeReview.EmployeeID && x.SkillsID == employeeReview.SkillID).Comments).ToString();

            return View(employeeReview);
        }

        [HttpPost]
        public ActionResult ReviewForm(EmployeeReviewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The Comments field is required.");
                return View(model);
            }

            pageCounter = Convert.ToInt32(System.Web.HttpContext.Current.Session["PageCounter"]);
            var index = 1;
            var list = db.Skills.Where(x => x.SkillTypeID == model.typeID).ToList();
            var customSkills = list.Select(x => new
            {
                Row_number = index++,
                SkillsID = x.SkillsID,
                SkillsName = x.SkillsName,
                SkillTypeID = x.SkillTypeID
            }).OrderBy(x => x.SkillsID).ToList();

            if (model.AddFlag == true)
            {
                int skillID = 0;
                skillID = Convert.ToInt32(customSkills.FirstOrDefault(x => x.SkillTypeID == model.typeID && x.Row_number == pageCounter).SkillsID.ToString());

                if (model.EmployeeID != 0 && skillID != 0 && model.SelectedRating != 0 && !string.IsNullOrEmpty(model.Comments))
                {
                    using (var context = new EmployeeContext())
                    {
                        EmployeeRating employeeRating = new EmployeeRating();
                        employeeRating.EmployeeID = model.EmployeeID;
                        employeeRating.SkillsID = skillID;
                        employeeRating.RatingsID = model.SelectedRating;
                        employeeRating.Comments = model.Comments;
                        employeeRating.CreateDate = DateTime.Now;

                        context.EmployeeRatings.Add(employeeRating);
                        context.SaveChanges();
                        ++pageCounter;
                    }
                }
            }
            else
            {
                if (model.EmployeeID != 0 && model.EmployeeRatingsID != 0)
                {
                    EmployeeRating employeeRating = db.EmployeeRatings.First(x => x.EmployeeRatingsID == model.EmployeeRatingsID);
                    if (employeeRating.EmployeeID == model.EmployeeID && !employeeRating.Comments.Equals(model.Comments))
                    {
                        employeeRating.RatingsID = model.SelectedRating;
                        employeeRating.Comments = model.Comments;
                        db.SaveChanges();
                    }
                    ++pageCounter;
                }
            }

            if (pageCounter <= customSkills.Count)
                System.Web.HttpContext.Current.Session["PageCounter"] = pageCounter;
            else
                return RedirectToAction("Home");

            return RedirectToAction("ReviewForm", new RouteValueDictionary(new { typeID = model.typeID }));
        }

        private EmployeeReviewModel GetEmployeeReviewData(int typeID)
        {
            EmployeeReviewModel employeeReview = new EmployeeReviewModel();
            int employeeID = Convert.ToInt32(System.Web.HttpContext.Current.Session["EmployeeID"]);
            pageCounter = Convert.ToInt32(System.Web.HttpContext.Current.Session["PageCounter"]);
            var index = 1;

            var list = db.Skills.Where(x => x.SkillTypeID == typeID).ToList();
            var customSkills = list.Select(x => new
            {
                Row_number = index++,
                SkillsID = x.SkillsID,
                SkillsName = x.SkillsName,
                SkillTypeID = x.SkillTypeID
            }).OrderBy(x => x.SkillsID).ToList();

            employeeReview.EmployeeID = employeeID;
            employeeReview.SkillTypeName = (db.SkillTypes.Single(x => x.SkillTypeID == typeID).SkillTypeName).ToString();
            employeeReview.SkillName = customSkills.FirstOrDefault(x => x.SkillTypeID == typeID && x.Row_number == pageCounter).SkillsName.ToString();
            employeeReview.Ratings = db.Ratings.Where(x => x.SkillTypeID == typeID).ToList();

            employeeReview.SkillID = Convert.ToInt32(customSkills.FirstOrDefault(x => x.SkillTypeID == typeID && x.Row_number == pageCounter).SkillsID);
            employeeReview.EmployeeRatings = db.EmployeeRatings.Where(x => x.EmployeeID == employeeID && x.SkillsID == employeeReview.SkillID).ToList();
            employeeReview.typeID = typeID;

            return employeeReview;
        }

        public ActionResult ReviewSummary()
        {
            int employeeID = Convert.ToInt32(System.Web.HttpContext.Current.Session["EmployeeID"]);

            var list = from er in db.EmployeeRatings
                       where er.EmployeeID == employeeID
                       join e in db.Employees on er.EmployeeID equals e.EmployeeID
                       join r in db.Ratings on er.RatingsID equals r.RatingsID
                       join s in db.Skills on er.SkillsID equals s.SkillsID
                       join st in db.SkillTypes on s.SkillTypeID equals st.SkillTypeID
                       select new { st.SkillTypeID, st.SkillTypeName, s.SkillsID, s.SkillsName, r.RatingsName, er.Comments };

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

            return View(employeeViewModel);
        }

        public JsonResult GetReviewFormCompleteStatus(int employeeID)
        {
            var query = from c in db.Skills
                        where !(
                                from o in db.EmployeeRatings
                                where o.EmployeeID == employeeID
                                select o.SkillsID)
                                .Contains(c.SkillsID)
                        select c;

            int success = query.Count() == 0 ? 1 : -1;
            return Json(new { success }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDesignation(string employeeName)
        {
            int DesignationID = 0;
            if (!string.IsNullOrEmpty(employeeName))
            {
                if (db.Employees.Any(x => x.EmployeeName == employeeName))
                    DesignationID = Convert.ToInt32(db.Employees.FirstOrDefault(y => y.EmployeeName == employeeName).DesignationID);
                else
                    DesignationID = -1;
            }
            return Json(new { DesignationID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            ViewBag.Designations = new SelectList(db.Designations.ToList(), "DesignationID", "DesignationName");
            return View("Index");
        }
    }
}