using MareksGym.Api.Application.Exercises;
using MareksGym.Api.Application.Exercises.Create;
using MareksGym.Api.Application.Exercises.Delete;
using MareksGym.Api.Application.Exercises.Update;
using MareksGym.Api.Application.Macros;
using MareksGym.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "FrontendDev";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MacroCalculator>();
builder.Services.AddScoped<MacroRequestMapper>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<MacroHistoryService>();
builder.Services.AddScoped<MacroHistoryQueryService>();
builder.Services.AddScoped<ExerciseQueryService>();
builder.Services.AddScoped<CreateExerciseValidator>();
builder.Services.AddScoped<ExerciseCreateService>();
builder.Services.AddScoped<ExerciseSearchStoredProcEfService>();
builder.Services.AddScoped<UpdateExerciseValidator>();
builder.Services.AddScoped<ExerciseUpdateService>();
builder.Services.AddScoped<ExerciseDeleteService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Seed development data so the API is usable in Swagger
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DevDataSeeder.SeedExercises(db);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(corsPolicyName);

app.MapControllers();

app.Run();