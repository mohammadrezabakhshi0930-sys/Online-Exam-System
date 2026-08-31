using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Application.Infrastructrue.Identity
{
    public class PersianIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError { Code = nameof(DuplicateUserName), Description = $"نام کاربری '{userName}' قبلاً انتخاب شده است." };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError { Code = nameof(DuplicateEmail), Description = $"ایمیل '{email}' قبلاً توسط شخص دیگری انتخاب شده است." };

        public override IdentityError InvalidEmail(string? email)
            => new IdentityError { Code = nameof(InvalidEmail), Description = $"ایمیل '{email}' معتبر نیست." };

        public override IdentityError PasswordMismatch()
            => new IdentityError { Code = nameof(PasswordMismatch), Description = "رمز عبور فعلی نادرست است." };

        public override IdentityError PasswordTooShort(int length)
            => new IdentityError { Code = nameof(PasswordTooShort), Description = $"رمز عبور باید حداقل {length} کاراکتر باشد." };

        public override IdentityError PasswordRequiresDigit()
            => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "رمز عبور باید حداقل شامل یک عدد باشد." };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "رمز عبور باید حداقل شامل یک حرف کوچک باشد." };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "رمز عبور باید حداقل شامل یک حرف بزرگ باشد." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "رمز عبور باید حداقل شامل یک کاراکتر خاص (مثل @ # $) باشد." };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"رمز عبور باید حداقل شامل {uniqueChars} کاراکتر متفاوت باشد." };

        public override IdentityError InvalidToken()
            => new IdentityError { Code = nameof(InvalidToken), Description = "توکن معتبر نیست." };

        public override IdentityError LoginAlreadyAssociated()
            => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "این کاربر قبلاً به سیستم وارد شده است." };
    }

}
