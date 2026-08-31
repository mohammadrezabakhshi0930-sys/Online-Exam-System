using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Application.Core.Domain.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Infrastructrue.DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions option) : base(option)
        {

        }
        public DbSet<ExamUsers> ExamUsers { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<ExamQuestionTypes> ExamQuestionTypes { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<UserAnswer> UserAnswer { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }



        }
    }
}
