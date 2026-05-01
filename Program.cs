using cs330_proj1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<ICourseServices>(new CourseServices(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
