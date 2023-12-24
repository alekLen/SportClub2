using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.DAL.Entities;
using SportClub.Filters;
using SportClub.Models;

namespace SportClub.Controllers
{
    [Culture]
    public class AdminController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        public AdminController(IAdmin adm, IUser us, ICoach c, ISpeciality sp, IPost p, IWebHostEnvironment _appEnv)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
            _appEnvironment = _appEnv;
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
                PostDTO p = new();
                p.Name = name;
                await postService.AddPost(p);
                // return RedirectToAction("Index", "Home");
                await putPosts();
                return View("Post");
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
            PostDTO p = await postService.GetPost(id);
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
                PostDTO p = await postService.GetPost(id);
                if (p == null)
                {
                    await putPosts();
                    return View("Post");
                }
                p.Name = name;
                await postService.UpdatePost(p);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await putPosts();
                return View("Post");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            PostDTO p = await postService.GetPost(id);
            if (p != null)
            {
                return View("DeletePost", p);
            }
            await putPosts();
            return View("Post");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeletePost(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            PostDTO p = await postService.GetPost(id);
            if (p != null)
            {
                await postService.DeletePost(id);
                return View("Index", p);
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
                SpecialityDTO sp = new();
                sp.Name = name;
                await specialityService.AddSpeciality(sp);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await putSpecialities();
                return View("Speciality");
            }
        }
        public async Task<IActionResult> EditSpeciality(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            SpecialityDTO sp = await specialityService.GetSpeciality(id);
            if (sp != null)
            {
                return View("EditSpeciality", sp);
            }
            await putSpecialities();
            return View("Speciality");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSpeciality(int id, string name)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                SpecialityDTO sp = await specialityService.GetSpeciality(id);
                if (sp == null)
                {
                    await putSpecialities();
                    return View("Speciality");
                }
                sp.Name = name;
                await specialityService.UpdateSpeciality(sp);
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
            IEnumerable<PostDTO> p = await postService.GetAllPosts();
            ViewData["PostId"] = new SelectList(p, "Id", "Name");
        }
        public async Task putSpecialities()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<SpecialityDTO> p = await specialityService.GetAllSpecialitys();
            ViewData["SpecialityId"] = new SelectList(p, "Id", "Name");
        }

        public async Task<IActionResult> EditClient(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            UserDTO us = await userService.GetUser(id);
            if (us != null)
            {
                return View("EditClient", us);
            }
           
            return View("GetClients", "User");

        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClient(int id, RegisterClientModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                UserDTO us = await userService.GetUser(id);
                if (us == null)
                {
                    return NotFound();
                }
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    DateTime currentDate = DateTime.Now;
                    int age = currentDate.Year - birthDate.Year;
                    if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                    {
                        age--;
                    }
                    us.Age = age;
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения");
                }

                //if (ModelState.IsValid)
                //{
                    us.Gender = user.Gender;
                    us.Login = user.Login;
                    us.DateOfBirth = user.DateOfBirth;
                    us.Phone = user.Phone;
                    us.Name = user.Name;
                    us.Email = user.Email;
                    us.DateOfBirth = user.DateOfBirth;
                    us.Password = user.Password;

                    try
                    {
                        await userService.UpdateUser(us);
                    }
                    catch { return View("EditClient", us); }
                //}
                return RedirectToAction("GetClients", "Users");
            }
            catch
            {
                return View("GetClients");
            }
        }


        public async Task<IActionResult> EditAdmin(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            AdminDTO us = await adminService.GetAdmin(id);
            if (us != null)
            {
                return View("EditAdmin", us);
            }

            return View("GetAdmins", "Admin");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(int id, RegisterAdminModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                AdminDTO a = await adminService.GetAdmin(id);
                if (a == null)
                {
                    return NotFound();
                }
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    DateTime currentDate = DateTime.Now;
                    int age = currentDate.Year - birthDate.Year;
                    if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                    {
                        age--;
                    }
                    a.Age = age;
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения");
                }

                //if (ModelState.IsValid)
                //{
                    a.Gender = user.Gender;
                    a.Login = user.Login;
                    a.DateOfBirth = user.DateOfBirth;
                    a.Phone = user.Phone;
                    a.Name = user.Name;
                    a.Email = user.Email;
                    a.DateOfBirth = user.DateOfBirth;
                    a.Password = user.Password;
                   
                    try
                    {
                        await adminService.UpdateAdmin(a);
                    }
                    catch { return View("EditAdmin", user); }
                //}
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View("GetAdmins", "Admin");
            }
        }

        public async Task<IActionResult> GetAdmins()
        {
            var p = await adminService.GetAllAdmins();
            await putPosts();
            await putSpecialities();
            return View(p);
        }
    }
}
