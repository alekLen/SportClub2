using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.Filters;

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
    }
}
