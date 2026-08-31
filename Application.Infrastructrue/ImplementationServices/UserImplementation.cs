using Application.Core.Domain.Interface;
using Application.Core.DTO.UserDto;
using Application.Infrastructrue.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.ImplementationServices
{
    public class UserImplementation : IUser
    {
        private readonly AppDbContext _Db;
        public UserImplementation(AppDbContext db)
        {
            _Db = db;
        }
        public async Task<List<GetUserDTO>> GetAllUsers()
        {
            List<GetUserDTO> Result = await _Db.Users
                .Select(temp => new GetUserDTO
                {
                    DateCreate = temp.RegistrationDate,
                    Id = temp.Id,
                    Name = temp.Name,
                    UserName = temp.UserName,
                    Role = _Db.UserRoles
               .Where(ur => ur.UserId == temp.Id)
               .Join(_Db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
               .ToList(),
                }).ToListAsync();
            return Result;
        }
    }
}
