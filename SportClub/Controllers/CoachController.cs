using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;

namespace SportClub.Controllers
{
    public class CoachController : Controller
    {
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        public CoachController(IAdmin adm, IUser us, ICoach c, ISpeciality sp, IPost p)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
        }

        // GET: Users
        public async Task<IActionResult> GetCoaches()
        {
            var p = await coachService.GetAllCoaches();
            await putPosts();
            await putSpecialities();
            return View(p);
        }


        public async Task<IActionResult> CoachProfile()
        {
            string s = HttpContext.Session.GetString("Id");
            int id = Int32.Parse(s);
            CoachDTO p = await coachService.GetCoach(id);
            return View(p);
        }
        public async Task<IActionResult> Details(int id)
        {
            CoachDTO p = await coachService.GetCoach(id);
            return View(p);
        }
        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    CoachDTO p = await coachService.GetCoach(id);
        //    await putPosts();
        //    await putSpecialities();
        //    return View(p);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(CoachDTO c)
        //{
        //    try
        //    {
        //        CoachDTO p = await coachService.GetCoach(c.Id);
        //        p.PostId = c.PostId;
        //        p.SpecialityId = c.SpecialityId;
        //        p.Description = c.Description;
        //        p.Phone= c.Phone;
        //        p.Email= c.Email;
        //        p.Photo = c.Photo;
        //        await coachService.UpdateCoach(p);
        //        return RedirectToAction("GetCoaches");
        //    }
        //    catch { return View(c); }
        //}

        public async Task<IActionResult> Edit(int id)/*Coach*/
        {
            HttpContext.Session.SetString("path", Request.Path);
            CoachDTO coachdto = await coachService.GetCoach(id);
            if (coachdto != null)
            {
                await putSpecialities();
                await putPosts();
                return View("Edit", coachdto);/*Coach*/
            }

            return View("GetCoaches"/*, "Coach"*/);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoachDTO coach, IFormFile Photo_URL)/*Coach*/
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                CoachDTO coachdto = await coachService.GetCoach(id);
                if (coachdto == null)
                {
                    return NotFound();
                }


                if (ModelState.IsValid)
                {
                    coachdto = coach;

                    try
                    {
                        await coachService.UpdateCoach(coachdto);
                    }
                    catch { return View("Edit", coach); }/*Coach*/
                }
                return RedirectToAction("GetCoaches");
            }
            catch
            {
                return View("GetCoaches"/*, "Coach"*/);
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
