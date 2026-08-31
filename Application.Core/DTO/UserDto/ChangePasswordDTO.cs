using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.UserDto
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "رمز عبور فعلی الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور فعلی")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "رمز عبور جدید الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_@\-!#$%^&*])[A-Za-z\d\s_@\-!#$%^&*]{8,}$",
            ErrorMessage = "رمز عبور باید حداقل 8 کاراکتر و شامل حروف بزرگ، کوچک، عدد و کاراکتر خاص باشد")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید و تکرار آن مطابقت ندارند")]
        public string? ConfirmPassword { get; set; }
    }
}
