using Application.Core.Domain.Interface;
using Application.Core.DTO.CategoryDto;
using Application.Core.DTO.QuestionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamOnline.Controllers
{
    [Route("Category")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategory _Category;

        public CategoryController( ICategory category)
        {
            _Category = category;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            List<CategoryShowDTO> categories = await _Category.GetCategory(1, UserId);
            int CountCategory = await _Category.GetCountCategory(UserId);

            ViewBag.Page = 1;
            ViewBag.CountPage = Math.Max(1, (int)Math.Ceiling((double)CountCategory / 50));

            return View(categories);
        }

        [Route("GetCategory")]
        public async Task<IActionResult> GetCategory(int Page = 1)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            int CountCategory = await _Category.GetCountCategory(UserId);
            int CountPage = Math.Max(1, (int)Math.Ceiling((double)CountCategory / 50));
            Page = Math.Clamp(Page, 1, CountPage);

            List<CategoryShowDTO> categories = await _Category.GetCategory(Page, UserId);

            ViewBag.Page = Page;
            ViewBag.CountPage = Math.Max(1, (int)Math.Ceiling((double)CountCategory / 50));

            return PartialView(categories);
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(string CategoryName)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(CategoryName)) return Json(new { Success = false, Data = "نام دسته بندی نباید خالی باشد" });

            KeyValuePair<bool, string> ResultAdd = await _Category.AddCategory(CategoryName, UserId);

            return Json(new { Success = ResultAdd.Key, Data = ResultAdd.Value });

        }

        [HttpPost]
        [Route("EditCategory")]
        public async Task<IActionResult> EditCategory(CategoryListDTO Edit)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(Edit.Name)) return Json(new { Success = false, Data = "نام دسته بندی نباید خالی باشد" });

            KeyValuePair<bool, string> ResultEdit = await _Category.EditCategory(Edit, UserId);

            return Json(new { Success = ResultEdit.Key, Data = ResultEdit.Value });
        }

        [Route("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<bool, string> ResultDelete = await _Category.DeleteCategory(Id, UserId);

            return Json(new { Success = ResultDelete.Key, Data = ResultDelete.Value });
        }

        [Route("QuestionCategory/{Id}")]
        public async Task<IActionResult> QuestionCategory(Guid Id)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            CategoryQuestionsDTO? QuestionCategory = await _Category.GetQuestionCategory(Id, UserId);

            if (QuestionCategory == null) return NotFound();

            return View(QuestionCategory);
        }
    }
}
