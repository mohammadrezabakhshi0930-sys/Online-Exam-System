using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class AnswerOption
    {
        [Key]
        public Guid Id { get; set; }     
        public Guid QuestionId { get; set; }
        [StringLength(200)]
        [Required]
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; } = false;
        [ForeignKey(nameof(QuestionId))]
        public virtual Question? Question { get; set; }



    }
}
