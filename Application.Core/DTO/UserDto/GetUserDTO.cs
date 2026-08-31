using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.UserDto
{
    public class GetUserDTO
    {
        public Guid Id{ get; set; }
        public string? Name{ get; set; }
        public DateTime DateCreate { get; set; }
        public List<string?>? Role {  get; set; }
        public string? UserName { get; set; }
    }
}
