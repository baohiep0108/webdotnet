using BusinessObject.Context;
using BusinessObject.Data;
using BusinessObject.Viewmodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using WebQuanLyhs.DTO;
using WebQuanLyhs.Helps;

namespace WebQuanLyhs.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ConnectDB db;

        public TeacherController(ConnectDB context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 3)
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
                var courses = await db.Teacher_Classes
                .Where(sc => sc.Teacher_Course_id == id)
                .Include(sc => sc.Class_Role)
                .Include(sc => sc.Teacher_Course)
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
            if (roleId == null || roleId != 3)
            {
                return Redirect("/User/Login");
            }
            DetailTeacher user = null;
            try
            {
                user = (from v in db.Teacher_Courses
                        join vs in db.Courses on v.Course_id equals vs.Coures_id
                        where v.Teacher_Coures_id == id
                        select new DetailTeacher
                        {
                            Teacher_id = v.Teacher_Coures_id,
                            Teacher_Course = new Teacher_Course
                            {
                                Course = new Course
                                {
                                    Coures_id = vs.Coures_id,
                                    Coures_name = vs.Coures_name,
                                    Exercises = (from b in db.Exercises
                                                 where b.Course_id == vs.Coures_id
                                                 select new Exercise
                                                 {
                                                     Exercise_id = b.Exercise_id,
                                                     File_name = b.File_name,
                                                 }).ToList(),
                                }
                            }
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return View(user);
        }
        public ActionResult Exercise()
        {
            var item = db.Exercises.Include(sc => sc.Course).ToList();
            return View(item);
        }
        public ActionResult ExerciseAdd()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 3)
            {
                return Redirect("/User/Login");
            }
            int? id = HttpContext.Session.GetInt32("ID");
            if (id == 0)
            {
                throw new Exception("");
            }
            // Lọc danh sách khóa học theo giáo viên.
            var courses = db.Teacher_Courses
               .Where(sc => sc.Teacher_Coures_id == id)
               .Select(sc => sc.Course)
               .ToList();


            ViewBag.KhoaHocSVList = new SelectList(courses, "Coures_id", "Coures_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExerciseAdd(AddExercise model)
        {
            if (model.File != null)
            {
                var exsercise = new Exercise()
                {
                    Exercise_name = model.Exercise_name,
                    Creat_time = DateTime.Now,
                    File_name = Myunti.UploadHinh(model.File, "Exercise"),
                    Link_submit_assignments = model.Link_submit_assignments,
                    Course_id = model.Course_id
                };
                db.Exercises.Add(exsercise);
                db.SaveChanges();
                return RedirectToAction("Exercise");
            }
            else { return View(model); }
        }
        public ActionResult ExerciseEdit()
        {
            int? id = HttpContext.Session.GetInt32("ID");
            if (id == 0)
            {
                throw new Exception("");
            }
            // Lọc danh sách khóa học theo giáo viên.
            var courses = db.Teacher_Courses
               .Where(sc => sc.Teacher_Coures_id == id)
               .Select(sc => sc.Course)
               .ToList();


            ViewBag.KhoaHocSVList = new SelectList(courses, "Coures_id", "Coures_name");
            return View();
        }
        [HttpPost]
        public IActionResult ExerciseEdit(AddExercise model, int id )
        {
            var exsercise = db.Exercises.FirstOrDefault(e => e.Exercise_id == id);

            exsercise.Exercise_name = model.Exercise_name;
            exsercise.Creat_time = DateTime.Now;
          

            db.Entry(exsercise).State = EntityState.Modified;
            db.SaveChanges();

            int userid = (int)HttpContext.Session.GetInt32("ID");

            return RedirectToAction("Detailcourse", new { id = userid });


        }
       
    }
}
