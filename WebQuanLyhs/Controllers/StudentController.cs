using AutoMapper;
using Azure.Core;
using BusinessObject.Context;
using BusinessObject.Data;
using BusinessObject.Viewmodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebQuanLyhs.Helps;
using WebQuanLyhs.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebQuanLyhs.Controllers
{

    public class StudentController : Controller
    {
        private readonly ConnectDB db;


        public StudentController(ConnectDB context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 4)
            {
                return Redirect("/User/Login");
            }
            try
            {
                int? id = HttpContext.Session.GetInt32("ID");
                if (id == 0)
                {
                    throw new Exception("");
                }
                var courses = await db.Student_Classes
                .Where(sc => sc.Student_Course_id == id)
                .Include(sc => sc.Class_Role)
                .Include(sc => sc.Student_Course)
                .ToListAsync();


                return View(courses);
            }
            catch (Exception ex)
            {

                return BadRequest($"Error occurred: {ex.Message}");
            }

        }

        public ActionResult Detailcourse(int id)
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 4)
            {
                return Redirect("/User/Login");
            }
            DetailCourse user = null;
            try
            {
                user = (from v in db.Student_Courses
                        join vs in db.Student_Classes on v.Student_id equals vs.Student_Course_id
                        join vss in db.Class_Roles on vs.Class_Role_id equals vss.Class_Role_id
                        join vsss in db.Teacher_Classes on vss.Class_Role_id equals vsss.Class_Role_id
                        join vssss in db.Teacher_Courses on vsss.Teacher_Course_id equals vssss.Teacher_Coures_id
                        join vsssss in db.Courses on vssss.Course_id equals vsssss.Coures_id
                        where v.Student_id == id
                        select new DetailCourse
                        {
                            Student_Class_id = vs.Student_Class_id,
                            Teacher_Class = new Teacher_Class
                            {
                                Teacher_Course = new Teacher_Course
                                {
                                    Course = new Course
                                    {
                                        Coures_id = vsssss.Coures_id,
                                        Coures_name = vsssss.Coures_name,
                                        Exercises = (from b in db.Exercises
                                                     where b.Course_id == vsssss.Coures_id
                                                     select new Exercise
                                                     {
                                                         Exercise_id = b.Exercise_id,
                                                         File_name = b.File_name,
                                                         Link_submit_assignments = b.Link_submit_assignments,

                                                     }).ToList(),

                                    }
                                }
                            }
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return View(user);
        }
        [HttpGet]
        public IActionResult AddBt()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 4)
            {
                return Redirect("/User/Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddBt(AddBt model, int? id)
        {
            var exsercise = db.Exercises.FirstOrDefault(e => e.Exercise_id == id);
            if (exsercise != null)
            {
                exsercise.Creat_time = DateTime.Now;
                exsercise.Link_submit_assignments = Myunti.UploadHinh(model.Link_submit_assignments, "Filenopbt");
                db.Entry(exsercise).State = EntityState.Modified;
                db.SaveChanges();
            }
            int userid = (int)HttpContext.Session.GetInt32("ID");

            return RedirectToAction("Detailcourse", new { id = userid });



        }
       





    }
}
