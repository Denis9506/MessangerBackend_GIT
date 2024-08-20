using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Services;
using MessangerBackend.Middlewares;
using MessangerBackend.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MessangerContext>(options => options.UseSqlServer("Server=.\\SQLEXPRESS;Database=MessangerDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;"));
builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    ;
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<UserStatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<UserStatisticsMiddleware>();

app.UseMiddleware<MessageFilteringMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();