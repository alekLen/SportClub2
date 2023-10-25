﻿using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SportClub.Filters;
using SportClub.Models;
using System.Security.Cryptography;
using System.Text;

namespace SportClub.Controllers
{

    [Culture]
    public class LoginController : Controller
    {
     /*   SportClubContext db;
        public LoginController(SportClubContext context)
        {
            db = context;
        }

        int age {  get; set; }
        public IActionResult Registration()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View("Register");
        }
        public async Task<IActionResult> RegistrationCoach()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putSpecialities();
            await putPosts();
            return View("RegisterCoach");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterClientModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);
           
            try
            {
               
                // DateTime dateTime = DateTime.Parse(user.DateOfBirth);
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    // 2. Вычисление возраста
                    DateTime currentDate = DateTime.Now;
                    age = currentDate.Year - birthDate.Year;

                    // Учитываем месяц и день рождения для точного определения возраста
                    if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                    {
                        age--;
                    }

                    Console.WriteLine("Ваш возраст: " + age + " лет");
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения");
                }      

            }
            catch { ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения"); }
            if (ModelState.IsValid)
            {
               /* if (await userService.GetUser(user.Login) != null)
                {
                    ModelState.AddModelError("login", "this login already exists");
                    return View(user);
                }
                if (await userService.GetEmail(user.email) != null)
                {
                    ModelState.AddModelError("email", "this email is already registred");
                    return View(user);
                }*/
              /*  User u = new();
                u.Login = user.Login;
                u.Gender = user.Gender;
                u.Email = user.Email;
                u.Age = age;
                u.Phone = user.Phone;
                u.Name=user.Name;
                u.Surname = user.Surname;
                u.Dopname=user.Dopname;
                u.DateOfBirth = user.DateOfBirth;
              
                byte[] saltbuf = new byte[16];
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();
                Salt s = new();
                s.salt = salt;
                string password = salt + user.Password;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                u.Password = hashedPassword;
                try
                {
                    db.Users.Add(u);
                   db.SaveChanges();
                    s.user = u;
                    db.Salts.Add(s);
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Login");
            }
            return View("Register",user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationCoach(RegisterCoachModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);

            try
            {

                // DateTime dateTime = DateTime.Parse(user.DateOfBirth);
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    // 2. Вычисление возраста
                    DateTime currentDate = DateTime.Now;
                    age = currentDate.Year - birthDate.Year;

                    // Учитываем месяц и день рождения для точного определения возраста
                    if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                    {
                        age--;
                    }

                    Console.WriteLine("Ваш возраст: " + age + " лет");
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения");
                }

            }
            catch { ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения"); }
            if (ModelState.IsValid)
            {
                /* if (await userService.GetUser(user.Login) != null)
                 {
                     ModelState.AddModelError("login", "this login already exists");
                     return View(user);
                 }
                 if (await userService.GetEmail(user.email) != null)
                 {
                     ModelState.AddModelError("email", "this email is already registred");
                     return View(user);
                 }*/
               /* User u = new();
                u.Login = user.Login;
                u.Gender = user.Gender;
                u.Email = user.Email;
                u.Age = age;
                u.Phone = user.Phone;
                u.Name = user.Name;
                u.Surname = user.Surname;
                u.Dopname = user.Dopname;
                u.DateOfBirth = user.DateOfBirth;

                byte[] saltbuf = new byte[16];
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();
                Salt s = new();
                s.salt = salt;
                string password = salt + user.Password;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                u.Password = hashedPassword;
                try
                {
                    db.Users.Add(u);
                    db.SaveChanges();
                    s.user = u;
                    db.Salts.Add(s);
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Login");
            }
            return View("RegisterCoach", user);
        }
        public async Task<IActionResult> Login()
        {
         
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);

            if (ModelState.IsValid)
            {
                User u = await db.Users.FirstOrDefaultAsync(m => m.Login == user.Login);
                if(u==null)
                {
                    Admin a= await db.Admins.FirstOrDefaultAsync(m => m.Login == user.Login);
                    if(a!=null)
                    {
                        if (await CheckPasswordA(a, user.Password))
                        {
                            HttpContext.Session.SetString("login", user.Login);
                            HttpContext.Session.SetString("admin", "admin");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "логин или пароль неверные");
                            return View(user);
                        }
                    }
                    else
                    {
                        Coach c = await db.Coaches.FirstOrDefaultAsync();
                        if (c != null)
                        {
                            if (await CheckPasswordC(c, user.Password))
                            {
                                HttpContext.Session.SetString("login", user.Login);
                                HttpContext.Session.SetString("coach", "coach");
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "логин или пароль неверные");
                                return View(user);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "логин или пароль неверные");
                            return View(user);
                        }
                    }
                }           
                else
                {
                        if (await CheckPasswordU(u, user.Password))
                        {
                            HttpContext.Session.SetString("login", user.Login);                            
                                HttpContext.Session.SetString("client", "client");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "логин или пароль неверные");
                            return View(user);
                        }
                 }              
            }
            return View(user);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            User u = await db.Users.FirstOrDefaultAsync(m => m.Email == email);
            if (u == null)
                return Json(true);
            else
                return Json(false);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsLoginInUse(string login)
        {
            User u = await db.Users.FirstOrDefaultAsync(m => m.Login == login);
            if (u == null)
                return Json(true);
            else
                return Json(false);
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // очищается сессия
            return RedirectToAction("Index", "Home");
        }
        public IActionResult CheckAge(int age)
        {
            try
            {
                if (Convert.ToInt32(age) < 0 || Convert.ToInt32(age) > 99)
                    return Json(false);
                else
                    return Json(true);
            }
            catch { return Json(false); }
        }
        public IActionResult CheckPassword(string password)
        {
            int length = password.Length;
            if (length < 9)
                return Json(false);
            int digitCount = password.Count(char.IsDigit);
            int uppercaseCount = password.Count(char.IsUpper);
            int lowercaseCount = password.Count(char.IsLower);
            int specialCharCount = password.Count(c => !char.IsLetterOrDigit(c));
            if (digitCount == 0 || uppercaseCount == 0 || lowercaseCount == 0 || specialCharCount == 0)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }
        public async Task<bool> CheckPasswordU(User u, string p)
        {
            var us = new User
            {
                Id = u.Id,
                Login = u.Login,
                Password = u.Password,
               
            };
            Salt s = await db.Salts.FirstOrDefaultAsync(m => m.user == us);
            string conf = s.salt + p;
            if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
                return true;
            else
                return false;
        }
        public async Task<bool> CheckPasswordA(Admin u, string p)
        {
            var us = new Admin
            {
                Id = u.Id,
                Login = u.Login,
                Password = u.Password,

            };
            Salt s = await db.Salts.FirstOrDefaultAsync(m => m.admin == us);
            string conf = s.salt + p;
            if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
                return true;
            else
                return false;
        }
        public async Task<bool> CheckPasswordC(Coach u, string p)
        {
            var us = new Coach
            {
                Id = u.Id,
                Login = u.Login,
                Password = u.Password,

            };
            Salt s = await db.Salts.FirstOrDefaultAsync(m => m.coach == us);
            string conf = s.salt + p;
            if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
                return true;
            else
                return false;
        }
        public async Task<IActionResult> AddPost()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putPosts();
            return View("Post");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(string name)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                Post p = new();
                p.Name = name;
                db.Posts.Add(p);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await putPosts();
                return View("Post");
            }
        }
        public async Task<IActionResult> EditPost(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            Post p = await db.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (p != null)
            {
                return View("EditPost", p);
            }
            await putPosts();
            return View("Post");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, string name)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                Post p = await db.Posts.FirstOrDefaultAsync(m => m.Id == id);
                if (p == null)
                {
                    await putPosts();
                    return View("Post");
                }
                p.Name = name;
                db.Posts.Update(p);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await putSpecialities();
                return View("Speciality");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            Post p = await db.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (p != null)
            {
                return View("DeletePost", p);
            }
            await putPosts();
            return View("Post");
        }
        public async Task<IActionResult> AddSpeciality()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putSpecialities();
            return View("Speciality");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSpeciality(string name)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                Speciality sp = new();
                sp.Name = name;
                db.Specialitys.Add(sp);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await putPosts();
                return View("Speciality");
            }
        }
        public async Task<IActionResult> EditSpeciality(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            Speciality sp = await db.Specialitys.FirstOrDefaultAsync(m => m.Id == id);
            if (sp != null)
            {              
                return View("EditSpeciality",sp);
            }
            await putSpecialities();
            return View("Speciality");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSpeciality(int id,string name)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                Speciality sp = await db.Specialitys.FirstOrDefaultAsync(m => m.Id == id);
                if(sp == null)
                {
                    await putSpecialities();
                    return View("Speciality");
                }
                sp.Name = name;
                db.Specialitys.Update(sp);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await putSpecialities();
                return View("Speciality");
            }
        }
        public async Task putPosts()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<Post> p = await db.Posts.ToListAsync();
            ViewData["PostId"] = new SelectList(p, "Id", "Name");
        }
        public async Task putSpecialities()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<Speciality> p = await db.Specialitys.ToListAsync();
            ViewData["SpecialityId"] = new SelectList(p, "Id", "Name");
        }*/
    }
}
