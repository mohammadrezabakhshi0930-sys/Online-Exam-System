using Application.Core.DTO.ExaminationDto;
using Application.Core.DTO.ExamQuestionTypeDto;
using Application.Core.DTO.QuestionDto;
using Application.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Interface
{
    public interface IExamination
    {
        Task<List<ExamShowDTO>> GetExam(int Page, Guid UserId);
        Task<int> GetCountExam(Guid UserId);
        Task<KeyValuePair<bool, string>> AddExam(ExamCreateDTO Add, Guid UserId);
        Task<KeyValuePair<bool, string>> EditExam(ExamEditDTO Edit, Guid UserId);
        Task<ExamDetailsDTO?> GetDetailsExam(Guid ExamId);
        Task<ExamEditDTO?> GetSingleExam(Guid ExamId, Guid UserId);
        Task<KeyValuePair<bool, string>> StartExam(Guid IdExam, Guid UserId);
        Task<ExamResultDTO?> ResultExam(Guid IdExam, Guid IdUser);
        Task<KeyValuePair<SaveAnswerStatus, string>> SaveAnswerAsync(Guid IdUserAnswer, Guid UserId,string Answer);
        Task<KeyValuePair<bool, string>> FinalizeAndQueueExamAsync(Guid IdExam, Guid UserId);
        Task<List<MyExamDTO>> GetMyExam(Guid UserId);
        Task<ExamConductDTO?> GetCurrentQuestion(Guid IdExamUser,Guid UserId,Guid? IdQuestionNow);
        Task<DetailsStartExam?> GetDetailsStartExam(Guid IdExam, Guid IdUser);
        Task<List<UserInExamDTO>> GetUserInExam(Guid IdExam, Guid IdUser);
        Task<ExamUserCheckDTO?> GetQuestionNotScore(Guid IdUserExam, Guid IdUser);
        Task<KeyValuePair<bool, string>> SubmitScore(Guid IdUserAnser, Guid UserId, double Score);
        Task<ExamUsersDetailsDTO?> GetExamUsersDetails(Guid IdExamUser, Guid IdUser);
    }
}
