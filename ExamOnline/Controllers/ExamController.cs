using Application.Core.Domain.Interface;
using Application.Core.DTO.CategoryDto;
using Application.Core.DTO.ExaminationDto;
using Application.Core.DTO.ExamQuestionTypeDto;
using Application.Core.DTO.QuestionDto;
using Application.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamOnline.Controllers
{
    [Route("Exam")]
    public class ExamController : Controller
    {
        private readonly IExamination _Examination;

        public ExamController(IExamination examination)
        {
            _Examination = examination;
        }

        [Route("Index")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            List<ExamShowDTO> exams = await _Examination.GetExam(1, UserId);
            int CountExam = await _Examination.GetCountExam(UserId);

            ViewBag.Page = 1;
            ViewBag.CountPage = Math.Max(1, (int)Math.Ceiling((double)CountExam / 50));

            return View(exams);
        }

        [Route("GetExam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetExam(int Page = 1)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            int CountExam = await _Examination.GetCountExam(UserId);
            int CountPage = Math.Max(1, (int)Math.Ceiling((double)CountExam / 50));
            Page = Math.Clamp(Page, 1, CountPage);

            List<ExamShowDTO> exams = await _Examination.GetExam(Page, UserId);

            ViewBag.Page = Page;
            ViewBag.CountPage = Math.Max(1, (int)Math.Ceiling((double)CountExam / 50));

            return PartialView(exams);
        }

        [HttpGet]
        [Route("CreateExam")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateExam()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateExam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateExam(ExamCreateDTO Add)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(Add);

            KeyValuePair<bool, string> ResultAdd = await _Examination.AddExam(Add, UserId);

            if (!ResultAdd.Key)
            {
                ModelState.AddModelError(string.Empty, ResultAdd.Value);
                return View(Add);
            }

            return RedirectToAction("Index");
        }

        [Route("Details/{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid Id)
        {
            ExamDetailsDTO? Details = await _Examination.GetDetailsExam(Id);
            if (Details == null) return NotFound();
            return View(Details);
        }

        [HttpGet]
        [Route("EditExam/{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExam(Guid Id)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            ExamEditDTO? Edit = await _Examination.GetSingleExam(Id, UserId);
            if (Edit == null) return NotFound();

            return View(Edit);
        }

        [HttpPost]
        [Route("EditExam/{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExam(ExamEditDTO Edit)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(Edit);

            KeyValuePair<bool, string> ResultEdit = await _Examination.EditExam(Edit, UserId);

            if (!ResultEdit.Key)
            {
                ModelState.AddModelError(string.Empty, ResultEdit.Value);
                return View(Edit);
            }

            return RedirectToAction("Index");
        }


        [Route("UserInExam/{IdExam}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserInExam(Guid IdExam)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            List<UserInExamDTO> Result = await _Examination.GetUserInExam(IdExam, UserId);

            return View(Result);
        }


        [Route("ExamCheckAnswer/{IdUserExam}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamCheckAnswer(Guid IdUserExam)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            ExamUserCheckDTO? Result = await _Examination.GetQuestionNotScore(IdUserExam, UserId);
            if (Result == null) return NotFound();

            return View(Result);
        }

        [Route("SubmitScore")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SubmitScore(Guid answerId, double score)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<bool, string> Result = await _Examination.SubmitScore(answerId, UserId, score);
            return Json(new { success = Result.Key, data = Result.Value });
        }


        [Route("ExamUserDetails/{UserExamId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamUserDetails(Guid UserExamId)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            ExamUsersDetailsDTO? Result = await _Examination.GetExamUsersDetails(UserExamId, UserId);
            if (Result == null) return NotFound();
            return View(Result);
        }

      
        
        
        
        
        
        
        
        
        
        
        
        
        
        [HttpGet]
        [Route("StartExam/{Id}")]
        public async Task<IActionResult> StartExam(Guid Id)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            DetailsStartExam? Start = await _Examination.GetDetailsStartExam(Id, UserId);
            if (Start == null) return NotFound();

            return View(Start);
        }

        [HttpPost]
        [Route("StartExamFinal")]
        public async Task<IActionResult> StartExamFinal(Guid ExamId)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<bool,string> Start = await _Examination.StartExam(ExamId, UserId);
            if(Start.Key == false)
            {
                TempData["ErrorMessage"] = Start.Value;

                return RedirectToAction("StartExam", new { Id = ExamId });
            }
            if (!Guid.TryParse(Start.Value,out Guid Id))
            {
                TempData["ErrorMessage"] = "عملیات ناموفق دوباره تلاش کنید";

                return RedirectToAction("StartExam", new { Id = ExamId });
            }
            return RedirectToAction("CheckQuestionExam", new { Id = Id });
        }

        [HttpGet]
        [Route("CheckQuestionExam")]
        public async Task<IActionResult> CheckQuestionExam(Guid Id, Guid? currentAnswerId = null)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            ExamConductDTO? Result = await _Examination.GetCurrentQuestion(Id,UserId,currentAnswerId);

            if (Result == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(Result);
        }

        [HttpPost]
        [Route("SaveAnswers")]
        public async Task<IActionResult> SaveAnswers(Guid examUserId,Guid currentUserAnswerId,string userAnswerText,Guid? nextAnswerId,bool isFinalSubmit = false)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<SaveAnswerStatus,string> saveResult = await _Examination.SaveAnswerAsync(currentUserAnswerId,UserId, userAnswerText);


                switch (saveResult.Key)
                {
                    case SaveAnswerStatus.Success:
                        
                    if (nextAnswerId.HasValue)
                    {
                            return RedirectToAction("CheckQuestionExam", new { Id = examUserId, currentAnswerId = nextAnswerId.Value });
                    }
                    if (isFinalSubmit)
                    {
                        return RedirectToAction("FinishExam", new { ExamId = examUserId });
                    }
                    return RedirectToAction("CheckQuestionExam", new { Id = examUserId, currentAnswerId = currentUserAnswerId });

                    case SaveAnswerStatus.TimeExpired:
                    TempData["ErrorMessage"] = saveResult.Value;
                    return RedirectToAction("FinishExam", new { ExamId = examUserId });

                    case SaveAnswerStatus.AlreadyFinished:
                    TempData["ErrorMessage"] = saveResult.Value;
                    return RedirectToAction("ResultExam", new { ExamId = examUserId });

                    case SaveAnswerStatus.QuestionNotFound:
                    TempData["ErrorMessage"] = saveResult.Value;
                    return RedirectToAction("Index","Home");
                default:
                        
                    TempData["ErrorMessage"] = saveResult.Value;
                    return RedirectToAction("Index", "Home");

                        
                }
        }

        [Route("FinishExam")]
        public async Task<IActionResult> FinishExam(Guid ExamId)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            KeyValuePair<bool, string> result = await _Examination.FinalizeAndQueueExamAsync(ExamId, UserId);

            if (result.Key == false)
            {
                TempData["ErrorMessage"] = result.Value;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ResultExam", new { IdExam = ExamId });
        }

        [Route("ResultExam")]
        public async Task<IActionResult> ResultExam(Guid IdExam)
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

             ExamResultDTO? Result = await _Examination.ResultExam(IdExam, UserId);

            if (Result == null)
            {
                TempData["ErrorMessage"] = "امتحان نامعتبر می باشد یا نتیجه این امتحان هنوز نیامده است ";
                return RedirectToAction("Index", "Home");
            }

            return View(Result);
        }

        [Route("GetMyExam")]
        public async Task<IActionResult> GetMyExam()
        {
            string? UserClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(UserClaim, out Guid UserId)) return RedirectToAction("Login", "Account");

            List<MyExamDTO> Result = await _Examination.GetMyExam(UserId);

            return View(Result);
        }



    }
}
