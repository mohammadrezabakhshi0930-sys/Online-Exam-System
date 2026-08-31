using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Core.DTO.AnswerDto;
using Application.Core.DTO.QuestionDto;
using Application.Infrastructrue.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.ImplementationServices
{
    public class QuestionImplementation : IQuestion
    {
        private readonly AppDbContext _Db;
        public QuestionImplementation(AppDbContext db)
        {
            _Db = db;
        }
        public async Task<KeyValuePair<bool, string>> AddQuestion(QuestionCreateDTO Add, Guid UserId)
        {
            if(!Add.IsDescriptiveQuestion && (Add.Answer == null || Add.Answer.Count < 2))return new KeyValuePair<bool, string>(false, "سوالات تستی باید گزینه های پاسخ داشته باشند");
           
            if(!Add.IsDescriptiveQuestion && string.IsNullOrEmpty(Add.AnswerIsTrue)) return new KeyValuePair<bool, string>(false, "سوالات تستی باید حتما گزینه صحیح داشته باشند");
            
            try
            {
               
                    if(Add.CategoryId != null)
                    {
                        if (!await _Db.Category.AnyAsync(temp => temp.Id == Add.CategoryId && temp.UserCreate == UserId)) return new KeyValuePair<bool, string>(false, "ایدی دسته بندی انتخاب شده اشتباه است");
                    }

                    Question question = new Question()
                    {
                        CategoryId = Add.CategoryId,
                        IsActive = Add.IsActive,
                        IsDescriptiveQuestion = Add.IsDescriptiveQuestion,
                        Point = Add.Point,
                        TextQuestion = Add.TextQuestion,
                        UserCreate = UserId,
                        CorrectAnswer = (Add.IsDescriptiveQuestion) ? Add.CorrectAnswer : null,
                        Id = Guid.NewGuid(),
                        DateCreate = DateTime.Now,
                    };
                   
                    if (!Add.IsDescriptiveQuestion)
                    {
                        List<AnswerOption> answerOptions = new List<AnswerOption>();

                        foreach (string temp in Add.Answer!)
                        {
                            answerOptions.Add(new AnswerOption()
                            {
                                AnswerText = temp,
                                Id = Guid.NewGuid(),
                                IsCorrect = (temp == Add.AnswerIsTrue),
                                Question = question,
                            });
                        }

                       await _Db.AnswerOptions.AddRangeAsync(answerOptions);
                    }

                    await _Db.Question.AddAsync(question);
                    await _Db.SaveChangesAsync();

                    return new KeyValuePair<bool, string>(true, "انجام شد");
                
            }
            catch
            {
                return new KeyValuePair<bool, string>(false, "مشکلی در ثبت در دیتابیس رخ داده است");
            }
        }

        public async Task<KeyValuePair<bool, string>> EditCategoryQuestion(Guid Id, Guid UserId)
        {
            try
            {
                Question? edit = await _Db.Question.FirstOrDefaultAsync(temp => temp.Id == Id && UserId == temp.UserCreate);
                if (edit == null) return new KeyValuePair<bool, string>(false, "سوال مد نظر یافت نشد");
                if (edit.CategoryId == null) return new KeyValuePair<bool, string>(true, "انجام شد");
                if (await _Db.ExamQuestionTypes
                    .AnyAsync(temp => temp.CategoryId == edit.CategoryId
                    && temp.Exam != null
                    && temp.Exam.StartExam < DateTime.Now
                    && temp.Exam.EndExam > DateTime.Now)) return new KeyValuePair<bool, string>(false, "از این دسته بندی در حال برگزاری امتحان می باشد امکان تغییر فعلا ندارید");
                ExamQuestionTypes? Types = await _Db.ExamQuestionTypes
                    .Include(temp=>temp.Exam)
                    .Where(temp => temp.CategoryId == edit.CategoryId && temp.Exam != null && temp.Exam.EndExam > DateTime.Now)
                    .OrderByDescending(temp => temp.Count)
                    .FirstOrDefaultAsync();
                if(Types != null && Types.Exam != null)
                {
                    
                        int Count = await _Db.Question.CountAsync(temp => temp.CategoryId == edit.CategoryId);
                    if (Count == Types.Count) return new KeyValuePair<bool, string>(false, $"با حذف این سوال از این دسته بندی تعداد سوالات تعیین شده در امتحان  {Types.Exam.Title} کمتر از میزان تعیین شده میشود لطفا ابتدا سوالات امتحان را ویرایش نمایید");
                }
                edit.CategoryId = null;
                await _Db.SaveChangesAsync();
                return new KeyValuePair<bool, string>(true, "موفق انجام شد");
            }
            catch(Exception e)
            {
                return new KeyValuePair<bool, string>(false, e.Message);
            }
           
        }

        public async Task<KeyValuePair<bool, string>> EditQuestion(QuestionEditDTO Edit, Guid UserId)
        {
            if (!Edit.IsDescriptiveQuestion && (Edit.Answer == null || Edit.Answer.Count < 2)) return new KeyValuePair<bool, string>(false, "سوالات تستی باید گزینه های پاسخ داشته باشند");

            if (!Edit.IsDescriptiveQuestion && string.IsNullOrEmpty(Edit.AnswerIsTrue)) return new KeyValuePair<bool, string>(false, "سوالات تستی باید حتما گزینه صحیح داشته باشند");

            try
            {
                if (Edit.CategoryId != null)
                {
                    if (!await _Db.Category.AnyAsync(temp => temp.Id == Edit.CategoryId && temp.UserCreate == UserId)) return new KeyValuePair<bool, string>(false, "ایدی دسته بندی انتخاب شده اشتباه است");
                }

                Question? Find = await _Db.Question.FirstOrDefaultAsync(temp => temp.Id == Edit.Id && temp.UserCreate == UserId);

                if (Find == null) return new KeyValuePair<bool, string>(false, "سوال مد نظر یافت نشد");

                DateTime Now = DateTime.Now;

                bool isQuestionInActiveExam = await _Db.ExamQuestionTypes
                    .Include(temp => temp.Exam)
                    .AnyAsync(temp => (temp.CategoryId == Find.CategoryId || temp.QuestionId == Find.Id)
                                      && temp.Exam != null
                                      && temp.Exam.StartExam < Now
                                      && temp.Exam.EndExam > Now);

                if (isQuestionInActiveExam)
                    return new KeyValuePair<bool, string>(false, "در هنگام برگزاری امتحان، امکان ویرایش این سوال وجود ندارد");

                if(Find.CategoryId != Edit.CategoryId)
                {
                    ExamQuestionTypes? Types = await _Db.ExamQuestionTypes
                    .Include(temp => temp.Exam)
                    .Where(temp => temp.CategoryId == Find.CategoryId && temp.Exam != null && temp.Exam.EndExam > DateTime.Now)
                    .OrderByDescending(temp => temp.Count)
                    .FirstOrDefaultAsync();
                    if (Types != null && Types.Exam != null)
                    {

                        int Count = await _Db.Question.CountAsync(temp => temp.CategoryId == Find.CategoryId);
                        if (Count == Types.Count) return new KeyValuePair<bool, string>(false, $"با حذف این سوال از این دسته بندی تعداد سوالات تعیین شده در امتحان  {Types.Exam.Title} کمتر از میزان تعیین شده میشود لطفا ابتدا سوالات امتحان را ویرایش نمایید");
                    }
                }

                Find.CategoryId = Edit.CategoryId;
                Find.IsActive = Edit.IsActive;
                Find.Point = Edit.Point;
                Find.TextQuestion = Edit.TextQuestion;
                Find.CorrectAnswer = (Edit.IsDescriptiveQuestion) ? Edit.CorrectAnswer : null;
                if (!Find.IsDescriptiveQuestion && Edit.IsDescriptiveQuestion)
                {
                    if (!await DeleteAnsweOption(Find.Id)) return new KeyValuePair<bool, string>(false, "مشکلی در تغییر در داده رخ داده است");
                }
                Find.IsDescriptiveQuestion = Edit.IsDescriptiveQuestion;


                if (!Edit.IsDescriptiveQuestion)
                {
                    if (!await DeleteAnsweOption(Find.Id)) return new KeyValuePair<bool, string>(false, "مشکلی در تغییر در داده رخ داده است");

                    List<AnswerOption> answerOptions = new List<AnswerOption>();

                    foreach (string temp in Edit.Answer!)
                    {
                        answerOptions.Add(new AnswerOption()
                        {
                            AnswerText = temp,
                            Id = Guid.NewGuid(),
                            IsCorrect = (temp == Edit.AnswerIsTrue),
                            QuestionId = Find.Id,
                        });
                    }

                    await _Db.AnswerOptions.AddRangeAsync(answerOptions);
                }

                await _Db.SaveChangesAsync();

                return new KeyValuePair<bool, string>(true, "انجام شد");

            }
            catch
            {
                return new KeyValuePair<bool, string>(false, "مشکلی در ثبت در دیتابیس رخ داده است");
            }
        }

        public async Task<int> GetCountQuestion(Guid UserId)
        {
           int Result = await _Db.Question.CountAsync(temp=>temp.UserCreate ==  UserId);
            return Result;
        }

        public async Task<QuestionDetailsDTO?> GetDetailsQuestion(Guid QuestionId)
        {
            QuestionDetailsDTO? Details = await _Db.Question
                .Where(temp => temp.Id == QuestionId)
                .Select(temp => new QuestionDetailsDTO()
                {
                    CategoryName =temp.Category == null?null:temp.Category.Title,
                    CorrectAnswer = temp.CorrectAnswer,
                    DateCreate = temp.DateCreate,
                    IsActive = temp.IsActive,
                    Point = temp.Point,
                    TextQuestion = temp.TextQuestion,
                    IsDescriptiveQuestion = temp.IsDescriptiveQuestion,
                    AnswerShowDTOs = temp.AnswerOptions == null? null
                    : temp.AnswerOptions.Select(w => new AnswerShowDTO()
                    {
                        IsTrue = w.IsCorrect,
                        Text = w.AnswerText,
                    }).ToList()
                }).FirstOrDefaultAsync();
            return Details;
        }

        public async Task<List<QuestinListDTO>> GetList(Guid Id)
        {
            List<QuestinListDTO> result = await _Db.Question
                .Where(temp => temp.IsActive && temp.UserCreate == Id)
                .Select(temp => new QuestinListDTO()
                {
                    TextQuestion = temp.TextQuestion,
                    Id = temp.Id,
                }).ToListAsync();
            return result;
        } 

        public async Task<List<QuestionShowDTO>> GetQuestion(int Page, Guid UserId)
        {
            int Skip = (Page * 50) - 50;
            List<QuestionShowDTO> Result = await _Db.Question
                .Where(temp => temp.UserCreate == UserId)
                .OrderByDescending(temp => temp.DateCreate)
                .Skip(Skip)
                .Take(50)
                .Select(temp => new QuestionShowDTO()
                {
                    DateCreate = temp.DateCreate,
                    Id = temp.Id,
                    IsActive = temp.IsActive,
                    Point = temp.Point,
                    TextQuestion = temp.TextQuestion,
                    QuestionType = temp.IsDescriptiveQuestion,
                }).ToListAsync();
            return Result;

        }

        public async Task<QuestionEditDTO?> GetSingleQuestion(Guid QuestionId, Guid UserId)
        {
            QuestionEditDTO? questionEditDTO = await _Db.Question
                 .Where(temp => temp.Id == QuestionId && temp.UserCreate == UserId)
                 .Select(temp => new QuestionEditDTO()
                 {
                     Id = temp.Id,
                     CategoryId = temp.CategoryId,
                     CorrectAnswer = temp.CorrectAnswer,
                     IsDescriptiveQuestion = temp.IsDescriptiveQuestion,
                     Point = temp.Point,
                     IsActive = temp.IsActive,
                     TextQuestion = temp.TextQuestion,
                     Answer = temp.AnswerOptions == null ? null
                    : temp.AnswerOptions.Select(w => w.AnswerText!).ToList(),
                     AnswerIsTrue = temp.AnswerOptions == null ? null
                    : temp.AnswerOptions.Where(w => w.IsCorrect).Select(w => w.AnswerText).FirstOrDefault()
                 }).FirstOrDefaultAsync();
            return questionEditDTO;
        }
       
        private async Task<bool> DeleteAnsweOption(Guid Id)
        {
            try
            {
                List<AnswerOption> Delete = await _Db.AnswerOptions.Where(temp => temp.QuestionId == Id).ToListAsync();
                if(Delete.Count < 0) return true;
                _Db.AnswerOptions.RemoveRange(Delete);
                await _Db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }           
        }
    }
}
