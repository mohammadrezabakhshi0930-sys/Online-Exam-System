using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class ExamUsersDetailsDTO
    {
        public string? FullName { get; set; }
        public string? ExamTitle { get; set; }
        public DateTime StartTime { get; set; } 
        public DateTime? EndTime { get; set; }

        public double TotalScore { get; set; }     
        public double? ObtainedScore { get; set; }  
        public double? PassingScore { get; set; }   
        public bool IsCorrected { get; set; }
        public List<QuestionDetailsForExamDTO>? QouestionUser { get; set; }
    }
    public class QuestionDetailsForExamDTO
    {
        public string? QuestionAnswer { get; set; }
        public string? QuestionAnswerUser { get; set; }
        public string? Question { get; set; }
        public int MaxScore { get; set; }
        public double? AssignedScore { get; set; }
        public Guid IdUserAnswer { get; set; }
        public bool IsIsCorrected { get; set; }
    }
}
