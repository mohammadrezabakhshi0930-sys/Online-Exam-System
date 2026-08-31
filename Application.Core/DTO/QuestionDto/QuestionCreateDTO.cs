using Application.Core.Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.QuestionDto
{
    public class QuestionCreateDTO
    {

        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        public string? TextQuestion { get; set; }

        [Display(Name = "بارم")]
        [Range(0, 100, ErrorMessage = "{0} باید عددی بین {1} و {2} باشد")]
        public int Point { get; set; }

        [Display(Name = "وضعیت فعال بودن")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "دسته‌بندی")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "سوال تشریحی")]
        public bool IsDescriptiveQuestion { get; set; } = false;
        [Display(Name = "جواب سوال")]
        public string? CorrectAnswer { get; set; }
        [Display(Name = "گزینه های تستی")]
        public List<string>? Answer { get; set; }
        public string? AnswerIsTrue { get; set; }

    }
}
