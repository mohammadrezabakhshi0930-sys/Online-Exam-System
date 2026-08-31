using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Core.DTO.CategoryDto;
using Application.Core.DTO.QuestionDto;
using Application.Infrastructrue.DbContext;
using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.ImplementationServices
{
    public class CategoryImplementation : ICategory
    {
        private readonly AppDbContext _Db;
        public CategoryImplementation(AppDbContext db)
        { 
        _Db = db;
        }

        public async Task<KeyValuePair<bool, string>> AddCategory(string NameCategory, Guid UserId)
        {
            if (string.IsNullOrWhiteSpace(NameCategory)) return new KeyValuePair<bool, string>(false,"لطفا نام دسته بندی را وارد کنید");
            string Name = NameCategory.Trim();
            try
            {
                if (await _Db.Category.AnyAsync(temp => temp.UserCreate == UserId && temp.Title == Name)) return new KeyValuePair<bool, string>(false, "نام دسته بندی تکراری است");
                Category Add = new Category()
                {
                    Title = Name,
                    UserCreate = UserId,
                    DateCreate = DateTime.Now,
                    Id = Guid.NewGuid(),
                };
                await _Db.Category.AddAsync(Add);
                await _Db.SaveChangesAsync();
                return new KeyValuePair<bool, string>(true,"موفق انجام شد");
            }
            catch(Exception e)
            {
                return new KeyValuePair<bool, string>(false, e.Message);
            }

        }

        public async Task<KeyValuePair<bool, string>> DeleteCategory(Guid Id, Guid UserId)
        {
            try
            {
                Category? Delete = await _Db.Category.Include(temp => temp.Questions).Include(temp => temp.ExamQuestionTypes).FirstOrDefaultAsync(temp => temp.Id == Id && temp.UserCreate == UserId);

                if (Delete == null) return new KeyValuePair<bool, string>(false, "این دسته بندی وجود ندارد");

                var now = DateTime.Now;

                var relatedExams = await _Db.ExamQuestionTypes
                    .Where(temp => temp.CategoryId == Delete.Id && temp.Exam != null)
                    .Select(temp => new {
                        temp.Exam!.Title,
                        temp.Exam!.StartExam,
                        temp.Exam!.EndExam,
                    })
                    .ToListAsync();

                string examTitles = "-----";

                if (relatedExams.Any())
                {
                    var ongoingExam = relatedExams.FirstOrDefault(e => now >= e.StartExam && now <= e.EndExam);
                    if (ongoingExam != null)
                    {
                        return new KeyValuePair<bool, string>(false,
                            $"امکان حذف وجود ندارد؛ این دسته‌بندی در امتحان در حال برگزاری '{ongoingExam.Title}' استفاده شده است.");
                    }


                    var futureExams = relatedExams.Where(e => now < e.StartExam || now > e.EndExam).ToList();
                    if (futureExams.Any())
                    {
                        examTitles = string.Join("، ", futureExams.Select(e => e.Title));

                        if (Delete.ExamQuestionTypes != null)
                        {
                            foreach (var eqt in Delete.ExamQuestionTypes)
                            {
                                eqt.CategoryId = null;
                            }
                        }
                    }
                }

                if (Delete.Questions != null)
                {
                    foreach (var eqt in Delete.Questions)
                    {
                        eqt.CategoryId = null;
                    }
                }

                _Db.Category.Remove(Delete);
                await _Db.SaveChangesAsync();
                return new KeyValuePair<bool, string>(true, $"دسته‌بندی با موفقیت حذف شد. توجه: سوالات این دسته بندی بدون دسته بندی شدند . امتحانات {examTitles} که از این دسته بندی استفاده کرده اند نیز سوالات خارج شد.");

            }
            catch(Exception e)
            {
                return new KeyValuePair<bool, string>(false, e.Message);

            }


        }

        public async Task<KeyValuePair<bool, string>> EditCategory(CategoryListDTO Edit, Guid UserId)
        {
            try
            {
                Category? category = await _Db.Category.FirstOrDefaultAsync(temp => temp.Id == Edit.Id && temp.UserCreate == UserId);
                if (category == null) return new KeyValuePair<bool, string>(false, "این دسته بندی وجود ندارد");
                if(await _Db.Category.AnyAsync(temp=>temp.Id != category.Id && temp.Title == Edit.Name)) return new KeyValuePair<bool, string>(false, "نام دسته بندی تکراری است");
                category.Title = Edit.Name;
                await _Db.SaveChangesAsync();
                return new KeyValuePair<bool, string>(true,"موفق انجام شد");
            }
            catch(Exception e)
            {
                return new KeyValuePair<bool, string>(false, e.Message);
            }


        }

        public async Task<List<CategoryShowDTO>> GetCategory(int Page, Guid UserId)
        {
            int Skip = (Page * 50) - 50;
            List<CategoryShowDTO> Result = await _Db.Category
                .Where(temp => temp.UserCreate == UserId)
                .OrderByDescending(temp => temp.DateCreate)
                .Skip(Skip)
                .Take(50)
                .Select(temp => new CategoryShowDTO()
                {
                    DateCreate = temp.DateCreate,
                    Id = temp.Id,
                    CategoryName = temp.Title,

                }).ToListAsync();
            return Result;
        }

        public async Task<int> GetCountCategory(Guid UserId)
        {
            int Result = await _Db.Category.CountAsync(temp => temp.UserCreate == UserId);
            return Result;
        }

        public async Task<List<CategoryListDTO>> GetList(Guid UserId)
        {
            List<CategoryListDTO> Result = await _Db.Category
                .Where(temp => temp.UserCreate == UserId)
                .OrderByDescending(temp => temp.DateCreate)
                .Select(temp => new CategoryListDTO()
                {
                    Id = temp.Id,
                    Name = temp.Title
                })
                .ToListAsync();
            return Result;
        }

        public async Task<CategoryQuestionsDTO?> GetQuestionCategory(Guid Id, Guid UserId)
        {
            CategoryQuestionsDTO? Result = await _Db.Category
                .Where(temp => temp.UserCreate == UserId && temp.Id == Id)
                .Select(temp => new CategoryQuestionsDTO()
                {
                    CategoryName = temp.Title,
                    Questions = temp.Questions.Select(a=>new QuestionShowDTO() 
                    {
                        DateCreate = a.DateCreate,
                        Id = a.Id,
                        IsActive = a.IsActive,
                        Point = a.Point,
                        TextQuestion = a.TextQuestion,
                        QuestionType = a.IsDescriptiveQuestion,
                    }).ToList(),

                }).FirstOrDefaultAsync();
            return Result;
        }
    }
}
