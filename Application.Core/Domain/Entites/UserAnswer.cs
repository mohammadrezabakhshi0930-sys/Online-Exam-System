using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class UserAnswer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(500)]
        public string? QuestionText { get; set; }
        [Required]
        public string? AnswerText { get; set; }
        public string? UserAnswerText { get; set; }
        public Guid QuestionId { get; set; }
        public Guid ExamUserId { get; set; }
        public double? ObtainedScore { get; set; }
        public int MaxScore { get; set; }
        public string? AiFeedback {  get; set; }
        public bool? CorrectionStatus { get; set; }


        [ForeignKey(nameof(QuestionId))]
        public virtual Question? Question { get; set; }

       [ForeignKey(nameof(ExamUserId))]    
       public virtual ExamUsers? ExamUser { get; set; }
    }
}
