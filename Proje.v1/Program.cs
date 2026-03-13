using Microsoft.EntityFrameworkCore;

using FluentValidation;
using FluentValidation.AspNetCore;
using Proje.v1.Data;
using Proje.v1.Services;
using Proje.v1.Validators;

using Proje.v1.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateNoteValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();