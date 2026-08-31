using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class UserInExamDTO
    {
        public Guid Id { get; set; }
        public string? NameUser { get; set; }
        public double? Score {  get; set; }
        public bool IsFinalScore { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

    }
}
