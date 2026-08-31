using Application.Core.Domain.Entites;
using Application.Core.DTO.AnswerDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.QuestionDto
{
    public class QuestionDetailsDTO
    {
        public string? TextQuestion { get; set; }

        public int Point { get; set; }
       
        public bool IsActive { get; set; } = true;
       
        public string? CategoryName { get; set; }
        
        public DateTime DateCreate { get; set; } 
        public bool IsDescriptiveQuestion { get; set; }
        public string? CorrectAnswer { get; set; }
        
        public List<AnswerShowDTO>? AnswerShowDTOs { get; set; }
    }
}
