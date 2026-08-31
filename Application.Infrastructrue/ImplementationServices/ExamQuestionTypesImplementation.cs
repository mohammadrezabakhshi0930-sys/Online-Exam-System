using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Core.DTO.ExamQuestionTypeDto;
using Application.Infrastructrue.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.ImplementationServices
{
    public class ExamQuestionTypesImplementation : IExamQuestionTypes
    {
        private readonly AppDbContext _Db;
        public ExamQuestionTypesImplementation(AppDbContext db)
        {
            _Db = db;
        }
        public async Task<KeyValuePair<bool, string>> AddTypes(QuestinsTypesEXDTO Add, Guid Id)
        {
            try
            {
                if (Add.QuestionSelect != null)
                {
                    Add.QuestionSelect = Add.QuestionSelect
                        .Where(q => q != null)
                        .ToList();
                }

                if (Add.CategorySelect != null)
                {
                    Add.CategorySelect = Add.CategorySelect
                        .Where(c => c != null)
                        .ToList();
                }

                var exam = await _Db.Examinations
                    .FirstOrDefaultAsync(e => e.Id == Add.IdExam && e.UserCreate == Id);

                if (exam == null)
                    return new KeyValuePair<bool, string>(false, "آزمون یافت نشد.");

                if (exam.StartExam <= DateTime.Now)
                    return new KeyValuePair<bool, string>(false, "آزمون شروع شده است؛ تغییر سوالات امکان‌پذیر نیست.");

                int warnings = 0;
                int addedCount = 0;

                var existingItems = await _Db.ExamQuestionTypes 
                    .Where(x => x.ExamId == Add.IdExam)
                    .ToListAsync();

                if (Add.QuestionSelect != null)
                {
                    List<Guid> IdQuestion = await _Db.Question.Where(temp => temp.UserCreate == Id && temp.IsActive).Select(temp => temp.Id).ToListAsync();

                    foreach (var qSelect in Add.QuestionSelect)
                    {
                        if (existingItems.Any(x => x.QuestionId == qSelect.IdQuestion)) 
                        {
                            warnings++; 
                            continue;
                        }

                        if (!IdQuestion.Contains(qSelect.IdQuestion))
                        {
                            warnings++;
                            continue;
                        }

                        _Db.ExamQuestionTypes.Add(new ExamQuestionTypes
                        {
                            ExamId = Add.IdExam,
                            QuestionId = qSelect.IdQuestion,
                            CategoryId = null, 
                            Count = null,
                            Id =  Guid.NewGuid(),
                        });
                        addedCount++;
                    }
                }

                if (Add.CategorySelect != null)
                {
                    Dictionary<Guid,int> IdCategoryAndCount  =await _Db.Category
                    .Where(temp=>temp.UserCreate == Id)
                    .Select(temp => new
                    {   CategoryId = temp.Id,
                        QuestionCount = temp.Questions.Count() 
                    }).ToDictionaryAsync(
                        x => x.CategoryId,
                        x => x.QuestionCount
                    );
                    foreach (var catSelect in Add.CategorySelect)
                    {
                        if (existingItems.Any(x => x.CategoryId == catSelect.IdCategory))
                        {
                            warnings++;
                            continue;
                        }

                        if (!IdCategoryAndCount.Keys.Contains(catSelect.IdCategory))
                        {
                            warnings++;
                            continue;
                        }

                        if (IdCategoryAndCount[catSelect.IdCategory] < catSelect.Count)
                        {
                            warnings++;
                            continue;
                        }

                        _Db.ExamQuestionTypes.Add(new ExamQuestionTypes
                        {
                            ExamId = Add.IdExam,
                            QuestionId = null,
                            CategoryId = catSelect.IdCategory,
                            Count = catSelect.Count,
                            Id = Guid.NewGuid(),
                        });
                        addedCount++;
                    }
                }

                if (addedCount > 0)
                {
                    await _Db.SaveChangesAsync();
                }

                string finalMessage = addedCount > 0 ? $"{addedCount} مورد جدید اضافه شد." : "مورد جدیدی برای اضافه کردن وجود نداشت.";
                if (warnings > 0)
                {
                    finalMessage += " (برخی موارد به دلیل تکراری بودن نادیده گرفته شدند)";
                }

                return new KeyValuePair<bool, string>(true, finalMessage);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }
        }

        public async Task<KeyValuePair<bool, string>> Delete(Guid Id)
        {
            try
            {
                ExamQuestionTypes? Delete = await _Db.ExamQuestionTypes.Include(temp=>temp.Exam).FirstOrDefaultAsync(temp => temp.Id == Id);
                if (Delete == null) return new KeyValuePair<bool, string>(false,"نوع سوال در امتحان یافت نشد");
                if (Delete.Exam != null && Delete.Exam.StartExam < DateTime.Now) return new KeyValuePair<bool, string>(false,"امکان تغییر نوع سوالات بعد از شروع وجود ندارد");
                _Db.ExamQuestionTypes.Remove(Delete);
                await _Db.SaveChangesAsync();
                return new KeyValuePair<bool, string>(true,"موفق انجام شد");
            }
            catch (Exception e)
            {
                return new KeyValuePair<bool, string>(false,e.Message);
            }
        }

        public async Task<List<ExamQTypesShowDTO>> GetListQuestionType(Guid IdExam)
        {
            List<ExamQTypesShowDTO> result = await _Db.ExamQuestionTypes
                .Where(temp => temp.ExamId == IdExam
                && (temp.CategoryId != null || (temp.QuestionId != null && temp.Question.IsActive))
                ).Select(temp => new ExamQTypesShowDTO()
                {
                    Count = temp.Count,
                    Id = temp.Id,
                    MaxScore = temp.Question.Point,
                    NameCategory = temp.Category.Title,
                    TextQuestion = temp.Question.TextQuestion,
                }).ToListAsync();
            return result;
        }
    }
}
