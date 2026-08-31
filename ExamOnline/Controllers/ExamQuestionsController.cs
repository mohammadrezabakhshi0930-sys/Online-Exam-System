using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Core.DTO.CategoryDto;
using Application.Core.DTO.ExamQuestionTypeDto;
using Application.Core.DTO.QuestionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamOnline.Controllers
{
    [Route("ExamQuestions")]
    [Authorize(Roles = "Admin")]
    public class ExamQuestionsController : Controller
    {
        private readonly IExamQuestionTypes _TyQuestions;
        private readonly ICategory _Category;
        private readonly IQuestion _Question;



        public ExamQuestionsController(IExamQuestionTypes Tyquestion,ICategory category,IQuestion  question)
        {
            _TyQuestions = Tyquestion;
            _Category = category;
            _Question = question;
        }

        [Route("Details/{Id}")]
        public async Task<IActionResult> Details(Guid Id)
        {
            List<ExamQTypesShowDTO> Details = await _TyQuestions.GetListQuestionType(Id);
            ViewBag.IdExam = Id;
            return View(Details);
        }

        [HttpGet]
        [Route("AddTypes")]
        public async Task<IActionResult> AddTypes(Guid IdExam)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            List<CategoryListDTO> Category = await _Category.GetList(UserId);
            ViewBag.ListCategory = Category;

            List<QuestinListDTO> Questions  = await _Question.GetList(UserId);
            ViewBag.ListQuesion = Questions;

            ViewBag.Id = IdExam;

            return View(new QuestinsTypesEXDTO());
        }

        [HttpPost]
        [Route("AddTypes")]
        public async Task<IActionResult> AddTypes(QuestinsTypesEXDTO Add)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<bool, string> ResultAdd = await _TyQuestions.AddTypes(Add, UserId);

            if (!ResultAdd.Key)
            {
                List<CategoryListDTO> Category = await _Category.GetList(UserId);
                ViewBag.ListCategory = Category;

                List<QuestinListDTO> Questions = await _Question.GetList(UserId);
                ViewBag.ListQuesion = Questions;
                ViewBag.Id = Add.IdExam;

                ModelState.AddModelError(string.Empty, ResultAdd.Value);
                return View(Add);
            }
            TempData["Message"] = ResultAdd.Value;
            TempData["MessageType"] = "info"; // گزینه‌ها: success, warning, danger, info
            return Redirect($"/ExamQuestions/Details/{Add.IdExam}");

        }

        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            KeyValuePair<bool,string> Result = await _TyQuestions.Delete(Id);
            return Json(new { Success = Result.Value,Data = Result.Value });
        }

    }
}
