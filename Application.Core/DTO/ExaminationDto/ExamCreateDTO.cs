using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class ExamCreateDTO : IValidatableObject
    {
        [Required(ErrorMessage = "وارد کردن عنوان آزمون الزامی است.")]
        [StringLength(150, ErrorMessage = "عنوان آزمون نمی‌تواند بیشتر از ۱۵۰ کاراکتر باشد.")]
        [Display(Name = "عنوان آزمون")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "وارد کردن تاریخ شروع الزامی است.")]
        [Display(Name = "تاریخ شروع")]
        public DateTime StartExam { get; set; }

        [Required(ErrorMessage = "وارد کردن تاریخ پایان الزامی است.")]
        [Display(Name = "تاریخ پایان")]
        public DateTime EndExam { get; set; }

        [Required(ErrorMessage = "وارد کردن مدت زمان آزمون الزامی است.")]
        [Range(1, 1000, ErrorMessage = "مدت زمان آزمون باید بین ۱ تا ۱۰۰۰ دقیقه باشد.")]
        [Display(Name = "مدت زمان (دقیقه)")]
        public int TimeExam { get; set; }

        [StringLength(1000, ErrorMessage = "توضیحات نمی‌تواند بیشتر از ۱۰۰۰ کاراکتر باشد.")]
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Range(0, 100, ErrorMessage = "نمره قبولی باید یک عدد مثبت باشد.")]
        [Display(Name = "نمره قبولی")]
        public int? PassScore { get; set; }

        [Required(ErrorMessage = "وارد کردن بارم کل آزمون الزامی است.")]
        [Range(1, 100, ErrorMessage = "بارم کل آزمون باید حداقل ۱ باشد.")]
        [Display(Name = "بارم کل")]
        public int MaxScore { get; set; }

        [Display(Name = "چینش تصادفی سوالات")]
        public bool RandomizeQuestion { get; set; } = false;

        [Display(Name = "چینش تصادفی گزینه‌ها")]
        public bool RandomizeAnswerOption { get; set; } = false;

        [Display(Name = "نمایش نمره به کاربر")]
        public bool ShowResultScore { get; set; } = false;

        [Display(Name = "دارای گواهی‌نامه")]
        public bool HasCertificate { get; set; } = false;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasCertificate && !PassScore.HasValue)
            {
                yield return new ValidationResult(
                    "در صورت فعال بودن صدور گواهینامه، وارد کردن نمره قبولی الزامی است.",
                    new[] { nameof(PassScore) }
                );
            }

            if (EndExam <= StartExam)
            {
                yield return new ValidationResult(
                    "تاریخ پایان آزمون باید بعد از تاریخ شروع باشد.",
                    new[] { nameof(EndExam) }
                );
            }


            if (StartExam < DateTime.Now.AddMinutes(-5))
            {
                yield return new ValidationResult(
                    "تاریخ شروع آزمون نمی‌تواند در گذشته باشد.",
                    new[] { nameof(StartExam) }
                );
            }

            if (PassScore.HasValue && PassScore.Value > MaxScore)
            {
                yield return new ValidationResult(
                    "نمره قبولی نمی‌تواند بیشتر از نمره کل آزمون باشد.",
                    new[] { nameof(PassScore) }
                );
            }
        }
    }

}
