using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.UserDto
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="نام کاربری اجباری است")]
        [RegularExpression(@"^[a-zA-Z0-9\s_@\-]*$", ErrorMessage = "کاراکترهای مجاز: حروف، اعداد، فاصله و (_ @ -)")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "رمز عبور اجباری است")]
        [RegularExpression(@"^[a-zA-Z0-9\s_@\-]*$", ErrorMessage = "کاراکترهای مجاز: حروف، اعداد، فاصله و (_ @ -)")]
        public string? Password { get; set; }
        public bool IsRemember { get; set; } = false;
    }
}
