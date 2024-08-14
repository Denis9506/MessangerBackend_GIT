using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Services;
using MessangerBackend.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MessangerContext>(options => options.UseSqlServer("Server=.\\SQLEXPRESS;Database=MessangerDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;"));
builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();