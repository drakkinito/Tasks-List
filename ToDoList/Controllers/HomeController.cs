using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string Search)
        {
            //var id = Convert.ToInt32(HttpContext.Request.Cookies["user_id"]);
           var items = from m in _context.Tasks
                       .Where(p=> p.UserId == UserId)
                       .OrderByDescending(x => x.Id)
                       select m;

            if (!String.IsNullOrEmpty(Search))
            {
                items = items.Where(s => s.Title.Contains(Search));
            }

            return View(await items.ToListAsync());
        }

        //post: Create
        public async Task<IActionResult> Create(TaskItem list)
        {
            //var id = Convert.ToInt32(HttpContext.Request.Cookies["user_id"]);
            if (list != null)
            {
                list.UserId = UserId;
                _context.Tasks.Add(list);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
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
    }
}
