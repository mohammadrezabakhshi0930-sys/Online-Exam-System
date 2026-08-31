using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class CheckQuestionDTO
    {
        public string? QuestionAnswer { get; set; }
        public string? QuestionAnswerUser { get; set; }
        public string? Question {  get; set; }
        public int MaxScore { get; set; }
        public double? AssignedScore { get; set; }
        public Guid IdUserAnswer { get; set; }
    }
}
