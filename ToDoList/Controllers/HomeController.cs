using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;

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
        public async Task<IActionResult> Index(int id)
        {
            HttpContext.Response.Cookies.Append("group_id", $"{id}");

            var taskItems = _context.Tasks
                .Where(p => p.GroupItemId == id)
                .OrderByDescending(x => x.Id);

            var groupItems = _context.UsersGroups
                        .Include(c => c.GroupItem)
                        .Where(p => p.UserId == UserId).ToList();

            User nameUser = _context.Users.FirstOrDefault(x => x.Id == UserId);
            if (id != 0)
            {
                UsersGroup usersGroup = _context.UsersGroups.FirstOrDefault(x => x.GroupItemId == id);

                ViewData["GroupId"] = usersGroup.GroupItemId;
            }

            ViewData["UserEmail"] = nameUser.Email;

            return View(new ModelGroupAndTask
            {
                Tasks = taskItems,
                UsersGroups = groupItems
            });
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
            var taskDb = await _context.Tasks.FindAsync(id);
            if (taskDb == null)
            {
                return NotFound();
            }

            taskDb.Title = taskVm.Title;
            taskDb.Text = taskVm.Text;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // get: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // post: Delete
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var list = await _context.Tasks.FindAsync(id);

            _context.Tasks.Remove(list);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //get show group
        //public async Task<IActionResult> Show(int id)
        //{
        //    var a = id;

        //    return View();
        //}


    }
}
