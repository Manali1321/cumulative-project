using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cumulative_project.Models;


namespace cumulative_project.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        //GET:/Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);


        }
        //GET:/Teacher/Show{int id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        //GET:/Teacher/DeleteConfirm{int id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }
        //POST: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }
        //GET: /Teacher/New
        public ActionResult New()
        {
            return View();
        }
        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, DateTime HireDate, decimal Salary, string EmployeeNumber)
        {
            Debug.WriteLine("I have accessed the create method");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(Salary);
            Debug.WriteLine(EmployeeNumber);

            
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname=TeacherLname;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);
            return RedirectToAction("List");


        }
    
    
    
    }




}