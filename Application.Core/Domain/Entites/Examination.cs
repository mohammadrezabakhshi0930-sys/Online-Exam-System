using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class Examination
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public DateTime StartExam {  get; set; }
        [Required]
        public DateTime EndExam { get; set; }
        [Required]
        public int TimeExam { get; set; }
        public string? Description { get; set; }
        public int? PassScore { get; set; }
        [Required]
        [Range(0,100)]
        public int MaxScore { get; set; }
        public bool RandomizeQuestion { get; set; } = false;
        public bool RandomizeAnswerOption { get; set; } = false;
        public bool ShowResultScore { get; set; } = false;
        public Guid UserCreate {  get; set; }
        public bool HasCertificate { get; set; } = false;
        public DateTime DateCreate { get; set; } = DateTime.Now;
        [ForeignKey(nameof(UserCreate))]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<ExamUsers>? ExamUsers { get; set; }
        public virtual ICollection<ExamQuestionTypes>? ExamQuestionTypes { get; set; }


    }
}
