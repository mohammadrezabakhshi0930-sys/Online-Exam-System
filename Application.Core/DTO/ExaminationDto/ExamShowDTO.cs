using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class ExamShowDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime DateCreate { get; set; }
        public bool HaveQuestion { get; set; }
        public int Status { get; set; }
        public int MaxScore { get; set; }
    }
}
