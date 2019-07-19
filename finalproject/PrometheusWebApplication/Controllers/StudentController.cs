using PrometheusWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrometheusWebApplication.Controllers
{/*
    The main Student class
    Contains all methods for performing student functions
*/
 /// <summary>
 /// Author : Mahajan Ashwin Vishnu
 /// DOC : 01/07/2019
 /// Controller for Student Functionalities 
 /// </summary
    public class StudentController : Controller
    {
        PrometheusContext prometheusContext = new PrometheusContext();

        /// <summary>
        /// Student Dashboard.
        /// </summary>
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
        /// Checks for the credentials and if correct it redirects to the home page of student or else it throws an exception.
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Student student)
        {
            try
            {
                var details = prometheusContext.Students.Single(s => s.StudentID == student.StudentID && s.Password == student.Password);
                if (details != null)
                {
                    Session["UserId"] = details.StudentID.ToString();
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
        /// Functionalities to update student details.
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateDetails()
        {
            if (Session["UserId"] != null)
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                return View(prometheusContext.Students.Find(temp));
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
     
        [HttpPost]
        public ActionResult UpdateDetails(Student student)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    int temp = Convert.ToInt32(Session["UserId"]);
                    Student studentItem = prometheusContext.Students.Find(temp);
                    studentItem.FName = student.FName;
                    studentItem.LName = student.LName;
                    studentItem.Address = student.Address;
                    studentItem.DOB = student.DOB;
                    studentItem.City = student.City;
                    studentItem.Password = student.Password;
                    studentItem.MobileNo = student.MobileNo;

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
        /// Functionalities to enroll for a course.
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseEnroll()
        {
            if (Session["UserId"] != null)
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                var result = from course in prometheusContext.Courses
                             join enrollment in prometheusContext.Enrollments
                             on course.CourseID equals enrollment.CourseID
                             where enrollment.StudentID == temp
                             select course;
                var result1 = prometheusContext.Courses
                              .Select(e => e)
                             .Except(result);
                return View(result1);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        public ActionResult AddEnroll(int id)
        {
            if (Session["UserId"] != null)
            {

                Enrollment enrollment = new Enrollment();
                enrollment.StudentID = Convert.ToInt32(Session["UserId"].ToString());
                enrollment.CourseID = id;
                prometheusContext.Enrollments.Add(enrollment);

                prometheusContext.SaveChanges();

                return RedirectToAction("CourseEnrolled");
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        //Confirmation for Course Enrolled.
        public ActionResult CourseEnrolled()
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
        /// Functionality to view homework.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewHomework()
        {
            if (Session["UserId"] != null)
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                var id = from enrollment in prometheusContext.Enrollments
                         join assignment in prometheusContext.Assignments
                         on enrollment.CourseID equals assignment.CourseID
                         where enrollment.StudentID == temp
                         select assignment.HomeWorkID;

                var result = from homework in prometheusContext.Homework
                             where id.Contains(homework.HomeWorkID)
                             select homework;
                return View(result);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to view Courses.
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourses()
        {
            if (Session["UserId"] != null)
            {
                int temp = Convert.ToInt32(Session["UserId"]);
                var result = from course in prometheusContext.Courses
                             join enrollment in prometheusContext.Enrollments
                             on course.CourseID equals enrollment.CourseID
                             where enrollment.StudentID == temp
                             select course;
                return View(result);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
    }
}