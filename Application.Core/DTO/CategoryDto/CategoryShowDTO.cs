using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.CategoryDto
{
    public class CategoryShowDTO
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
