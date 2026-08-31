using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Core.DTO.ExaminationDto;
using Application.Core.DTO.ExamQuestionTypeDto;
using Application.Core.DTO.QuestionDto;
using Application.Core.Enums;
using Application.Infrastructrue.DbContext;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.ImplementationServices
{
    public class ExaminationImplementation : IExamination
    {
        private readonly AppDbContext _Db;
        public ExaminationImplementation(AppDbContext db)
        {
            _Db = db;
        }

        private UserAnswer CreateAnswerRow(Question q, Guid examUserId)
        {
            return new UserAnswer
            {
                Id = Guid.NewGuid(),
                ExamUserId = examUserId,
                QuestionId = q.Id,
                QuestionText = q.TextQuestion,
                AnswerText = (q.IsDescriptiveQuestion) ? q.CorrectAnswer : q.AnswerOptions!.Where(temp => temp.IsCorrect).Select(temp => temp.AnswerText).FirstOrDefault(),
                MaxScore = q.Point,
                UserAnswerText = null,
                ObtainedScore = 0
            };
        }

        public async Task<KeyValuePair<bool, string>> AddExam(ExamCreateDTO Add, Guid UserId)
        {
            try
            {
                string Title = Add.Title!.Trim();

                if (await _Db.Examinations.AnyAsync(temp => temp.Title == Title)) return new KeyValuePair<bool, string>(false, "عنوان امتحان تکراری  می باشد");
                Examination Exam = new Examination()
                {
                    RandomizeAnswerOption = Add.RandomizeAnswerOption,
                    Title = Title,
                    DateCreate = DateTime.Now,
                    Description = Add.Description,
                    EndExam = Add.EndExam,
                    HasCertificate = Add.HasCertificate,
                    Id = Guid.NewGuid(),
                    MaxScore = Add.MaxScore,
                    ShowResultScore = Add.ShowResultScore,
                    TimeExam = Add.TimeExam,
                    PassScore = Add.PassScore,
                    RandomizeQuestion = Add.RandomizeQuestion,
                    StartExam = Add.StartExam,
                    UserCreate = UserId,
                };

                await _Db.Examinations.AddAsync(Exam);
                await _Db.SaveChangesAsync();

                return new KeyValuePair<bool, string>(true, "انجام شد");

            }
            catch
            {
                return new KeyValuePair<bool, string>(false, "مشکلی در ثبت در دیتابیس رخ داده است");
            }
        }

        public async Task<KeyValuePair<bool, string>> EditExam(ExamEditDTO Edit, Guid UserId)
        {
            try
            {
                string Title = Edit.Title!.Trim();
                Examination? examination = await _Db.Examinations.FirstOrDefaultAsync(temp => temp.Id == Edit.Id);
                if (examination == null) return new KeyValuePair<bool, string>(false, "امتحان انتخاب شده نامعتبر می باشد");
                if (examination.StartExam < DateTime.Now) return new KeyValuePair<bool, string>(false, "امتحان بعداز تایم شروع دیگر قابل تغییر نیست");
                if (await _Db.Examinations.AllAsync(temp => temp.Id != examination.Id && temp.Title == Title)) return new KeyValuePair<bool, string>(false, "عنوان امتحان تکراری  می باشد");

                Edit.RandomizeAnswerOption = examination.RandomizeAnswerOption;
                Edit.Title = Title;
                Edit.Description = examination.Description;
                Edit.EndExam = examination.EndExam;
                Edit.HasCertificate = examination.HasCertificate;
                Edit.MaxScore = examination.MaxScore;
                Edit.ShowResultScore = examination.ShowResultScore;
                Edit.TimeExam = examination.TimeExam;
                Edit.PassScore = examination.PassScore;
                Edit.RandomizeQuestion = examination.RandomizeQuestion;
                Edit.StartExam = examination.StartExam;


                await _Db.SaveChangesAsync();

                return new KeyValuePair<bool, string>(true, "انجام شد");

            }
            catch
            {
                return new KeyValuePair<bool, string>(false, "مشکلی در ثبت در دیتابیس رخ داده است");
            }
        }

        public async Task<int> GetCountExam(Guid UserId)
        {
            int Result = await _Db.Examinations.CountAsync(temp => temp.UserCreate == UserId);
            return Result;
        }

        public async Task<ExamDetailsDTO?> GetDetailsExam(Guid ExamId)
        {
            ExamDetailsDTO? Result = await _Db.Examinations
                .Where(temp => temp.Id == ExamId)
                .Select(temp => new ExamDetailsDTO()
                {
                    Id = temp.Id,
                    Title = temp.Title,
                    StartExam = temp.StartExam,
                    EndExam = temp.EndExam,
                    Description = temp.Description,
                    HasCertificate = temp.HasCertificate,
                    MaxScore = temp.MaxScore,
                    PassScore = temp.PassScore,
                    RandomizeAnswerOption = temp.RandomizeAnswerOption,
                    RandomizeQuestion = temp.RandomizeQuestion,
                    ShowResultScore = temp.ShowResultScore,
                    TimeExam = temp.TimeExam,
                    DateCreate = temp.DateCreate,
                    BeforeExam = (temp.StartExam > DateTime.Now) ?
                    temp.ExamQuestionTypes
                    .Select(a => new ExamQTypesShowDTO()
                    {
                        Count = a.Count,
                        Id = a.Id,
                        MaxScore = a.Question.Point,
                        NameCategory = a.Category.Title,
                        TextAnswer = a.Question.AnswerOptions.Where(b => b.IsCorrect).Select(b => b.AnswerText).FirstOrDefault(),
                        TextQuestion = a.Question.TextQuestion,
                    }).ToList() : null,

                }).FirstOrDefaultAsync();

            if (Result != null && Result.StartExam < DateTime.Now)
            {
                Result.AfterExam = await _Db.UserAnswer
                    .Where(temp => temp.ExamUser != null
                     && temp.ExamUser.ExamId == Result.Id)
                    .GroupBy(temp => temp.QuestionId)
                    .Select(group => new SumrizeQ()
                    {
                        MaxScore = group.First().MaxScore,
                        TextQuestion = group.First().QuestionText,
                        TextAnswer = group.First().AnswerText
                    }).ToListAsync();

            }

            return Result;
        }

        public async Task<DetailsStartExam?> GetDetailsStartExam(Guid IdExam, Guid IdUser)
        {
            DetailsStartExam? Result = await _Db.Examinations
                .Where(temp => temp.Id == IdExam && temp.ExamQuestionTypes != null).
                Select(temp => new DetailsStartExam()
                {
                    AlreadyParticipated = ((temp.ExamUsers == null) || (temp.ExamUsers.Any(temp => temp.UserExaminee == IdUser))),
                    Description = temp.Description,
                    DurationMinutes = temp.TimeExam,
                    EndExam = temp.EndExam,
                    ExamId = temp.Id,
                    PassingScore = temp.PassScore ?? 0,
                    StartExam = temp.StartExam,
                    Title = temp.Title,
                    TotalQuestions = temp.ExamQuestionTypes!.Sum(eq => eq.QuestionId != null ? 1 : (eq.Count ?? 0)),
                    TotalScore = temp.MaxScore

                }).FirstOrDefaultAsync();
            return Result;

        }

        public async Task<List<ExamShowDTO>> GetExam(int Page, Guid UserId)
        {
            int Skip = (Page * 50) - 50;
            List<ExamShowDTO> Result = await _Db.Examinations
                .Where(temp => temp.UserCreate == UserId)
                .OrderByDescending(temp => temp.DateCreate)
                .Skip(Skip)
                .Take(50)
                .Select(temp => new ExamShowDTO()
                {
                    DateCreate = temp.DateCreate,
                    Id = temp.Id,
                    HaveQuestion = temp.ExamQuestionTypes.Any(a => a.CategoryId != null || a.QuestionId != null),
                    MaxScore = temp.MaxScore,
                    Status = (temp.EndExam.AddMinutes(temp.TimeExam) < DateTime.Now) ? 2 : (temp.StartExam > DateTime.Now) ? 0 : 1,
                    Title = temp.Title,
                }).ToListAsync();
            return Result;
        }

        public async Task<ExamEditDTO?> GetSingleExam(Guid ExamId, Guid UserId)
        {
            ExamEditDTO? ExamEditDTO = await _Db.Examinations
                             .Where(temp => temp.Id == ExamId && temp.UserCreate == UserId)
                             .Select(temp => new ExamEditDTO()
                             {
                                 Id = temp.Id,
                                 Title = temp.Title,
                                 StartExam = temp.StartExam,
                                 EndExam = temp.EndExam,
                                 Description = temp.Description,
                                 HasCertificate = temp.HasCertificate,
                                 MaxScore = temp.MaxScore,
                                 PassScore = temp.PassScore,
                                 RandomizeAnswerOption = temp.RandomizeAnswerOption,
                                 RandomizeQuestion = temp.RandomizeQuestion,
                                 ShowResultScore = temp.ShowResultScore,
                                 TimeExam = temp.TimeExam,
                             }).FirstOrDefaultAsync();
            return ExamEditDTO;
        }

        public async Task<KeyValuePair<bool, string>> StartExam(Guid IdExam, Guid UserId)
        {
            Examination? exam = await _Db.Examinations
        .Include(e => e.ExamQuestionTypes)
        .FirstOrDefaultAsync(e => e.Id == IdExam);

            if (exam == null)
            {
                return new KeyValuePair<bool, string>(false, "آزمون مورد نظر یافت نشد.");
            }

            if (exam.StartExam > DateTime.Now)
            {
                return new KeyValuePair<bool, string>(false, $"این آزمون هنوز شروع نشده است. زمان شروع: {exam.StartExam.ToString("yyyy/MM/dd HH:mm")}");
            }

            if (exam.EndExam < DateTime.Now)
            {
                return new KeyValuePair<bool, string>(false, "مهلت شرکت در این آزمون به پایان رسیده است.");
            }

            var existingParticipation = await _Db.ExamUsers
                .FirstOrDefaultAsync(eu => eu.ExamId == IdExam && eu.UserExaminee == UserId);

            if (existingParticipation != null)
            {
                if (existingParticipation.DateFinish == null)
                {
                    return new KeyValuePair<bool, string>(true, existingParticipation.Id.ToString());
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "شما قبلاً در این آزمون شرکت کرده‌اید و آزمون شما خاتمه یافته است.");
                }
            }

            var newExamUser = new ExamUsers
            {
                Id = Guid.NewGuid(),
                ExamId = IdExam,
                UserExaminee = UserId,
                DateCreate = DateTime.Now,
                DateFinish = null,
                 
            };

            var userAnswers = new List<UserAnswer>();

            if (exam.ExamQuestionTypes == null || !exam.ExamQuestionTypes.Any())
            {
                return new KeyValuePair<bool, string>(false, "این آزمون هنوز سوالی برای نمایش ندارد. لطفا با برگزارکننده تماس بگیرید.");
            }

            List<Guid?> IdQuestionStable = exam.ExamQuestionTypes.Where(t=>t.QuestionId != null).Select(t=>t.QuestionId).ToList();

            List<Question> StableQuestion = await _Db.Question.Include(temp=>temp.AnswerOptions).Where(temp=> IdQuestionStable.Contains(temp.Id)).ToListAsync();

            foreach (var rule in exam.ExamQuestionTypes)
            {
                if (rule.QuestionId != null)
                {
                    var question = StableQuestion.FirstOrDefault(t=>t.Id == rule.QuestionId);
                    if (question != null)
                    {
                        if(!userAnswers.Any(t=>t.QuestionId == question.Id))
                        {
                            userAnswers.Add(CreateAnswerRow(question, newExamUser.Id));
                        }
                    }
                }
                else if (rule.CategoryId != null && rule.Count > 0)
                {
                    var randomQuestions = await _Db.Question.Include(temp=>temp.AnswerOptions)
                        .Where(q => q.CategoryId == rule.CategoryId)
                        .OrderBy(q => Guid.NewGuid()) 
                        .Take(rule.Count.Value)
                        .ToListAsync();

                    foreach (var q in randomQuestions)
                    {
                        if (!userAnswers.Any(t => t.QuestionId == q.Id))
                        {
                            userAnswers.Add(CreateAnswerRow(q, newExamUser.Id));
                        }
                    }
                }
            }

            if (userAnswers.Count == 0)
            {
                return new KeyValuePair<bool, string>(false, "خطا در بارگذاری سوالات آزمون. سوالی تعریف نشده است.");
            }

            if (exam.RandomizeQuestion){
                userAnswers = userAnswers.OrderBy(q => Guid.NewGuid()).ToList();
            }

            using (var transaction = await _Db.Database.BeginTransactionAsync())
            {
                try
                {
                    await _Db.ExamUsers.AddAsync(newExamUser);
                    await _Db.UserAnswer.AddRangeAsync(userAnswers);

                    await _Db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new KeyValuePair<bool, string>(false, ex.Message);
                }
            }

            return new KeyValuePair<bool, string>(true, newExamUser.Id.ToString());
        }

        public async Task<ExamConductDTO?> GetCurrentQuestion(Guid IdExamUser, Guid UserId, Guid? IdQuestionNow)
        {
            
            ExamUsers? examUser = await _Db.ExamUsers
                .Include(eu => eu.Exam)
                .FirstOrDefaultAsync(eu => eu.Id == IdExamUser && eu.UserExaminee == UserId);

            if (examUser == null || examUser.DateFinish != null || examUser.Exam == null) return null;

            double passedSeconds = (DateTime.Now - examUser.DateCreate).TotalSeconds;
            int remainingSeconds = (int)(examUser.Exam.TimeExam * 60 - passedSeconds);
            if (remainingSeconds < 0) remainingSeconds = 0;

            
          List<UserAnswer> allAnswers = await _Db.UserAnswer
                .Where(ua => ua.ExamUserId == IdExamUser)
                .OrderBy(ua => ua.Id)
                .ToListAsync();

            UserAnswer? currentAnswer = IdQuestionNow.HasValue
                ? allAnswers.FirstOrDefault(x => x.Id == IdQuestionNow)
                : allAnswers.FirstOrDefault(x => string.IsNullOrEmpty(x.UserAnswerText)) ?? allAnswers.First();

            if (currentAnswer == null) return null;


            Question? originalQuestion = await _Db.Question
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == currentAnswer.QuestionId);

            List<string> options = new List<string>();
            
            if (originalQuestion != null && originalQuestion.AnswerOptions != null && !originalQuestion.IsDescriptiveQuestion)
            {
                options = originalQuestion.AnswerOptions.Select(o => o.AnswerText!).ToList();

                if (examUser.Exam.RandomizeAnswerOption) 
                {
                    options = options.OrderBy(x => Guid.NewGuid()).ToList();
                }
            }

            return new ExamConductDTO
            {
                ExamUserId = examUser.Id,
                ExamTitle = examUser.Exam.Title,
                RemainingSeconds = remainingSeconds,
                CurrentUserAnswerId = currentAnswer.Id,
                QuestionText = currentAnswer.QuestionText,
                IsDescriptive = originalQuestion?.IsDescriptiveQuestion ?? true,
                UserAnswerText = currentAnswer.UserAnswerText,
                AnswerOptions = options,
                TotalQuestions = allAnswers.Count,
                CurrentQuestionIndex = allAnswers.IndexOf(currentAnswer) + 1,
                Steps = allAnswers.Select((x, index) => new NavStepDto
                {
                    UserAnswerId = x.Id,
                    Index = index + 1,
                    IsAnswered = !string.IsNullOrEmpty(x.UserAnswerText)
                }).ToList()
            };

        }

        public async Task<KeyValuePair<SaveAnswerStatus, string>> SaveAnswerAsync(Guid IdUserAnswer, Guid UserId, string Answer)
        {

            try
            {
                UserAnswer? userAnswer = await _Db.UserAnswer
                  .Include(ua => ua.ExamUser)
                  .ThenInclude(temp => temp.Exam)
                  .FirstOrDefaultAsync(ua => ua.Id == IdUserAnswer && ua.ExamUser != null && ua.ExamUser.UserExaminee == UserId);

                if (userAnswer == null)
                {
                    return new KeyValuePair<SaveAnswerStatus, string>(SaveAnswerStatus.QuestionNotFound, "سوال مد نظر یافت نشد");
                }

                ExamUsers examUser = userAnswer.ExamUser!;

                if (examUser.DateFinish != null)
                {
                    return new KeyValuePair<SaveAnswerStatus, string>(SaveAnswerStatus.AlreadyFinished, "آزمون پایان یافته است");
                }
                double passedSeconds = (DateTime.Now - examUser.DateCreate).TotalSeconds;
                int remainingSeconds = (int)(examUser.Exam.TimeExam * 60 - passedSeconds);
                if (remainingSeconds <= 0)
                {
                    return new KeyValuePair<SaveAnswerStatus, string>(SaveAnswerStatus.TimeExpired, "زمان آزمون شما به پایان رسیده است و پاسخ جدید ثبت نشد.");
                }
                userAnswer.UserAnswerText = Answer?.Trim();
                await _Db.SaveChangesAsync();

                return new KeyValuePair<SaveAnswerStatus, string>(SaveAnswerStatus.Success, "جواب ذخیره شد.");
            }
            catch(Exception e)
            {
                return new KeyValuePair<SaveAnswerStatus, string>(SaveAnswerStatus.ErrorExption, $"جواب ذخیره نشد مشکل سیستمی{e.Message}");
            }


        }

        public async Task<KeyValuePair<bool, string>> FinalizeAndQueueExamAsync(Guid IdExam, Guid UserId)
        {
            ExamUsers? examUser = await _Db.ExamUsers
            .Include(eu => eu.Exam)
            .Include(eu => eu.UserAnswers!)
            .ThenInclude(eu => eu.Question)
            .FirstOrDefaultAsync(eu => eu.Id == IdExam && eu.UserExaminee == UserId && eu.UserAnswers != null && eu.Exam != null);

            if (examUser == null) return new KeyValuePair<bool, string>(false, "آزمون یافت نشد.");
            if (examUser.DateFinish != null) return new KeyValuePair<bool, string>(true, examUser.Id.ToString());

            examUser.DateFinish = DateTime.Now;

            double totalExamBarom = examUser.UserAnswers!.Sum(ua => ua.MaxScore);
            double maxScoreTarget = examUser.Exam!.MaxScore;

            int currentRawScore = 0;
            bool hasDescriptive = false;

            foreach (UserAnswer answer in examUser.UserAnswers!)
            {
                if (!answer.Question!.IsDescriptiveQuestion)
                {

                    if(answer.UserAnswerText == null)
                    {
                        answer.ObtainedScore = null; continue;
                    }
                    if (answer.UserAnswerText.Trim() == answer.AnswerText?.Trim())
                    {
                        answer.ObtainedScore = answer.MaxScore;
                        currentRawScore += (int)answer.ObtainedScore;
                    }
                    else
                    {
                        answer.ObtainedScore = 0;
                    }
                    answer.CorrectionStatus = true;
                }
                else
                {
                    answer.CorrectionStatus = false;
                    hasDescriptive = true;
                }
            }


            if (totalExamBarom > 0)
            {
                double rawScore = (currentRawScore / totalExamBarom) * maxScoreTarget;
                examUser.ScoreFinal = Math.Round(rawScore, 2);
            }
            else
            {
                examUser.ScoreFinal = 0;
            }

            if (!hasDescriptive)
            {
                examUser.IsFinishedScore = true;
                if (examUser.Exam.HasCertificate && examUser.Exam.PassScore <= examUser.ScoreFinal)
                {
                    Certificate Add = new Certificate()
                    {
                        DateHolder = DateTime.Now,
                        ExamUserId = examUser.Id,
                        CertificateHolder = examUser.UserExaminee,
                        Id = Guid.NewGuid(),

                    };
                    await _Db.Certificate.AddAsync(Add);
                }
            }
            else 
            {
                examUser.IsFinishedScore = false;            
            }

            await _Db.SaveChangesAsync();

            return new KeyValuePair<bool, string>(true, examUser.Id.ToString());
        }

        public async Task<ExamResultDTO?> ResultExam(Guid IdExam, Guid IdUser)
        {
            ExamResultDTO? Result = await _Db.ExamUsers
                .Where(temp => temp.Id == IdExam && temp.UserExaminee == IdUser && temp.UserAnswers != null)
                .Select(temp => new ExamResultDTO()
                {
                    ExamTitle = temp.Exam.Title,
                    TotalScore = temp.Exam != null ? temp.Exam.MaxScore : 0,
                    Score = (temp.IsFinishedScore) ? temp.ScoreFinal : null,
                    CertificateUrl = (temp.Certificates != null && temp.Certificates.Any()) ? temp.Certificates.Select(t => t.Id).FirstOrDefault() : null,
                    CorrectAnswers = temp.UserAnswers!.Count(t => t.ObtainedScore > 0),
                    HasCertificate = (temp.Exam != null && temp.Exam.HasCertificate)?true:false,
                    IsCorrected = temp.IsFinishedScore,
                    PassingScore = temp.Exam != null ? temp.Exam.PassScore:0,
                    Unanswered = temp.UserAnswers!.Count(t => t.ObtainedScore == null),
                    WrongAnswers = temp.UserAnswers!.Count(t => t.ObtainedScore == 0),
                }).FirstOrDefaultAsync();
            return Result;
        }

        public async Task<List<UserInExamDTO>> GetUserInExam(Guid IdExam, Guid IdUser)
        {
            List<UserInExamDTO> Result = await _Db.ExamUsers
                .Where(temp => temp.ExamId == IdExam && temp.Exam.UserCreate == IdUser)
                .OrderByDescending(temp => temp.DateFinish).Select(temp => new UserInExamDTO
                {
                    Id = temp.Id,
                    DateEnd = temp.DateFinish,
                    DateStart = temp.DateCreate,
                    IsFinalScore = temp.IsFinishedScore,
                    NameUser = temp.User!.Name,
                    Score = (temp.IsFinishedScore) ? temp.ScoreFinal : null,
                }).ToListAsync();
            return Result;
        }

        public async Task<ExamUserCheckDTO?> GetQuestionNotScore(Guid IdUserExam, Guid IdUser)
        {
            ExamUserCheckDTO? Result = await _Db.ExamUsers
                .Where(t => t.Id == IdUserExam
                && t.Exam != null
                && t.Exam.UserCreate == IdUser
                && t.User != null
                && t.UserAnswers != null)
                .Select(t => new ExamUserCheckDTO
                {
                    ExamName = t.Exam!.Title,
                    Name = t.User!.Name,
                    QuestionExam = t.UserAnswers!
                    .Where(b => b.CorrectionStatus == false)
                    .Select(b => new CheckQuestionDTO
                    {
                        IdUserAnswer = b.Id,
                        MaxScore = b.MaxScore,
                        Question = b.QuestionText,
                        QuestionAnswer = b.AnswerText,
                        QuestionAnswerUser = b.UserAnswerText,
                        AssignedScore = b.ObtainedScore
                    }).ToList()

                }).FirstOrDefaultAsync();
            return Result;
        }

        public async Task<KeyValuePair<bool, string>> SubmitScore(Guid IdUserAnser, Guid UserId, double Score)
        {
            try
            {
                UserAnswer? Find = await _Db.UserAnswer.FirstOrDefaultAsync(t=>t.Id == IdUserAnser &&t.ExamUser != null&&t.ExamUser.Exam != null && t.ExamUser.Exam.UserCreate == UserId);
                
                
                if (Find == null) return new KeyValuePair<bool, string>(false,"این سوال یافت نشد");
                if (Score < 0) return new KeyValuePair<bool, string>(false, "نمره نمی‌تواند عدد منفی باشد");
                if (Find.MaxScore < Score) return new KeyValuePair<bool, string>(false, "نمره اعطا شده نمی تواند از نمره کل بیشتر باشد");
                
                
                Find.ObtainedScore = Score;
                Find.CorrectionStatus = true;
               
                
                await _Db.SaveChangesAsync();
                
                
                ExamUsers? examUser = await _Db.ExamUsers
                    .Include(t=>t.Exam)
                    .Include(t=>t.UserAnswers)
                    .Include(t=>t.Certificates)
                    .FirstOrDefaultAsync(t => t.Id == Find.ExamUserId);
                
                if (examUser == null) return new KeyValuePair<bool, string>(false, "این سوال یافت نشد");
                
                
                double totalExamBarom = examUser.UserAnswers!.Sum(ua => ua.MaxScore);
                double maxScoreTarget = examUser.Exam!.MaxScore;

                double currentRawScore = examUser.UserAnswers!.Sum(a=>a.ObtainedScore ?? 0);
                bool hasDescriptive = examUser.UserAnswers!.Any(t=>t.CorrectionStatus ==false);
                

                if (totalExamBarom > 0)
                {
                    double rawScore = (currentRawScore / totalExamBarom) * maxScoreTarget;
                    examUser.ScoreFinal = Math.Round(rawScore, 2);
                }
                else
                {
                    examUser.ScoreFinal = 0;
                }

                if (!hasDescriptive)
                {
                    examUser.IsFinishedScore = true;
                    if (examUser.Exam.HasCertificate && examUser.Exam.PassScore <= examUser.ScoreFinal)
                    {
                        Certificate Add = new Certificate()
                        {
                            DateHolder = DateTime.Now,
                            ExamUserId = examUser.Id,
                            CertificateHolder = examUser.UserExaminee,
                            Id = Guid.NewGuid(),

                        };
                        await _Db.Certificate.AddAsync(Add);
                    }
                    else
                    {
                        Certificate? Delete = examUser.Certificates?.FirstOrDefault();
                        if(Delete != null)
                        {
                            _Db.Certificate.Remove(Delete);
                        }
                    }
                }
                else
                {
                    examUser.IsFinishedScore = false;
                }
                
                
                await _Db.SaveChangesAsync();
                return new KeyValuePair<bool, string>(true, "موفق انجام شد");
            }
            catch
            {
                return new KeyValuePair<bool, string>(false, "خطا در برقراری ارتباط با دیتابیس");
            }
        }

        public async Task<ExamUsersDetailsDTO?> GetExamUsersDetails(Guid IdExamUser, Guid IdUser)
        {
            ExamUsersDetailsDTO? Result = await _Db.ExamUsers
                .Where(t => t.Id == IdExamUser
                && t.Exam != null
                && t.Exam.UserCreate == IdUser
                && t.User != null
                && t.UserAnswers != null)
                .Select(t => new ExamUsersDetailsDTO
                {
                    ExamTitle = t.Exam!.Title,
                    FullName = t.User!.Name,
                    EndTime = t.DateFinish,
                    IsCorrected = t.IsFinishedScore,
                    ObtainedScore = t.ScoreFinal,
                    PassingScore = t.Exam.PassScore,
                    StartTime = t.DateCreate,
                    TotalScore = t.Exam.MaxScore,
                    QouestionUser = t.UserAnswers!
                    .Select(b => new QuestionDetailsForExamDTO
                    {
                        IdUserAnswer = b.Id,
                        MaxScore = b.MaxScore,
                        Question = b.QuestionText,
                        QuestionAnswer = b.AnswerText,
                        QuestionAnswerUser = b.UserAnswerText,
                        AssignedScore = b.ObtainedScore,
                        IsIsCorrected = b.CorrectionStatus ?? false,
                    }).ToList()

                }).FirstOrDefaultAsync();
            return Result;
        }

        public async Task<List<MyExamDTO>> GetMyExam(Guid UserId)
        {
            List<MyExamDTO> Result = await _Db.ExamUsers
                .Where(temp => temp.UserExaminee == UserId)
                .OrderByDescending(temp => temp.DateCreate)
                .Select(temp => new MyExamDTO
                {
                    Id = temp.Id,
                    DateEnd = temp.DateFinish,
                    DateStart = temp.DateCreate,
                    IsFinalScore = temp.IsFinishedScore,
                    Name = temp.Exam!.Title,
                    Score = (temp.IsFinishedScore) ? temp.ScoreFinal : null,
                }).ToListAsync();
            return Result;
        }
    }
}
