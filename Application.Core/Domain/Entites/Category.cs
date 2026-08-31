using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength (100)]
        [Required]
        public string? Title { get; set; }
        public Guid UserCreate {  get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        
        [ForeignKey(nameof(UserCreate))]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Question>? Questions { get; set; }
        public virtual ICollection<ExamQuestionTypes>? ExamQuestionTypes { get; set; }


    }
}
