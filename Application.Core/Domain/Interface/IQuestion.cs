using Application.Core.Domain.Entites;
using Application.Core.DTO.QuestionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Interface
{
    public interface IQuestion
    {
       Task<List<QuestionShowDTO>> GetQuestion(int Page, Guid UserId);
       Task<int> GetCountQuestion(Guid UserId);
       Task<KeyValuePair<bool,string>> AddQuestion(QuestionCreateDTO Add, Guid UserId);
       Task<KeyValuePair<bool, string>> EditQuestion(QuestionEditDTO Edit, Guid UserId);    
       Task<QuestionDetailsDTO?> GetDetailsQuestion(Guid QuestionId);
       Task<QuestionEditDTO?> GetSingleQuestion(Guid QuestionId,Guid UserId);
       Task<KeyValuePair<bool, string>> EditCategoryQuestion(Guid Id, Guid UserId);
        Task<List<QuestinListDTO>> GetList(Guid Id);

    }
}
