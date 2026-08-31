using Application.Core.DTO.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Interface
{
    public interface IUser
    {
        Task<List<GetUserDTO>> GetAllUsers();
    }
}
