using AutoMapper;
using BusinessObject.Context;
using BusinessObject.Data;
using BusinessObject.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace WebQuanLyhs.Controllers
{

    public class AdminController : Controller
    {
        private readonly ConnectDB db;
        private readonly IMapper _mapper;

        public AdminController(ConnectDB context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var usersWithRoles = db.Users.Include(u => u.Role).ToList();
            var admin = db.Users;
            
            return View(admin);
        }
        public ActionResult AddAccount()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var phanLoaiSVList = db.Roles.ToList();
            ViewBag.PhanLoaiSVList = new SelectList(phanLoaiSVList, "Role_id", "Role_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddAccount(UserLogin model)
        {
            if (ModelState.IsValid)
            {
               
                 var admin = _mapper.Map<User>(model);
                 db.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult EditAcc(int id)
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var item = db.Users.Find(id);
            var phanLoaiSVList = db.Roles.ToList();
            ViewBag.PhanLoaiSVList = new SelectList(phanLoaiSVList, "Role_id", "Role_name");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult EditAcc(User model)
        {
            var user = db.Users.FirstOrDefault(u => u.User_id == model.User_id);

            model.Detail = user.Detail;
            model.CCCD = user.CCCD;
            model.Sex_name = user.Sex_name;
            model.Avata = user.Avata;

                db.Update(user);

                db.SaveChanges();
                    return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteAcc(int id)
        {
            var item = db.Users.Find(id);
            if (item != null)
            {
                /*var DeleteItem=db.Categories.Attach(item);*/
                db.Users.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

		#region Category_Course
		public IActionResult CategoryIndex()
		{
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var category = db.Category_Courses.ToList();
			return View(category);
		}
		public ActionResult AddCategory()
		{
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult AddCategory(Category_Course model)
		{
			if (ModelState.IsValid)
			{
				db.Category_Courses.Add(model);
				db.SaveChanges();
				return RedirectToAction("CategoryIndex");
			}
			return View();

		}
		public IActionResult EditCategory(int id)
		{
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var item = db.Category_Courses.Find(id);
			return View(item);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult EditCategory(Category_Course model)
		{
			db.Category_Courses.Attach(model);
			db.Update(model);

			db.SaveChanges();
			return RedirectToAction("CategoryIndex");
		}
		[HttpPost]
		

		public ActionResult DeleteCategory(int id)
		{
			var item = db.Category_Courses.Find(id);
			if (item != null)
			{
				/*var DeleteItem=db.Categories.Attach(item);*/
				db.Category_Courses.Remove(item);
				db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
		#endregion
		#region Course

		public IActionResult CourseIndex()
		{
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var usersWithRoles = db.Courses.Include(u => u.Category_Course).ToList();
			var course = db.Courses.ToList();
			return View(course);
		}
		public ActionResult AddCourse()
		{
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var phanLoaiSVList = db.Category_Courses.ToList();
			ViewBag.KhoaHocSVList = new SelectList(phanLoaiSVList, "Category_coures_id", "Category_name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddCourse(Course model)
		{
			if (model != null)
			{
				db.Courses.Add(model);
				db.SaveChanges();
				return RedirectToAction("CourseIndex");
			}
			return View();

		}
		public IActionResult EditCourse(int id)
		{
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null || roleId != 1)
            {
                return Redirect("/User/Login");
            }
            var phanLoaiSVList = db.Category_Courses.ToList();
			ViewBag.KhoaHocSVList = new SelectList(phanLoaiSVList, "Category_coures_id", "Category_name");
			var item = db.Courses.Find(id);
			return View(item);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditCourse(Course model)
		{
			db.Courses.Attach(model);
			db.Update(model);

			db.SaveChanges();
			return RedirectToAction("CourseIndex");
		}
		[HttpPost]
		
		public ActionResult DeleteCourse(int id)
		{
			var item = db.Courses.Find(id);
			if (item != null)
			{
				/*var DeleteItem=db.Categories.Attach(item);*/
				db.Courses.Remove(item);
				db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		#endregion
	}
}
