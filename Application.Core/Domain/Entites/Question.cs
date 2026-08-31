using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(500)] 
        public string? TextQuestion { get; set; }
        [Range(0, 100)]
        public int Point { get; set; }
        public bool IsActive { get; set; }= true;
        public Guid? CategoryId {  get; set; }
        public Guid? UserCreate {  get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public bool IsDescriptiveQuestion { get; set; }
        [StringLength(500)]
        public string? CorrectAnswer { get; set; }

        [ForeignKey(nameof(UserCreate))]
        public virtual ApplicationUser? User { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        public virtual ICollection<AnswerOption>? AnswerOptions { get; set; }
        public virtual ICollection<UserAnswer>? UserAnswers { get; set; }
        public virtual ICollection<ExamQuestionTypes>? ExamQuestionTypes { get; set; }




    }
}
