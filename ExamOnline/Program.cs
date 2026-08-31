using Application.Core.Domain.Entites;
using Application.Core.Domain.Interface;
using Application.Infrastructrue.DbContext;
using Application.Infrastructrue.Identity;
using Application.Infrastructrue.ImplementationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUser,UserImplementation>();
builder.Services.AddScoped<IAnswerOption, AnswerOptionImplementation>();
builder.Services.AddScoped<ICategory, CategoryImplementation>();
builder.Services.AddScoped<ICertificate, CertificateImplementation>();
builder.Services.AddScoped<IExamination, ExaminationImplementation>();
builder.Services.AddScoped<IExamQuestionTypes, ExamQuestionTypesImplementation>();
builder.Services.AddScoped<IExamUsers, ExamUsersImplementation>();
builder.Services.AddScoped<IQuestion, QuestionImplementation>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<PersianIdentityErrorDescriber>()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>();
builder.Services.AddAuthorization(option =>
option.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
builder.Services.ConfigureApplicationCookie(Options =>
{
    Options.ExpireTimeSpan = TimeSpan.FromDays(30);
    Options.SlidingExpiration = true;
    Options.LoginPath = "/Account/Login";
    Options.AccessDeniedPath = "/Account/Login";
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    IServiceProvider ADD = scope.ServiceProvider;
    await IdentitySeed.SeedData(ADD);
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
