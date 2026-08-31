using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.QuestionDto
{
    public class QuestionShowDTO
    {
        public Guid Id { get; set; }
        public DateTime DateCreate { get; set; }
        public int Point { get; set; }
        public string? TextQuestion {  get; set; }
        public bool IsActive { get; set; }
        public bool QuestionType { get; set; }

    }
}
