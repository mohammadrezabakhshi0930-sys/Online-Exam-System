using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.CertificateDto
{
    public class CertificateDetailsDTO
    {
        public string? FullName { get; set; }
        public string? ExamName { get; set; }
        public DateTime IssueDate { get; set; } 
        public double Score { get; set; }
        public double TotalScore { get; set; }
    }
}
