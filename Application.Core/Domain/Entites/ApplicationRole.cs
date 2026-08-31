using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Application.Core.Domain.Entites
{
    public class ApplicationRole: IdentityRole<Guid>
    {
    }
}
