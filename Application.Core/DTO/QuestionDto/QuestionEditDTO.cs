using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.QuestionDto
{
    public class QuestionEditDTO:QuestionCreateDTO
    {
        public Guid Id { get; set; }
    }
}
