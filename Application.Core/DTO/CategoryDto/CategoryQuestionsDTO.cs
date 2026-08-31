using Application.Core.DTO.QuestionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.CategoryDto
{
    public class CategoryQuestionsDTO 
    {
        public string? CategoryName { get; set; }
        public List<QuestionShowDTO>? Questions { get; set; }

    }
}
