using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;

namespace SrmBook.Controllers
{
    public class BookUserController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(BookLoginView model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BookUserContext())
                {
                    var user = db.BookUser.FirstOrDefault(u => u.USER_ID.Equals(model.USER_ID) && u.USER_PW.Equals(model.USER_PW)); // 아이디 비밀번호 매칭

                    if (user != null)
                    {
                        HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.USER_NUM);
                        return RedirectToAction("Index", "Home");
                    }

                }
                ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(BookUser model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BookUserContext())
                {
                    db.BookUser.Add(model); 
                    db.SaveChanges(); 
                }
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}