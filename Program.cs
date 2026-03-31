using cs330_proj1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ICourseServices, CourseServices>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
