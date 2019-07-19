using PrometheusWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrometheusWebApplication.Controllers
{

    public class AdminController : Controller
    {
        PrometheusContext prometheusContext = new PrometheusContext();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Login page.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Teacher teacher)
        {
            try
            {
                var details = prometheusContext.Teachers.Single(t => t.TeacherID == teacher.TeacherID && t.Password == teacher.Password && t.IsAdmin == "yes");
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
        /// Admin Dashboard.
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
        /// Functionalities add new teacher.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTeacher()
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
        [ValidateAntiForgeryToken]
        public ActionResult AddTeacher(Teacher teacherRegistration)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    prometheusContext.Teachers.Add(teacherRegistration);
                    prometheusContext.SaveChanges();
                    return RedirectToAction("TeacherAdded");
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
        /// Teacher added confirmation.
        /// </summary>
        public ActionResult Teacheradded()
        {
            if (Session["UserId"] != null)
            {
                var newTeacherID = (from t in prometheusContext.Teachers
                                    select t.TeacherID).Max();
                return View(prometheusContext.Teachers.Find(newTeacherID));
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionalities to add new student.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddStudent()
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
        public ActionResult AddStudent(Student studentRegistration)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    prometheusContext.Students.Add(studentRegistration);
                    prometheusContext.SaveChanges();
                    return RedirectToAction("Studentadded");
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
        /// Student added confirmation.
        /// </summary>
        public ActionResult Studentadded()
        {

            if (Session["UserId"] != null)
            {

                var newStudentID = (from t in prometheusContext.Students
                                    select t.StudentID).Max();
                return View(prometheusContext.Students.Find(newStudentID));
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionalities to add new course.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCourse()
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
        public ActionResult AddCourse(Course courseAdded)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    prometheusContext.Courses.Add(courseAdded);
                    prometheusContext.SaveChanges();
                    return RedirectToAction("CourseAdded");
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
        /// Course added confirmation.
        /// </summary>
        public ActionResult CourseAdded()
        {
            if (Session["UserId"] != null)
            {

                var newCourseID = (from t in prometheusContext.Courses
                                   select t.CourseID).Max();
                return View(prometheusContext.Courses.Find(newCourseID));
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionalities to search teacher by name.
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchTeacherByName()
        {
            if (Session["UserId"] != null)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SearchTeacherByName(string div)
        {
            if (Session["UserId"] != null)
            {
                //To maintain our data for the request
                TempData["Name"] = div;
                return RedirectToAction("DisplayTeachers");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Functionality to display teachers list.
        /// </summary>
        /// <returns></returns>
        public ActionResult DisplayTeachers()
        {
            if (Session["UserId"] != null)
            {

                List<Teacher> teachers = new List<Teacher>();
                foreach (var item in prometheusContext.Teachers)
                {
                    if (item.FName.ToLower().Contains(TempData["Name"].ToString().ToLower()) || item.LName.ToLower().Contains(TempData["Name"].ToString().ToLower()))
                        teachers.Add(item);

                }
                return View(teachers);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }

        }

        /// <summary>
        /// Functionalities to search student by name.
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchStudentByName()
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
        public ActionResult SearchStudentByName(string div)
        {
            if (Session["UserId"] != null)
            {
                //To maintain our data for the request
                TempData["Name"] = div;
                return RedirectToAction("DisplayStudents");
            }
            else
            {
                return RedirectToAction("index", "Home");
            }

        }

        /// <summary>
        /// Functionality to display students list.
        /// </summary>
        /// <returns></returns>
        public ActionResult DisplayStudents()
        {
            if (Session["UserId"] != null)
            {

                List<Student> students = new List<Student>();
                foreach (var item in prometheusContext.Students)
                {
                    if (item.FName.ToLower().Contains(TempData["Name"].ToString().ToLower()) || item.LName.ToLower().Contains(TempData["Name"].ToString().ToLower()))
                        students.Add(item);

                }
                return View(students);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionalities to search course by name.
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCourseByName()
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
        public ActionResult SearchCourseByName(string div)
        {
            if (Session["UserId"] != null)
            {
                //To maintain our data for the request
                TempData["Name"] = div;
                return RedirectToAction("DisplayCourses");
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to display courses list.
        /// </summary>
        /// <returns></returns>
        public ActionResult DisplayCourses()
        {
            if (Session["UserId"] != null)
            {

                List<Course> courses = new List<Course>();
                foreach (var item in prometheusContext.Courses)
                {
                    if (item.CourseName.ToLower().Contains(TempData["Name"].ToString().ToLower()))
                        courses.Add(item);

                }
                return View(courses);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to view courses list.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCourses()
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
        /// Teacher course mapping.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TeachersTeachingTheCourse(int id)
        {
            if (Session["UserId"] != null)
            {

                var teachers = from t in prometheusContext.Teaches
                               where t.CourseID == id
                               select t.TeacherID;

                var result = from t in prometheusContext.Teachers
                             where teachers.Contains(t.TeacherID)
                             select t;

                List<Teacher> NotAssignedTeacherList = (from t in prometheusContext.Teachers
                                                        where !(teachers.Contains(t.TeacherID))
                                                        select t).ToList();

                TempData["NotAssignedTeacherList"] = NotAssignedTeacherList;

                string courseName = (from c in prometheusContext.Courses
                                     where c.CourseID == id
                                     select c.CourseName).First().ToString();

                ViewData["courseID"] = id;
                TempData["courseID"] = id;
                ViewData["courseName"] = courseName;

                return View(result);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to assign new teacher.
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignNewTeacher()
        {
            if (Session["UserId"] != null)
            {

                return View(TempData["NotAssignedTeacherList"]);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        /// <summary>
        /// Functionality to assign teacher to a course.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AssignTeacherToCourse(int id)
        {
            if (Session["UserId"] != null)
            {

                Teach teach = new Teach();
                teach.CourseID = Convert.ToInt32(TempData["courseID"]);
                teach.TeacherID = id;
                prometheusContext.Teaches.Add(teach);
                prometheusContext.SaveChanges();
                return RedirectToAction("TeacherAssignedSuccessfully");
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        //Confirmation for Teacher Assigned.
        public ActionResult TeacherAssignedSuccessfully()
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