using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class ExamResultDTO
    {
        public string? ExamTitle { get; set; }
        public int TotalScore { get; set; } 
        public double? PassingScore { get; set; } 

        public double? Score { get; set; } 
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int Unanswered { get; set; }

        public bool IsCorrected { get; set; } 
        public bool HasCertificate { get; set; } 
        public Guid? CertificateUrl { get; set; }
        public double? Percentage => (Score.HasValue && TotalScore > 0)
     ? Math.Round((Score.Value / TotalScore) * 100, 0)
     : (double?)null;



        public bool HasPassingScore => PassingScore.HasValue;
        public bool? IsPassed => (IsCorrected && HasPassingScore && Score.HasValue)
            ? Score.Value >= PassingScore.Value
            : (bool?)null;

        public bool CanDownloadCertificate => IsCorrected && IsPassed == true && HasCertificate && CertificateUrl != null;
    }
}
