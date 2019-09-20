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

            var taskItems = _context.Tasks
                .Include(x=>x.Users)
                .Where(p => p.GroupItemId == id)
                .OrderByDescending(x => x.Id).ToList();

            var groupItems = _context.UsersGroups
                        .Include(c => c.GroupItem)
                        .Where(p => p.UserId == UserId).ToList();

            var users = _context.UsersGroups
                        .Include(x => x.User)
                        .Where(p => p.GroupItemId == id && p.UserId != UserId);

            var groupName = _context.Groups.FirstOrDefault(x => x.Id == id);

            User nameUser = _context.Users.FirstOrDefault(x => x.Id == UserId);

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
            if (message.Length == 1)
            {
                ViewBag.UserErrorMessage = message[0];
                ViewBag.UserErrorName = message[1];
            }

            //var assignUser = _context.Tasks.Include(x=>x.Users).FirstOrDefault(x => x.UserId == x.Users.Id);

            ViewData["UserEmail"] = nameUser.Email;
            ViewBag.GroupName = groupName;
            ////ViewBag.AssignUser = assignUser.Users.Email;

            return View(new GroupsTasksViewModel
            {
                Tasks = taskItems,
                UsersGroups = groupItems,
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
                            message = new string[] { "There is such Email", model.Name }
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
                    message = new string[] { "not found", model.Name }
                });
            }
            return RedirectToAction("Index", "Home", new { id = model.GroupId });
        }

        public IActionResult AddAssign(int id, AssignUserViewModel modelUser)
        {
            var assignUserId = Convert.ToInt32(modelUser.AssignUserId);
            var groupId = Convert.ToInt32(HttpContext.Request.Cookies["group_id"]);
            User user = _context.Users.FirstOrDefault(x => x.Id == assignUserId);
            if (user != null)
            {
                TaskItem taskItem = _context.Tasks.FirstOrDefault(x => x.Id == id);

                taskItem.Assign = false;
                taskItem.UserId = assignUserId;

                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home", new { id = groupId });
        }

        // get: Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // post: Create
        public async Task<IActionResult> CreateNewTask(TaskItem list)
        {
            //var id = Convert.ToInt32(HttpContext.Request.Cookies["user_id"]);
            string groupId = HttpContext.Request.Cookies["group_id"];
            if (list != null)
            {
                list.GroupItemId = Convert.ToInt32(groupId);
                list.Assign = null;

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
    }
}
