using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoList.Controllers
{
    public class Group : BaseController
    {
        private readonly ListContext _context;

        public Group(ListContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var itemsUsers = _context.Users.ToList();

            return View();
        }

        public async Task<IActionResult> CreateGroup(GroupItem model)
        {
            if (model != null)
            {
                GroupItem groupItem = new GroupItem
                { Name = model.Name, IsPrivate = model.IsPrivate, UserRole = model.UserRole };

                await _context.Groups.AddAsync(groupItem);
                _context.SaveChanges();

                groupItem.UsersGroups.Add(new UsersGroup { UserId = UserId, GroupItemId = groupItem.Id });
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

      

    }
}
