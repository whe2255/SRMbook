using System.Text;
using Microsoft.AspNetCore.Mvc;
using SrmBook.Data;
using SrmBook.Models;
using System.Security.Cryptography;


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
             //sha256 단방향 암호화 후 비교하여 로그인
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(model.USER_PW);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                string enteredPasswordHash = BitConverter.ToString(hashBytes).Replace("-", "");

                if (ModelState.IsValid)
                {
                    using (var db = new BookUserContext())
                    {
                        var user = db.BookUser.FirstOrDefault(u => u.USER_ID == model.USER_ID && u.USER_PW == enteredPasswordHash); // 아이디 비밀번호 매칭

                        if (user != null)//세션 부여
                        {
                            HttpContext.Session.SetString("USER_LOGIN_KEY", user.USER_TYPE);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");

            }

            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USER_LOGIN_KEY");
            return RedirectToAction("index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(BookUser model)
        {
            //sha256 단방향 암호화
            string input = model.USER_PW;
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                model.USER_PW = builder.ToString();
            }
            //id중복체크
            if (IsDuplicateId(model))
            {
                ModelState.AddModelError("USER_ID", "중복된 ID입니다.");
            }
            if (ModelState.IsValid)
            {
                using (var db = new BookUserContext())
                {
                    db.BookUser.Add((model));
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        //id중복체크
        private bool IsDuplicateId(BookUser model)
        {
            using (var db = new BookUserContext())
            {
                // 해당 ID를 데이터베이스에서 조회합니다.
                var existingItem = db.BookUser.FirstOrDefault(u => u.USER_ID == model.USER_ID);

                // 중복 여부를 확인합니다.
                if (existingItem != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}