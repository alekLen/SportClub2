﻿using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
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
        private readonly ITrainingGroup trainingGroupService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;

        public GroupController(ITrainingGroup tg, IGroup group, IUser us, ICoach c, ISpeciality sp, IPost p, IWebHostEnvironment _appEnv)
        {
            trainingGroupService = tg;
            groupService = group;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
            _appEnvironment = _appEnv;
        }
        //public async Task<IActionResult> CreateGroup()
        //{
        //    HttpContext.Session.SetString("path", Request.Path);
        //    //await putSpecialities();Speciality 
        //    return View("CreateGroup");
        //}

        [HttpPost]
        public async Task<IActionResult> CreateGroup(string name, int number)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                GroupDTO g = new();
                g.Name = name;
                g.Number = number;
                g.UsersId = new();
                await groupService.AddGroup(g);
                //HttpContext.Session.SetString("speciality", name);
                HttpContext.Session.SetString("Name", name);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> AddedGroup(string groupname)
        {
            HttpContext.Session.SetString("path", Request.Path);
            HttpContext.Session.SetString("groupname", groupname);
            return PartialView();
        }
        //public async Task<IActionResult> CreateGroup()
        //{
        //    HttpContext.Session.SetString("path", Request.Path);
        //    //await putCoaches();
        //    //await putUsers();
        //    return View("CreateGroup");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateGroup(GroupAndTrainingGroup group, int[] UsersList)
        //{
        //    HttpContext.Session.SetString("path", Request.Path);

        //    if (ModelState.IsValid)
        //    {
        //        //var coac = await coachService.GetCoach(group.CoachId);
        //        GroupDTO u = new();
        //        u.Name = group.group.Name;

        //        //u.CoachName = coac.Name;
        //        //u.CoachId = group.CoachId;
        //        foreach (var id in UsersList)
        //        {
        //            u.UsersId.Add(await userService.GetUser(id));
        //        }
        //        u.Number = group.group.Number;
        //        // u.UsersId = UsersList.ToList();//group.UsersId
        //        //u.Phone = group.Phone;
        //        //u.Name = group.Name;
        //        //u.DateOfBirth = group.DateOfBirth;
        //        //u.Password = group.Password;

        //        try
        //        {
        //            await groupService.AddGroup(u);
        //        }
        //        catch { } 
        //        //return RedirectToAction("Index", "Home");
        //    }  
        //    return View("CreateGroup", group);//дів откривати і закривати
        //}

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
            //ViewData["UsersListId"] = new SelectList(p, "Id", "Name"); 
            ViewData["UsersList"] = new SelectList(p, "Id", "Name");
        }

        public async Task<IActionResult> GetGroups()
        {
            var p = await groupService.GetAllGroups();
            return View(p);
        }

        public async Task putUsers(GroupDTO group)
        {
            HttpContext.Session.SetString("path", Request.Path);
            int c = group.UsersId.Count;
            IEnumerable<UserDTO> p = group.UsersId.ToList();
            //ViewData["UsersListId"] = new SelectList(p, "Id", "Name"); 
            ViewData["UsersList"] = new SelectList(p, "Id", "Name");
        }

        public async Task<IActionResult> AddUsersToTrainingGroup(int groupId, int roomId)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(groupId);
            HttpContext.Session.SetInt32("roomId", trgroupdto.RoomId);
            GroupDTO groupdto = await groupService.GetGroup(trgroupdto.GroupId);
            if (groupdto != null)
            {
                await putGroupUsers(groupdto.Id);
                await putUsers();
                return View("AddUsersToTrainingGroup", groupdto);
            }
            return View("GetGroups", groupdto);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUsersToTrainingGroup(int groupId,/* int roomId,*/ int[] UsersList)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                GroupDTO groupdto = await groupService.GetGroup(groupId);
                if (groupdto == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    //groupdto.Name = group.Name;
                    //groupdto.Number = group.Number;
                    //groupdto.Id = group.Id;

                    foreach (var i in UsersList)
                    {
                        groupdto.UsersId.Add(await userService.GetUser(i));
                    }

                    try { 
                        await groupService.UpdateGroup(groupdto); 
                    }
                    catch { return View("AddUsersToTrainingGroup"/*, group*/); }
                    return RedirectToAction("RoomWithShedule", "Time", new { Id = HttpContext.Session.GetInt32("roomId") });
                    //return RedirectToAction("GetGroups", "Group");
                }
                return RedirectToAction("RoomWithShedule", "Time", new { Id = HttpContext.Session.GetInt32("roomId") });
                //return RedirectToAction("GetGroups");
            }
            catch
            {
                return View("GetGroups", "Group");
            }
        }
        public async Task putGroupUsers(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<UserDTO> p = await groupService.GetGroupUsers(id);
            ViewData["UserstableId"] = new SelectList(p, "Id", "Name");
        }
        //public async Task<IActionResult> EditGroup(int Id)
        //{

        //    HttpContext.Session.SetString("path", Request.Path);
        //    TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(Id);
        //    GroupDTO groupdto = await groupService.GetGroup(trgroupdto.GroupId);
        //    if (groupdto != null)
        //    {
        //        await putUsers(); 
        //        return View("EditGroup", groupdto);
        //    } 
        //    return View("GetGroups", groupdto); 
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditGroup(int id, GroupDTO group, int[] UsersList)
        //{
        //    HttpContext.Session.SetString("path", Request.Path);
        //    try
        //    {
        //        GroupDTO groupdto = await groupService.GetGroup(id);
        //        if (groupdto == null)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        { 
        //            groupdto.Name = group.Name;
        //            groupdto.Number = group.Number; 
        //            groupdto.Id = group.Id;

        //            foreach (var i in UsersList)
        //            {
        //                groupdto.UsersId.Add(await userService.GetUser(i));
        //            }

        //            try
        //            {
        //                await groupService.UpdateGroup(groupdto);
        //            }
        //            catch { return View("EditGroup", group); }
        //            return RedirectToAction("GetGroups", "Group");
        //        }
        //        return RedirectToAction("GetGroups");
        //    }
        //    catch
        //    {
        //        return View("GetGroups", "Group");
        //    }
        //}

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
