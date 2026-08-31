using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExamQuestionTypeDto
{
    public class ExamConductDTO
    {
        public Guid ExamUserId { get; set; }
        public string? ExamTitle { get; set; }
        public int RemainingSeconds { get; set; }

        public Guid CurrentUserAnswerId { get; set; }
        public string? QuestionText { get; set; }
        public bool IsDescriptive { get; set; }
        public string? UserAnswerText { get; set; }

        public List<string> AnswerOptions { get; set; } = new();

        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
        public List<NavStepDto> Steps { get; set; } = new();
    }
    public class NavStepDto
    {
        public Guid UserAnswerId { get; set; }
        public int Index { get; set; }
        public bool IsAnswered { get; set; }
    }
}
