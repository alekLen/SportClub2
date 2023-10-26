using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.Filters;
using SportClub.Models;
using SportClub.BLL.Interfaces;
using SportClub.BLL.DTO;


namespace SportClub.Controllers
{
    [Culture]
    public class UsersController : Controller
    {
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        public UsersController(IAdmin adm, IUser us, ICoach c, ISpeciality sp, IPost p)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
        }

        // GET: Users
        public async Task<IActionResult> Coaches()
        {
            /* return _context.Users != null ?
                         View(await _context.Users.ToListAsync()) :
                         Problem("Entity set 'SportClubContext.Users'  is null.");*/
            var p= await coachService.GetAllCoaches();
            await putPosts();
            await putSpecialities();
            return View(p);
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
        // GET: Users/Details/5
        /*  public async Task<IActionResult> Details(int? id)
          {
              if (id == null || _context.Users == null)
              {
                  return NotFound();
              }

              var user = await _context.Users
                  .FirstOrDefaultAsync(m => m.Id == id);
              if (user == null)
              {
                  return NotFound();
              }

              return View(user);
          }

          // GET: Users/Create
          public IActionResult Create()
          {
              return View();
          }

          // POST: Users/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Phone,Email,Sex,Description,Photo,Login,Password,Status")] User user)
          {
              if (ModelState.IsValid)
              {
                  _context.Add(user);
                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }
              return View(user);
          }

          // GET: Users/Edit/5
          public async Task<IActionResult> Edit(int? id)
          {
              if (id == null || _context.Users == null)
              {
                  return NotFound();
              }

              var user = await _context.Users.FindAsync(id);
              if (user == null)
              {
                  return NotFound();
              }
              return View(user);
          }

          // POST: Users/Edit/5
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Phone,Email,Sex,Description,Photo,Login,Password,Status")] User user)
          {
              if (id != user.Id)
              {
                  return NotFound();
              }

              if (ModelState.IsValid)
              {
                  try
                  {
                      _context.Update(user);
                      await _context.SaveChangesAsync();
                  }
                  catch (DbUpdateConcurrencyException)
                  {
                      if (!UserExists(user.Id))
                      {
                          return NotFound();
                      }
                      else
                      {
                          throw;
                      }
                  }
                  return RedirectToAction(nameof(Index));
              }
              return View(user);
          }

          // GET: Users/Delete/5
          public async Task<IActionResult> Delete(int? id)
          {
              if (id == null || _context.Users == null)
              {
                  return NotFound();
              }

              var user = await _context.Users
                  .FirstOrDefaultAsync(m => m.Id == id);
              if (user == null)
              {
                  return NotFound();
              }

              return View(user);
          }

          // POST: Users/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(int id)
          {
              if (_context.Users == null)
              {
                  return Problem("Entity set 'SportClubContext.Users'  is null.");
              }
              var user = await _context.Users.FindAsync(id);
              if (user != null)
              {
                  _context.Users.Remove(user);
              }

              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
          }

          private bool UserExists(int id)
          {
              return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
          }
          public async Task<IActionResult> GetClients()
          {
              HttpContext.Session.SetString("path", Request.Path);

              IEnumerable<User> s = await _context.Users.ToListAsync();

             // ViewBag.Users = s;
              return View("Clients",s);
          }*/
    }
}
