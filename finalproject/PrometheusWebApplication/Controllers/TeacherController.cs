using PrometheusWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrometheusWebApplication.Controllers
{
    public class TeacherController : Controller
    {
        PrometheusContext prometheusContext = new PrometheusContext();

        /// <summary>
        /// Teacher dashboard.
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Login page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Checks for the credentials and if correct it redirects to the home page of teacher or else it throws an exception.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Teacher teacher)
        {
            try
            {
                var details = prometheusContext.Teachers.Single(t => t.TeacherID == teacher.TeacherID && t.Password == teacher.Password);
                if (details != null)
                {
                    Session["UserId"] = details.TeacherID.ToString();
                    
                    return RedirectToAction("Home");
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }
        }

        /// <summary>
        /// Functionalities to update personal details.
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateDetails()
        {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("index", "Home");

                
            }
            else
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                return View(prometheusContext.Teachers.Find(temp));
            }
        }

        [HttpPost]
        public ActionResult UpdateDetails(Teacher teacher)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    int temp = Convert.ToInt32(Session["UserId"]);
                    Teacher teacherItem = prometheusContext.Teachers.Find(temp);
                    teacherItem.FName = teacher.FName;
                    teacherItem.LName = teacher.LName;
                    teacherItem.Address = teacher.Address;
                    teacherItem.DOB = teacher.DOB;
                    teacherItem.City = teacher.City;
                    teacherItem.Password = teacher.Password;
                    teacherItem.MobileNo = teacher.MobileNo;

                    prometheusContext.SaveChanges();
                    return RedirectToAction("DetailsUpdated");
                }
                else
                {
                    return RedirectToAction("index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Invalid inputs");
                return View();
            }
        }

        //Confirmation for Details Updated.
        public ActionResult DetailsUpdated()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to view course details.
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseDetails()
        {
            if (Session["UserId"] != null)
            {
                return View(prometheusContext.Courses);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to view student details.
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentDetails()
        {
            if (Session["UserId"] != null)
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                var id = from en in prometheusContext.Enrollments
                         join ts in prometheusContext.Teaches
                         on en.CourseID equals ts.CourseID
                         where ts.TeacherID == temp
                         select en.StudentID;

                var result = from std in prometheusContext.Students
                             where id.Contains(std.StudentID)
                             select std;

                return View(result);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to view homework details.
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeworkDetails()
        {
            if (Session["UserId"] != null)
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                var result = from homework in prometheusContext.Homework
                             join assignment in prometheusContext.Assignments
                             on homework.HomeWorkID equals assignment.HomeWorkID
                             where assignment.TeacherID == temp
                             select homework;
                return View(result);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionalities to create and add homework.
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateHomeWork()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
        
        [HttpPost]
        public ActionResult CreateHomeWork(Homework homework)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    prometheusContext.Homework.Add(homework);
                    prometheusContext.SaveChanges();
                    TempData["HomeworkID"] = homework.HomeWorkID;
                    return RedirectToAction("HomeworkCourseMap");
                }
                else
                {
                    return RedirectToAction("index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Invalid inputs");
                return View();
            }
        }

        /// <summary>
        /// A teacher can add homework only to the courses he/she teaches.
        /// </summary>       
        public ActionResult HomeworkCourseMap()
        {
            if (Session["UserId"] != null)
            {
                TempData.Keep();
                int temp = Convert.ToInt32(Session["UserId"]);
                var result = from course in prometheusContext.Courses
                             join ts in prometheusContext.Teaches
                             on course.CourseID equals ts.CourseID
                             where ts.TeacherID == temp
                             select course;
                return View(result);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        public ActionResult CourseHomeworkMap(int id)
        {
            if (Session["UserId"] != null)
            {
                Assignment assignment = new Assignment();
                assignment.CourseID = id;
                assignment.TeacherID = Convert.ToInt32(Session["UserId"]);
                assignment.HomeWorkID = Convert.ToInt32(TempData["HomeworkID"]);
                prometheusContext.Assignments.Add(assignment);
                prometheusContext.SaveChanges();
                return RedirectToAction("HomeWorkAdded");
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        //Confirmation for HomeWork Added.
        public ActionResult HomeWorkAdded()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionalities to edit homework.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditHomeWork(int id)
        {
            if (Session["UserId"] != null)
            {
                Homework homework = prometheusContext.Homework.Find(id);
                return View(homework);

            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditHomeWork(Homework homework)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    Homework homeworkItem = prometheusContext.Homework.Find(homework.HomeWorkID);
                    homeworkItem.Description = homework.Description;
                    homeworkItem.Deadline = homework.Deadline;
                    homeworkItem.ReqTime = homework.ReqTime;
                    homeworkItem.LongDescription = homework.LongDescription;

                    prometheusContext.SaveChanges();
                    return RedirectToAction("HomeworkEdited");
                }
                else
                {
                    return RedirectToAction("index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Invalid inputs");
                return View();
            }
        }

        //Confirmation for Homework Edited.
        public ActionResult HomeworkEdited()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
    }
}