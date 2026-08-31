using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class ExamQuestionTypes
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Count { get; set; }
        public Guid ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Examination? Exam {  get; set; }
        [ForeignKey(nameof(QuestionId))]
        public virtual Question? Question { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

    }
}
