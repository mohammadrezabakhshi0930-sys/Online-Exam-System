using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExamQuestionTypeDto
{
    public class ExamQTypesShowDTO
    {
        public Guid Id { get; set; }
        public string? NameCategory { get; set; }
        public int? Count { get; set; }
        public string? TextQuestion {  get; set; }
        public string? TextAnswer { get; set; }
        public int? MaxScore { get; set; }
       
    }
}
