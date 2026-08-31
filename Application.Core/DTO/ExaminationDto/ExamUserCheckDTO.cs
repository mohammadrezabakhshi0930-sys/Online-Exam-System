using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class ExamUserCheckDTO
    {
        public string? Name { get; set; }
        public string? ExamName { get; set; }
        public List<CheckQuestionDTO>? QuestionExam { get; set; }
    }
}
