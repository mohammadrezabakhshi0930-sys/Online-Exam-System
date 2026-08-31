using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CertificateHolder {  get; set; }
        public Guid ExamUserId { get; set; }
        public DateTime DateHolder { get; set; }= DateTime.Now;
        [ForeignKey(nameof(CertificateHolder))]
        public virtual ApplicationUser? User { get; set; }
        [ForeignKey(nameof(ExamUserId))]
        public virtual ExamUsers? ExamUsers { get; set; }
    }
}
