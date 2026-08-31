using Application.Core.DTO.ExamQuestionTypeDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTO.ExaminationDto
{
    public class ExamDetailsDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartExam { get; set; }
        public DateTime EndExam { get; set; }
        public int TimeExam { get; set; }
        public string? Description { get; set; }
        public int? PassScore { get; set; }
        public int MaxScore { get; set; }
        public bool RandomizeQuestion { get; set; }
        public bool RandomizeAnswerOption { get; set; }
        public bool ShowResultScore { get; set; }
        public bool HasCertificate { get; set; } 
        public DateTime DateCreate { get; set; }
        public List<ExamQTypesShowDTO>? BeforeExam {  get; set; }
        public List<SumrizeQ>? AfterExam { get; set; }


    }
    public class SumrizeQ 
    { 
        public string? TextQuestion { get; set; }
        public string? TextAnswer { get; set; }
        public int MaxScore {  set; get; }
    }

}
