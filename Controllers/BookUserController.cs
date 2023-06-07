using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;


namespace SrmBook.Controllers
{
    public class BookUserController : Controller
    {
        private readonly BookUserContext _context;

        public BookUserController(BookUserContext context)
        {
            _context = context;
        }
        // 회원 가입
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
                //id중복체크
                if (IsIdDuplicate(model.USER_ID))
                {
                    ModelState.AddModelError("USER_ID", "이미 사용 중인 아이디입니다.");
                    return View(model);
                }
                // 비밀번호 암호화
                string hashedPassword = HashPassword(model.USER_PW);
                model.USER_PW = hashedPassword;
                _context.BookUser.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        // 회원 정보
        public IActionResult Index()
        {
            var users = _context.BookUser.ToList();
            return View(users);
        }
        // 로그인
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginView();
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(LoginView model)
        {
            var user = _context.BookUser.FirstOrDefault(u => u.USER_ID == model.USER_ID);

            if (user == null || !VerifyPassword(model.USER_PW, user.USER_PW))//암호화된 비밀번호 검증
            {
                ModelState.AddModelError("LoginFailed", "아이디 또는 비밀번호가 잘못되었습니다.");
                return View();
            }

            HttpContext.Session.SetString("USER_TYPE_KEY", user.USER_TYPE); // 권한 세션
            HttpContext.Session.SetString("USER_SESSION_KEY", user.USER_ID); // 세션

            return RedirectToAction("Index", "Home");
        }
        // 로그아웃
        public IActionResult Logout()
        { 
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        // 회원 수정
        [HttpGet]
        public IActionResult Edit(int userNum)
        {
            var user = _context.BookUser.FirstOrDefault(u => u.USER_NUM == userNum);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(BookUser model)
        {
            if (ModelState.IsValid)
            {
                //비밀번호 암호화
                string hashedPassword = HashPassword(model.USER_PW);
                model.USER_PW = hashedPassword;
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        // 회원 삭제
        [HttpGet]
        public IActionResult Delete(int userNum)
        {
            var user = _context.BookUser.FirstOrDefault(u => u.USER_NUM == userNum);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(BookUser model)
        {
            var user = _context.BookUser.FirstOrDefault(u => u.USER_NUM == model.USER_NUM);

            if (user == null)
            {
                return NotFound();
            }

            _context.BookUser.Remove(user);
            _context.SaveChanges();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // 비밀번호 암호화
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // 솔트 생성
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        // 비밀번호 검증
        private bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('.', 2);
            if (parts.Length != 2)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            string hashed = parts[1];

            string rehashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed == rehashed;
        }
        //id중복체크
        private bool IsIdDuplicate(string Id)
        {
            return _context.BookUser.Any(u => u.USER_ID == Id);
        }
    }
}
