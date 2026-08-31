using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Entites
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public bool IsRegisterAdmin { get; set; }
        public  DateTime RegistrationDate { get; set; }
        public bool IsLogout {  get; set; }

        public virtual ICollection<Certificate>? Certificates { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Examination>? Examinations { get; set; }
        public virtual ICollection<ExamUsers>? ExamUsers { get; set; }




    }
}
