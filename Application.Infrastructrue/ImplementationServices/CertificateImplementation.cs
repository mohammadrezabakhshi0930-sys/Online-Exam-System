using Application.Core.Domain.Interface;
using Application.Core.DTO.CertificateDto;
using Application.Infrastructrue.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.ImplementationServices
{
    public class CertificateImplementation : ICertificate
    {
        private readonly AppDbContext _Db;
        public CertificateImplementation(AppDbContext db)
        {
            _Db = db;
        }
        public async Task<CertificateDetailsDTO?> GetCertificate(Guid Id)
        {
            CertificateDetailsDTO? Result = await _Db.Certificate.Where(t => t.Id == Id)
                 .Select(t => new CertificateDetailsDTO
                 {
                     ExamName = t.ExamUsers.Exam.Title,
                     FullName = t.User.Name,
                     IssueDate = t.DateHolder,
                     Score = t.ExamUsers.ScoreFinal ?? 0,
                     TotalScore = t.ExamUsers.Exam.MaxScore
                 }).FirstOrDefaultAsync();
            return Result;
        }
    }
}
