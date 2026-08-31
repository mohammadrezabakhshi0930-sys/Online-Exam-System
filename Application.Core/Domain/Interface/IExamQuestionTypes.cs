using Application.Core.DTO.ExamQuestionTypeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Interface
{
    public interface IExamQuestionTypes
    {
        Task<List<ExamQTypesShowDTO>> GetListQuestionType(Guid IdExam);
        Task<KeyValuePair<bool, string>> AddTypes(QuestinsTypesEXDTO Add, Guid Id);
        Task<KeyValuePair<bool, string>> Delete(Guid Id);

    }
}
