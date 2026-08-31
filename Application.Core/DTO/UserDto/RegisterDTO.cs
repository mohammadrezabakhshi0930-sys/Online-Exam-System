using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.UserDto
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "نام کاربری اجباری است")]
        [RegularExpression(@"^[a-zA-Z0-9\s_@\-]*$", ErrorMessage = "کاراکترهای مجاز: حروف، اعداد، فاصله و (_ @ -)")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "نام و نام خانوادگی اجباری است")]
        [RegularExpression(@"^[^<>;'""&]*$", ErrorMessage = "استفاده از کاراکترهای خاص مجاز نیست")]
        public string? Name { get; set; }
        [Required(ErrorMessage = " رمز عبور اجباری است")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_@\-!#$%^&*])[A-Za-z\d\s_@\-!#$%^&*]{8,}$",
ErrorMessage = "رمز عبور باید حداقل 8 کاراکتر و شامل حروف بزرگ، کوچک، عدد و حداقل یک کاراکتر خاص (_ @ - ! # $ % ^ & *) باشد")]

        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        public string? Password { get; set; }
    }
}
