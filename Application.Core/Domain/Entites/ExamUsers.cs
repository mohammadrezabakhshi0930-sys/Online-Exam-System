using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public  class ExamUsers
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserExaminee { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateFinish { get; set; }
        public Guid ExamId { get; set; }
        public bool IsFinishedScore { get; set; }   
        public double? ScoreFinal { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Examination? Exam { get; set; }
        [ForeignKey(nameof(UserExaminee))]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<UserAnswer>? UserAnswers { get; set; }
        public virtual ICollection<Certificate>? Certificates { get; set; }



    }
}
