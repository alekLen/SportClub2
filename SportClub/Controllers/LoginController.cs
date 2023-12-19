
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.Filters;
using SportClub.Models;
using SportClub.BLL.Interfaces;
using SportClub.BLL.DTO;


namespace SportClub.Controllers
{

    [Culture]
    public class LoginController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        public LoginController(IAdmin adm,IUser us, ICoach c, ISpeciality sp,IPost p, IWebHostEnvironment _appEnv)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
            _appEnvironment = _appEnv;
        }

        int age { get; set; }
        [HttpGet]
        public IActionResult RegistrationClient()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View("RegisterClient");
        }
        public async Task<IActionResult> RegistrationCoach()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putSpecialities();
            await putPosts();
            return View("RegisterCoach");
        }
        public async Task<IActionResult> RegistrationAdmin()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putSpecialities();
            await putPosts();
            return View("RegisterAdmin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationAdmin(RegisterAdminModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
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
                AdminDTO u = new();
                u.Login = user.Login;
                u.Gender = user.Gender;
                u.Email = user.Email;
                u.Age = age;
                u.Phone = user.Phone;
                u.Name = user.Name;
              //  u.Surname = user.Surname;
              //  u.Dopname = user.Dopname;
                u.DateOfBirth = user.DateOfBirth;

                /*  byte[] saltbuf = new byte[16];
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
                  u.Password = hashedPassword;*/
                u.Password = user.Password;
                try
                {
                    /* db.Users.Add(u);
                    db.SaveChanges();
                    s.user = u;
                     db.Salts.Add(s);
                     db.SaveChanges();*/
                   await adminService.AddAdmin(u);
                }
                catch { }
                return RedirectToAction("Login");
            }
            return View("RegisterAdmin", user);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationCoach( RegisterCoachModel user, IFormFile p)
        {
            HttpContext.Session.SetString("path", Request.Path);

            try
            {
                // DateTime dateTime = DateTime.Parse(user.DateOfBirth);
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    DateTime currentDate = DateTime.Now;
                    age = currentDate.Year - birthDate.Year;
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
                if (p != null)
                {
                    string str = p.FileName.Replace(" ", "_");
                    string str1 = str.Replace("-", "_");
                    // Путь к папке Files
                    string path = "/Coaches/" + str1; // имя файла

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await p.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    CoachDTO u = new();
                    u.Login = user.Login;
                    u.Gender = user.Gender;
                    u.Email = user.Email;
                    u.Age = age;
                    u.Phone = user.Phone;
                    u.Photo = path;
                    u.Name = user.Name;
                  //  u.Surname = user.Surname;
                  //  u.Dopname = user.Dopname;
                    u.DateOfBirth = user.DateOfBirth;
                    u.Password = user.Password;
                    u.Description = user.Description;
                    u.PostId = user.PostId;
                    u.SpecialityId=user.SpecialityId;
                    try
                    {
                        await coachService.AddCoach(u);
                    }
                    catch { }
                    return RedirectToAction("Login");
                }
            }
            await putSpecialities();
            await putPosts();
            return View("RegisterCoach", user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationClient(RegisterClientModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);

            try
            {
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    DateTime currentDate = DateTime.Now;
                    age = currentDate.Year - birthDate.Year;
                    if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                    {
                        age--;
                    }
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения");
                }

            }
            catch { ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения"); }
            if (ModelState.IsValid)
            {
                UserDTO u = new();
                u.Login = user.Login;
                u.Gender = user.Gender;
                u.Email = user.Email;
                u.Age = age;
                u.Phone = user.Phone;
                u.Name = user.Name;
              //  u.Surname = user.Surname;
              //  u.Dopname = user.Dopname;
                u.DateOfBirth = user.DateOfBirth;
                u.Password = user.Password;
                try
                {
                    await userService.AddUser(u);
                }
                catch { return View("RegisterClient", user); }
                return RedirectToAction("Login");
            }
            return View("RegisterClient", user);
        }
        public IActionResult Login()
        {        
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel user)
        {
            HttpContext.Session.SetString("path", Request.Path);

            if (ModelState.IsValid)
            {
                UserDTO u = await userService.GetUserByLogin(user.Login);
                if (u==null)
                {
                    AdminDTO a= await adminService.GetAdminByLogin(user.Login);
                    if(a!=null)
                    {
                        if (await adminService.CheckPasswordA(a, user.Password))
                        {
                            HttpContext.Session.SetString("login", user.Login);
                            HttpContext.Session.SetString("admin", "admin");
                            HttpContext.Session.SetString("Id", a.Id.ToString());
                           // return RedirectToAction("Index", "Home");
                            return Json(true);
                        }
                        else
                        {
                            ModelState.AddModelError("", "логин или пароль неверные");
                            // return View(user);
                            return Json(false);
                        }
                    }
                    else
                    {
                        CoachDTO c = await coachService.GetCoachByLogin(user.Login);
                        if (c != null)
                        {
                            if (await coachService.CheckPasswordC(c, user.Password))
                            {
                                HttpContext.Session.SetString("login", user.Login);
                                HttpContext.Session.SetString("coach", "coach");
                                HttpContext.Session.SetString("Id", c.Id.ToString());
                               // return RedirectToAction("Index", "Home");
                                return Json(true);
                            }
                            else
                            {
                                ModelState.AddModelError("", "логин или пароль неверные");
                                //returnView(user);
                                return Json(false);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "логин или пароль неверные");
                            // returnView(user);
                            return Json(false);
                        }
                    }
                }           
                else
                {
                        if (await userService.CheckPasswordU(u, user.Password))
                        {
                             HttpContext.Session.SetString("login", user.Login);                            
                             HttpContext.Session.SetString("client", "client");
                             HttpContext.Session.SetString("Id", u.Id.ToString());
                       //  return RedirectToAction("Index", "Home");
                        return Json(true);
                    }
                        else
                        {
                            ModelState.AddModelError("", "логин или пароль неверные");
                        // returnView(user);
                        return Json(false);
                    }
                 }              
            }
            // return View(user);
            return Json(false);
        }
        [AcceptVerbs("Get", "Post")]
       public async Task<IActionResult> IsEmailInUse(string email)
        {
            UserDTO u = await userService.GetUserByEmail(email);
            CoachDTO c = await coachService.GetCoachByEmail(email);
            AdminDTO a = await adminService.GetAdminByEmail(email);
            if (u == null && c==null && a==null)
                return Json(true);
            else
                return Json(false);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsUserLoginInUse(string login)
        {
            UserDTO u = await userService.GetUserByLogin(login);         
            if (u == null)
                return Json(true);
            else
                return Json(false);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsAdminLoginInUse(string login)
        {
            AdminDTO a = await adminService.GetAdminByLogin(login);
            if (a == null)
                return Json(true);
            else
                return Json(false);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsCoachLoginInUse(string login)
        {
            CoachDTO c = await coachService.GetCoachByLogin(login);
            if (c == null)
                return Json(true);
            else
                return Json(false);
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // очищается сессия
            return RedirectToAction("Index", "Home");
        }
        [AcceptVerbs("Get", "Post")]
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
      /*  public async Task<bool> CheckPasswordU(User u, string p)
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
        }*/
       /* public async Task<bool> CheckPasswordA(AdminDTO u, string p)
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
        }*/
    /*    public async Task<bool> CheckPasswordC(Coach u, string p)
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
        }*/
      /*  public async Task<IActionResult> AddPost()
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
            PostDTO p = await postService.GetPost( id);
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
        }*/
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
