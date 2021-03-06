﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using EmployeeReview.Core.Models;
using EmployeeReviewApp.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeeReviewApp.Controllers
{
    public class EmployeeReviewController : Controller
    {
        int pageCounter = 0;

        [HttpGet]
        public ActionResult Index()
        {
            //To Insert Master Data 
            //Initialize initialize = new Initialize();
            //initialize.Start();

            FillDesignationDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult Index(EmployeeModel model)
        {
            int employeeID = 0;
            if (!string.IsNullOrEmpty(model.EmployeeName))
            {
                HttpClient client = InitialiseHttpClient();
                HttpResponseMessage response = client.GetAsync("EmployeeReview/IsValidEmployee?EmployeeName=" + model.EmployeeName).Result;
                if (response.IsSuccessStatusCode)
                {
                    var JSONResponse = response.Content.ReadAsStringAsync();
                    string jsonString = JSONResponse.Result.ToString();
                    dynamic data = JObject.Parse(jsonString);
                    employeeID = data.EmployeeID;

                    if (employeeID == 0)
                    {
                        FillDesignationDropDown();
                        ModelState.AddModelError("EmployeeName", "Employee Name Invalid");
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["EmployeeID"] = employeeID;
                        return RedirectToAction("Home");
                    }
                }
            }
            return View();
        }

        public ActionResult Home()
        {
            EmployeeReviewContext db = new EmployeeReviewContext();

            System.Web.HttpContext.Current.Session["PageCounter"] = 1;

            SkillsTypeModels model = new SkillsTypeModels();
            model.SkillType = db.SkillTypes.ToList();

            return View(model);
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

            employeeReview.SelectedRating = Convert.ToInt32(employeeReview.EmployeeRatings.FirstOrDefault().RatingID);
            employeeReview.EmployeeRatingsID = Convert.ToInt32(employeeReview.EmployeeRatings.FirstOrDefault().Id);
            employeeReview.Comments = (employeeReview.EmployeeRatings.FirstOrDefault().Comments).ToString();

            return View(employeeReview);
        }

        [HttpPost]
        public ActionResult ReviewForm(EmployeeReviewModel model)
        {
            EmployeeReviewContext db = new EmployeeReviewContext();

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
                SkillsID = x.Id,
                SkillsName = x.SkillsName,
                SkillTypeID = x.SkillTypeID
            }).OrderBy(x => x.SkillsID).ToList();

            if (model.AddFlag == true)
            {
                int skillID = 0;
                skillID = Convert.ToInt32(customSkills.FirstOrDefault(x => x.SkillTypeID == model.typeID && x.Row_number == pageCounter).SkillsID.ToString());

                if (model.EmployeeID != 0 && skillID != 0 && model.SelectedRating != 0 && !string.IsNullOrEmpty(model.Comments))
                {
                    EmployeeRating employeeRating = new EmployeeRating();
                    employeeRating.EmployeeID = model.EmployeeID;
                    employeeRating.SkillID = skillID;
                    employeeRating.RatingID = model.SelectedRating;
                    employeeRating.Comments = model.Comments;
                    employeeRating.CreateDate = DateTime.Now;

                    HttpClient client = InitialiseHttpClient();
                    HttpResponseMessage response = client.PostAsJsonAsync("EmployeeReview/AddEmployeeRating", employeeRating).Result;
                    if (response.IsSuccessStatusCode)
                        ++pageCounter;
                }
            }
            else
            {
                if (model.EmployeeID != 0 && model.EmployeeRatingsID != 0)
                {
                    EmployeeRating employeeRating = db.EmployeeRatings.First(x => x.Id == model.EmployeeRatingsID);
                    if (employeeRating.EmployeeID == model.EmployeeID && !employeeRating.Comments.Equals(model.Comments))
                    {
                        employeeRating.RatingID = model.SelectedRating;
                        employeeRating.Comments = model.Comments;
                        db.SaveChanges();
                    }
                    ++pageCounter;
                    //EmployeeRating employeeRating = new EmployeeRating();
                    //employeeRating.RatingsID = model.SelectedRating;
                    //employeeRating.Comments = model.Comments;

                    //HttpClient client = InitialiseHttpClient();
                    //HttpResponseMessage response = client.PutAsJsonAsync("EmployeeReview/UpdateEmployeeRating", employeeRating).Result;
                    //if (response.IsSuccessStatusCode)
                    //    ++pageCounter;

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
            int employeeID = Convert.ToInt32(System.Web.HttpContext.Current.Session["EmployeeID"]);
            pageCounter = Convert.ToInt32(System.Web.HttpContext.Current.Session["PageCounter"]);

            HttpClient client = InitialiseHttpClient();

            EmployeeReviewModel employeeReviewModel = null;
            HttpResponseMessage response = client.GetAsync("EmployeeReview/GetEmployeeReviewFormData?typeID=" + typeID + "&pageCounter=" + pageCounter + " &employeeID=" + employeeID).Result;
            if (response.IsSuccessStatusCode)
            {
                var JSONResponse = response.Content.ReadAsStringAsync();
                employeeReviewModel = JsonConvert.DeserializeObject<EmployeeReviewModel>(JSONResponse.Result.ToString());
            }
            return employeeReviewModel;
        }

        public ActionResult ReviewSummary()
        {
            int employeeID = Convert.ToInt32(System.Web.HttpContext.Current.Session["EmployeeID"]);
            HttpClient client = InitialiseHttpClient();

            EmployeeViewModel employeeViewModel = null;
            HttpResponseMessage response = client.GetAsync("EmployeeReview/GetEmployeeReviewSummary?employeeID=" + employeeID).Result;
            if (response.IsSuccessStatusCode)
            {
                var JSONResponse = response.Content.ReadAsStringAsync();
                employeeViewModel = JsonConvert.DeserializeObject<EmployeeViewModel>(JSONResponse.Result.ToString());
            }

            return View(employeeViewModel);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            FillDesignationDropDown();

            return View("Index");
        }

        public HttpClient InitialiseHttpClient()
        {

            string webAPIURI = System.Configuration.ConfigurationManager.AppSettings["WebAPI"];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(webAPIURI);
            return client;
        }

        private void FillDesignationDropDown()
        {
            HttpClient client = InitialiseHttpClient();
            HttpResponseMessage response = client.GetAsync("EmployeeReview/GetEmployeeDesignationList").Result;
            if (response.IsSuccessStatusCode)
            {
                var JSONResponse = response.Content.ReadAsStringAsync();
                DesignationModel designationList = JsonConvert.DeserializeObject<DesignationModel>(JSONResponse.Result.ToString());
                ViewBag.Designations = new SelectList(designationList.Designations, "Id", "DesignationName");
            }
        }
    }
}