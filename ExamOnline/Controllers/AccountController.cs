using Application.Core.Domain.Entites;
using Application.Core.DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExamOnline.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _User;
        private readonly SignInManager<ApplicationUser> _SignIn;
        public AccountController(UserManager<ApplicationUser> UserManager,SignInManager<ApplicationUser> SignInManager)
        {
            _User = UserManager;
            _SignIn = SignInManager;
        }
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO Login)
        {
            if(!ModelState.IsValid) return View(Login);
            Microsoft.AspNetCore.Identity.SignInResult Result = await _SignIn.PasswordSignInAsync(Login.UserName!, Login.Password!, Login.IsRemember, true);
            if (Result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "حساب کاربری شما برای امنیت بیشتر مدتی قفل شده است ");
                return View(Login);
            }
            if (!Result.Succeeded)
            {

                ModelState.AddModelError(string.Empty, "اطلاعات وارد شده صحیح نمباشد");
                return View(Login);
            }
            return RedirectToAction("Index","Home");
        }

        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await _SignIn.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO Register)
        {
            if (!ModelState.IsValid) return View(Register);
            ApplicationUser? CheckUserName = await _User.FindByNameAsync(Register.UserName!.Trim());
            if(CheckUserName != null)
            {
                ModelState.AddModelError("UserName", "نام کاربری تکراری می باشد");
                return View(Register);
            } 
            ApplicationUser NewUser = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                IsLogout = false,
                IsRegisterAdmin = false,
                Name = Register.Name!.Trim(),
                RegistrationDate = DateTime.Now,
                UserName = Register.UserName!.Trim(),
            };
            var result = await _User.CreateAsync(NewUser, Register.Password!.Trim());
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "ثبت نام ناموفق بود لطفا دوباره تلاش کنید");
                return View(Register);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _User.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _User.ChangePasswordAsync(user, model.OldPassword!, model.NewPassword!);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "رمز عبور شما با موفقیت تغییر یافت.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
