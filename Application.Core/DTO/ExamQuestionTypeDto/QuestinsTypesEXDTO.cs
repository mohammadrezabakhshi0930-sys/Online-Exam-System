using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExamQuestionTypeDto
{
    public class QuestinsTypesEXDTO
    {
        public Guid IdExam { get; set;}
        public List<QuestionTySingleDTO>? QuestionSelect {  get; set;}
        public List<QuestionTyCategoryDTO>? CategorySelect { get; set; }
    }
    public class QuestionTySingleDTO
    {
        public Guid IdExam { get; set; }
        public Guid IdQuestion { get; set; }
    }
    public class QuestionTyCategoryDTO
    {
        public Guid IdExam { get; set; }
        public Guid IdCategory { get; set; }
        public int Count { get; set; }
    }
}
