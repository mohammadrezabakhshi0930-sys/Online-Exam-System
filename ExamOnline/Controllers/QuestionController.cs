using Application.Core.Domain.Interface;
using Application.Core.DTO.CategoryDto;
using Application.Core.DTO.QuestionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExamOnline.Controllers
{
    [Route("Question")]
    [Authorize(Roles ="Admin")]
    public class QuestionController : Controller
    {
        private readonly IQuestion _Question;
        private readonly ICategory _Category;

        public QuestionController(IQuestion question,ICategory category) 
        {
           _Question = question;
            _Category = category;
        }
       
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            List<QuestionShowDTO> Questions = await _Question.GetQuestion(1,UserId);
            int CountQuestion = await _Question.GetCountQuestion(UserId);

            ViewBag.Page = 1;
            ViewBag.CountPage = Math.Max(1, (int)Math.Ceiling((double)CountQuestion / 50)); 

            return View(Questions);
        }

        [Route("GetQuestion")]
        public async Task<IActionResult> GetQuestion(int Page=1)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");
            
            int CountQuestion = await _Question.GetCountQuestion(UserId);
            int CountPage = Math.Max(1, (int)Math.Ceiling((double)CountQuestion / 50));           
            Page = Math.Clamp(Page, 1, CountPage);
           
            List<QuestionShowDTO> Questions = await _Question.GetQuestion(Page, UserId);

            ViewBag.Page = Page;
            ViewBag.CountPage = Math.Max(1, (int)Math.Ceiling((double)CountQuestion / 50));

            return PartialView(Questions);
        }

        [HttpGet]
        [Route("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion()
        {
            string? UserClaim= User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login","Account");
            
            List<CategoryListDTO> Category =await _Category.GetList(UserId);
            ViewBag.ListCategory = Category;
            
            return View();
        }

        [HttpPost]
        [Route("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion(QuestionCreateDTO Add)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");
  
            if (!ModelState.IsValid) 
            {
                List<CategoryListDTO> Category = await _Category.GetList(UserId);
                ViewBag.ListCategory = Category;
                return View(Add);
            } 
            
            KeyValuePair<bool, string> ResultAdd =await _Question.AddQuestion(Add,UserId);

            if (!ResultAdd.Key)
            {
                List<CategoryListDTO> Category = await _Category.GetList(UserId);
                ViewBag.ListCategory = Category;
                ModelState.AddModelError(string.Empty, ResultAdd.Value);
                return View(Add);
            }
            
            return RedirectToAction("Index");
        }

        [Route("Details/{Id}")]
        public async Task<IActionResult> Details(Guid Id)
        {
            QuestionDetailsDTO? Details = await _Question.GetDetailsQuestion(Id);
            if (Details == null) return NotFound();
            return View(Details);
        }

        [HttpGet]
        [Route("EditQuestion/{Id}")]
        public async Task<IActionResult> EditQuestion(Guid Id)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            QuestionEditDTO? Edit = await _Question.GetSingleQuestion(Id, UserId);
            if (Edit == null) return NotFound();
            
            List<CategoryListDTO> Category = await _Category.GetList(UserId);
            ViewBag.ListCategory = Category;

            return View(Edit);
        }

        [HttpPost]
        [Route("EditQuestion/{Id}")]
        public async Task<IActionResult> EditQuestion(QuestionEditDTO Edit)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                List<CategoryListDTO> Category = await _Category.GetList(UserId);
                ViewBag.ListCategory = Category;
                return View(Edit);
            }

            KeyValuePair<bool, string> ResultEdit = await _Question.EditQuestion(Edit, UserId);

            if (!ResultEdit.Key)
            {
                List<CategoryListDTO> Category = await _Category.GetList(UserId);
                ViewBag.ListCategory = Category;
                ModelState.AddModelError(string.Empty, ResultEdit.Value);
                return View(Edit);
            }

            return RedirectToAction("Index");
        }

        [Route("EditCategoryQuestion")]
        public async Task<IActionResult> EditCategoryQuestion(Guid Id)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<bool, string> ResultEdit = await _Question.EditCategoryQuestion(Id, UserId);

            return Json(new { Success = ResultEdit.Key, Data = ResultEdit.Value });

        }
    }
}
