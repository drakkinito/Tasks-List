using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoList.Controllers
{
    public class GroupController : BaseController
    {
        private readonly ListContext _context;

        public GroupController(ListContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult CreateGroup()
        {
            // list users for select and delete item user in your account
            var users = _context.Users
                .Select(s => new PublicUserViewModel { Id = s.Id, Email = s.Email })
                .Where(x => x.Id != UserId);

            ViewBag.Users = users;

            return View(new CreateGroupViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateGroupViewModel model)
        {
            if (model != null)
            {
                GroupItem groupItem = new GroupItem();

                groupItem.Name = model.Name;
                groupItem.IsPrivate = model.IsPrivate;
                groupItem.AdminUserId = UserId;

                if (model.Users != null)
                {
                    var items = model.Users.Concat(new[] { UserId });

                    model.Users = items;

                    groupItem.Users = model.Users.Select(x => new UsersGroup
                    {
                        UserId = x
                    }).ToList();
                }
                else
                {
                    model.Users = new int[] { UserId };

                    groupItem.Users = model.Users.Select(x => new UsersGroup
                    {
                        UserId = x
                    }).ToList();
                }

                await _context.Groups.AddAsync(groupItem);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home", new { id = groupItem.Id });
            }

            return View();
        }

        [HttpGet]
        public IActionResult EditGroup(int id)
        {
            var group = _context.Groups
                .Include(x => x.Users).ThenInclude(x => x.User)
                .FirstOrDefault(u => u.Id == id);
            if (group != null)
            {
                // list users for select and delete item user in your account
                var users = _context.Users.Select(s =>
                    new PublicUserViewModel { Id = s.Id, Email = s.Email }).Where(u => u.Id != UserId);

                var usersChecked = group.Users.Select(x =>
                    new PublicUserViewModel { Email = x.User.Email, Id = x.User.Id });

                ViewBag.UsersChecked = usersChecked;
                ViewBag.Users = users;


                return View(new CreateGroupViewModel
                {
                    Id = id,
                    Name = group.Name,
                    IsPrivate = group.IsPrivate
                });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(CreateGroupViewModel model)
        {

            int taskId = 0;
            if (model != null)
            {

                var groupItem = _context.Groups
                    .Include(x => x.Users)
                    .FirstOrDefault(x => x.Id == model.Id);

                var taskItem = _context.UsersGroups
                    .Where(x => x.GroupItemId == model.Id)
                    .Select(x => x.UserId).ToList();

                List<UsersGroup> newUsers = new List<UsersGroup>();
                if (model.Users != null)
                {
                    foreach (int item in model.Users)
                    {
                        newUsers.Add(new UsersGroup { UserId = item, GroupItemId = model.Id });
                    }
                }

                newUsers.Add(new UsersGroup { UserId = UserId, GroupItemId = model.Id });

                groupItem.Users = newUsers;
                groupItem.Name = model.Name;
                groupItem.IsPrivate = model.IsPrivate;

                // delete assign user from task
                //var modelUsers = model.Users.ToArray();

                //for (int i = 1; i < taskItem.Count(); i++)
                //{
                //    for (int j = 0; j < modelUsers.Count(); j++)
                //    {
                //        if (taskItem[i] != modelUsers[j])
                //        {
                //            var assignUser = _context.Tasks.FirstOrDefault(x => x.UserId == taskItem[i]);
                //            assignUser.UserId = null;

                //            taskId = taskItem[i];
                //            //_context.SaveChanges();
                //        }

                //    }
                //}


                _context.SaveChanges();
            }


            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { id = model.Id, tId = taskId });
        }

        //[HttpPost]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            GroupItem rmGroup = _context.Groups.FirstOrDefault(x => x.Id == id);

            _context.Groups.Remove(rmGroup);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { id = 4 });
        }
    }

}
