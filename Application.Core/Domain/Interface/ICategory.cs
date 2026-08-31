using Application.Core.Domain.Entites;
using Application.Core.DTO.CategoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Domain.Interface
{
    public interface ICategory
    {
        Task<List<CategoryListDTO>> GetList(Guid UserId);
        Task<List<CategoryShowDTO>> GetCategory(int Page,Guid UserId);
        Task<int> GetCountCategory(Guid UserId);
        Task<KeyValuePair<bool,string>> AddCategory(string NameCategory,Guid UserId);
        Task<KeyValuePair<bool, string>> EditCategory(CategoryListDTO Edit, Guid UserId);
        Task<KeyValuePair<bool, string>> DeleteCategory(Guid Id, Guid UserId);
        Task<CategoryQuestionsDTO?> GetQuestionCategory(Guid Id,Guid UserId);


    }
}
