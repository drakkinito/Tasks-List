using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToDoList.Controllers
{
    public class BaseController : Controller
    {
        public int UserId
        {
            get
            {
                return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sub").Value);
            }
        }
    }
}
