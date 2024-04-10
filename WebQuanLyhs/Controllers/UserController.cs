using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BusinessObject.Context;
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using BusinessObject.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebQuanLyhs.DTO;
using WebQuanLyhs.Helps;

namespace WebQuanLyhs.Controllers
{
	public class UserController : Controller
	{
        public static UserController ?instance;
        public static readonly object instanceLock = new object();
        public static UserController Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserController();
                    }
                    return instance;
                }
            }
        }
        private readonly ConnectDB db = new ConnectDB();
		public IActionResult Login(string? ReturnUrl)
		{
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
		}
	
		[HttpPost]
		public async Task<IActionResult>Login(UserLogin model, string? ReturnUrl)
		{
            ViewBag.ReturnUrl = ReturnUrl;
            var user = db.Users.SingleOrDefault(kh => kh.Email == model.Email && kh.Password == model.Password);
            
            if (user?.Email != model.Email || user?.Password != model.Password)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
            if(user == null)
            {
                return View();
            }   
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Name", user.Fullname);
                HttpContext.Session.SetInt32("ID", user.User_id);
                HttpContext.Session.SetInt32("Role", user.Role_id);
            if (user.Avata != null)
            {
                HttpContext.Session.SetString("Avata", user.Avata);
            }


            if (user.Role_id == 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.Role_id == 2)
            {
                return RedirectToAction("TeacherIndex", "StaffTrain");
            }
            else if (user.Role_id == 3)
            {
                return RedirectToAction("index", "Teacher");
            }
            else if (user.Role_id == 4)
            {
                return RedirectToAction("index", "Student");
            }
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Login");


        }
       
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear(); 
            return Redirect("/");
        }
        public ActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            if (userId == null)
            {
                return RedirectToAction("Login", "User"); 
            }

            var user = db.Users.FirstOrDefault(u => u.User_id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public ActionResult EditProfile(int id)
        {
            int? user = HttpContext.Session.GetInt32("ID");
            if (user == null)
            {
                return Redirect("/User/Login");
            }
            var item = db.Users.Find(id);
            return View(item);
        }
        [HttpPost]
        public IActionResult EditProfile(Profile model)
        {
          

            var user = db.Users.FirstOrDefault(u => u.User_id == model.User_id);

            if (user == null)
            {
                return NotFound();
            }

            user.User_id = model.User_id;
            user.Password = model.Password;
            user.Phone = model.Phone;
            user.Fullname = model.Fullname;
            user.Detail = model.Detail;
            user.Sex_name = model.Sex_name;
            user.CCCD = model.CCCD;
          

            db.Entry(user).State = EntityState.Modified;

            db.SaveChanges();

            
            return RedirectToAction("Profile");
        }
      /*  public IActionResult Uploadimg()
        {
            int? roleId = HttpContext.Session.GetInt32("Role");
            if (roleId == null )
            {
                return Redirect("/User/Login");
            }
            return View();
        }*/
        [HttpPost]
        public IActionResult Uploadimg(Upload model ,int User_id)
        {

            var user = db.Users.FirstOrDefault(u => u.User_id == User_id);

            if (user != null)
            {
                user.Avata = Myunti.UploadHinh(model.Avata, "Filenopbt");
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

            }
            int userid = (int)HttpContext.Session.GetInt32("ID");

            return RedirectToAction("Profile", new { id = userid });
        }


    }
}

