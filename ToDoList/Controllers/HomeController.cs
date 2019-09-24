using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;
using System.Collections.Generic;

namespace ToDoList.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ListContext _context;

        public HomeController(ListContext context)
        {
            _context = context;
        }

        [Authorize]
        //public async Task<IActionResult> Index(string Search)
        public async Task<IActionResult> Index(int id, string[] message, int userId)
        {
            HttpContext.Response.Cookies.Append("group_id", $"{id}");


            var groupItemsLoc = _context.UsersGroups
                        .Include(c => c.GroupItem)
                        .Where(p => p.UserId == UserId && p.GroupItem.IsPrivate != false)
                        .Select(x => new GroupItem
                            {
                            Name = x.GroupItem.Name,
                            Id = x.GroupItem.Id,
                            IsPrivate = x.GroupItem.IsPrivate,
                            UserRole = x.GroupItem.UserRole })
                        .ToList();

            var groupItemsGlob = _context.Groups
                .Where(x => x.IsPrivate == false)
                .Select(x => new GroupItem { Name = x.Name, Id = x.Id, IsPrivate = x.IsPrivate })
                .ToList();

            var role = _context.UsersGroups
                .Include(x => x.GroupItem)
                .Where(x => x.UserId == UserId)
                .Select(x => new GroupItem { UserRole = x.GroupItem.UserRole });

            var resultGroup = groupItemsLoc.Concat(groupItemsGlob);

            var users = _context.UsersGroups
                        .Include(x => x.User)
                        .Where(p => p.GroupItemId == id);

            var groupName = _context.Groups.FirstOrDefault(x => x.Id == id);

            User nameUser = _context.Users.FirstOrDefault(x => x.Id == UserId);

            // check isMygoup
            var isMygroup = _context.Groups.FirstOrDefault(x => x.Name == "My task" && x.Id == id);

            List<TaskItem> taskItems;

            if (isMygroup != null)
            {
                taskItems = _context.Tasks.Include(x => x.Users).Where(y => y.UserId == UserId).ToList();
            }
            else
            {
                taskItems = _context.Tasks
                      .Include(x => x.Users)
                      .Where(p => p.GroupItemId == id)
                      .OrderByDescending(x => x.Id).ToList();
            }

            if (id != 0)
            {
                //UsersGroup usersGroup = _context.UsersGroups.FirstOrDefault(x => x.GroupItemId == id);

                //ViewData["GroupId"] = usersGroup.GroupItemId;
            }

            if (userId != 0)
            {
                //var assign = _context.Users.FirstOrDefault(x => x.Id == userId);
                //ViewBag.AssignUser = assign.Email;
            }
            // show message add user to group
            if (message.Length != 0 && message.Length >= 0)
            {
                if (message[0] == "This field is required.")
                {
                    ViewBag.UserErrorMessage = message[0];
                }
                else
                {
                    ViewBag.UserErrorMessage = message[0];
                    ViewBag.UserErrorName = message[1];
                }
            }


            var assignUser = _context.Tasks.Include(x => x.Users).FirstOrDefault(x => x.UserId == x.Users.Id);

            //taskItems.Contains()

            ViewData["UserEmail"] = nameUser.Email;
            ViewBag.GroupName = groupName;
            ////ViewBag.AssignUser = assignUser.Users.Email;

            return View(new GroupsTasksViewModel
            {
                Tasks = taskItems,
                GroupItems = resultGroup,
                Users = users
            });
        }

        public async Task<IActionResult> AddUserForGroup(AddUserViewModel model)
        {
            if (model.Name != null)
            {
                var user = _context.Users
                    .Include(x => x.UsersGroups)
                    .FirstOrDefault(x => x.Email == model.Name);

                if (user != null)
                {
                    var a = _context.UsersGroups
                        .Include(x => x.User)
                        .Where(x => x.GroupItemId == model.GroupId && x.User.Email.Contains(model.Name))
                        .ToList();

                    if (a.Count > 0)
                    {
                        return RedirectToAction("Index", "Home", new
                        {
                            id = model.GroupId,
                            message = new string[] { "There is such Email.", model.Name }
                        });
                    }

                    UsersGroup usersGroup = new UsersGroup()
                    {
                        GroupItemId = model.GroupId,
                        UserId = user.Id
                    };
                    await _context.UsersGroups.AddAsync(usersGroup);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home", new { id = model.GroupId });
                }

                return RedirectToAction("Index", "Home", new
                {
                    id = model.GroupId,
                    message = new string[] { "User not found.", model.Name }
                });
            }
            return RedirectToAction("Index", "Home", new
            {
                id = model.GroupId,
                message = new string[] { "This field is required." }
            });
        }

        public IActionResult AddAssign(int tasksId, AssignUserViewModel modelUser)
        {
            var assignUserId = Convert.ToInt32(modelUser.AssignUserId);
            var groupId = Convert.ToInt32(HttpContext.Request.Cookies["group_id"]);

            User user = _context.Users.FirstOrDefault(x => x.Id == assignUserId);

            if (user != null)
            {
                TaskItem taskItem = _context.Tasks.FirstOrDefault(x => x.Id == tasksId);

                taskItem.isAssign = false;
                taskItem.UserId = assignUserId;

                var usersGroup = _context.UsersGroups
                    .Include(x => x.GroupItem)
                    .Where(x => x.UserId == UserId);
                var a = usersGroup.Where(x => x.GroupItem.Name.Contains("My task"));

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home", new { id = groupId });
        }

        // get: Create
        public IActionResult Create()
        {
            return View();
        }

        // post: Create
        public async Task<IActionResult> CreateNewTask(TaskItem list)
        {
            int groupId = Convert.ToInt32(HttpContext.Request.Cookies["group_id"]);
            if (list != null)
            {
                var getUserId = _context.UsersGroups
                    .Where(x => x.UserId == UserId && x.GroupItemId == groupId)
                    .ToList();

                if (getUserId != null)
                {
                    list.UserId = UserId;
                }

                list.GroupItemId = groupId;
                list.isAssign = null;

                await _context.Tasks.AddAsync(list);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home", new { id = groupId });
        }

        // get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Tasks.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Title, Text, ReleaseDate")] TaskItem taskVm)
        {
            string groupId = HttpContext.Request.Cookies["group_id"];
            var taskDb = await _context.Tasks.FindAsync(id);
            if (taskDb == null)
            {
                return NotFound();
            }

            taskDb.Title = taskVm.Title;
            taskDb.Text = taskVm.Text;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { id = groupId });
        }

        // get: Delete
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var list = await _context.Tasks
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (list == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(list);
        //}

        // post: Delete
        public async Task<IActionResult> Delete(int id)
        {
            string groupId = HttpContext.Request.Cookies["group_id"];

            if (id != 0)
            {
                var list = await _context.Tasks.FindAsync(id);

                _context.Tasks.Remove(list);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home", new { id = groupId });
            }
            return RedirectToAction("Index", "Home", new { id = groupId });
        }

        public async Task<IActionResult> FinishTask(int taskId)
        {
            string groupId = HttpContext.Request.Cookies["group_id"];

            if (taskId != 0)
            {
                var statusAssignUser = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (statusAssignUser.isAssign == true)
                    statusAssignUser.isAssign = false;
                else
                    statusAssignUser.isAssign = true;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home", new { id = 1 });
        }
    }
}
