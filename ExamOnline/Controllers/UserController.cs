using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Core.DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace ExamOnline.Controllers
{
    [Route("User")]
    [Authorize(Roles ="SuperAdmin")]
    public class UserController : Controller
    {
        
        private readonly IUser _User;
        private readonly UserManager<ApplicationUser> _UserMan;
        private readonly SignInManager<ApplicationUser> _SignIn;
        public UserController(IUser User, UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> SignInManager) 
        { 
            _User = User;
            _UserMan = UserManager;
            _SignIn = SignInManager;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            List<GetUserDTO> Result = await _User.GetAllUsers();
            return View(Result);
        }

        [Route("ChangeRole")]
        public async Task<IActionResult> ChangeRole(Guid UserId,string Role)
        {
            ApplicationUser? Find = await _UserMan.FindByIdAsync(UserId.ToString());
            if(Find == null)
            {
                TempData["Message"] = "کاربر مورد نظر یافت نشد";
                return RedirectToAction("Index");
            }
            if(await _UserMan.IsInRoleAsync(Find, Role))
            {
               await _UserMan.RemoveFromRoleAsync(Find, Role);
            }
            else
            {
                await _UserMan.AddToRoleAsync(Find, Role);
            }
            return RedirectToAction("Index");
        }
    }
}
