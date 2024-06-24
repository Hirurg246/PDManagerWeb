using PDManagerWeb.Models;
using PDManagerWeb.Controllers;
using PDManagerWeb.Middleware;
using PDManagerWeb.Models.MappingProfiles;
using PDManagerWeb.Repositories.Interfaces;
using PDManagerWeb.Repositories;
using PDManagerWeb.Services.Interfaces;
using PDManagerWeb.Services;
using System.Reflection.Metadata;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PDManagerContext>();

builder.Services.AddAutoMapper(typeof(AccountMappingProfile));

builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<IAccountsQueryService, AccountsQueryService>();
builder.Services.AddScoped<IAccountsCommandService, AccountsCommandService>();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
