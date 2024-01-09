using Azure;
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
    public class GroupController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private readonly IGroup groupService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        public GroupController(IGroup group, IUser us, ICoach c, ISpeciality sp, IPost p, IWebHostEnvironment _appEnv)
        {
            groupService = group;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
            _appEnvironment = _appEnv;
        }
        public async Task<IActionResult> CreateGroup()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putCoaches();
            await putUsers();
            return View("CreateGroup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(CreateGroupModel group)
        {
            HttpContext.Session.SetString("path", Request.Path);
            
            if (ModelState.IsValid)
            {
                var coac = await coachService.GetCoach(group.CoachId);
                GroupDTO u = new();
                u.Name = group.Name;
                u.Number = group.Number;
                u.CoachName = coac.Name;
                u.CoachId = group.CoachId;
                u.UsersId = group.UsersId;
                //u.Phone = group.Phone;
                //u.Name = group.Name;
                //u.DateOfBirth = group.DateOfBirth;
                //u.Password = group.Password;
                 
                try
                {
                    await groupService.AddGroup(u);
                }
                catch { }
                return RedirectToAction("Index", "Home");
            }
            return View("CreateGroup", group);
        }

        public async Task putCoaches()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<CoachDTO> p = await coachService.GetAllCoaches();
            ViewData["CoachListId"] = new SelectList(p, "Id", "Name");
        }
        public async Task putUsers()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<UserDTO> p = await userService.GetAllUsers();
            ViewData["UsersListId"] = new SelectList(p, "Id", "Name");
        }

        public async Task putCoach(GroupDTO groupdto)
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<UserDTO> p = groupdto.UsersId;
            ViewData["UsersLId"] = new SelectList(p, "Id", "Name");
        }
        public async Task<IActionResult> GetGroups()
        {
            var p = await groupService.GetAllGroups();
            return View(p);
        }
        public async Task<IActionResult> EditGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            GroupDTO groupdto = await groupService.GetGroup(id);
            if (groupdto != null)
            {
                await putCoaches();
                await putUsers();
                //await putCoach(groupdto);
                //var ts = groupdto.UsersId;
                
                return View("EditGroup", groupdto);
            }

            return View("GetCroups", "Group");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, GroupDTO group)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                GroupDTO groupdto = await groupService.GetGroup(id);
                if (groupdto == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var coac = await coachService.GetCoach(group.CoachId);

                    groupdto.Name = group.Name;
                    groupdto.Number = group.Number;
                    groupdto.CoachName = coac.Name;
                    groupdto.CoachId = group.CoachId;
                    groupdto.UsersId = group.UsersId;
                     
                    try
                    {
                        await groupService.UpdateGroup(groupdto);
                    }
                    catch { return View("EditGroup", group); }
                    return RedirectToAction("GetGroups", "Group");
                    // } 
                }
                return RedirectToAction("GetGroups");
            }
            catch
            {
                return View("GetGroupss", "Group");
            }
        }

        public async Task<IActionResult> DeleteGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            GroupDTO group = await groupService.GetGroup(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }
        [HttpPost, ActionName("DeleteGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GroupDTO group = await groupService.GetGroup(id);
            if (group == null)
            {
                return NotFound();
            }
            
            await groupService.DeleteGroup(id);
            return RedirectToAction("GetGroups", "Group"); 
        }
        public async Task<IActionResult> Details(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            GroupDTO group = await groupService.GetGroup(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }
    }
}
