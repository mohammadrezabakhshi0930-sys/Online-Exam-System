using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class DetailsStartExam
    {
       
            public Guid ExamId { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public int DurationMinutes { get; set; }
            public int TotalQuestions { get; set; }
            public int TotalScore { get; set; } 
            public int PassingScore { get; set; }
            public bool AlreadyParticipated { get; set; }
        public DateTime StartExam { get; set; }
        public DateTime EndExam { get; set; }

    }
}
