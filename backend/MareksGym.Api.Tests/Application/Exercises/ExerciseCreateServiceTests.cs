using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MareksGym.Api.Application.Exercises.Create;
using MareksGym.Api.Contracts.Exercises.Create;
using MareksGym.Api.Infrastructure.Persistence;
using MareksGym.Api.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MareksGym.Api.Tests.Application.Exercises;

public class ExerciseCreateServiceTests
{
    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_WhenRequestIsNull_ReturnsValidationError()
    {
        using var db = CreateDbContext();
        var validator = new CreateExerciseValidator();
        var service = new ExerciseCreateService(db, validator);

        var (validation, created) = await service.CreateAsync(null, CancellationToken.None);

        Assert.Null(created);
        Assert.False(validation.IsValid);

        // Validator uses result.AddError("request", "Request body is required.");
        Assert.True(validation.Errors.ContainsKey("request"));
    }

    [Fact]
    public async Task CreateAsync_WhenDuplicateExerciseExists_ReturnsDuplicateError()
    {
        using var db = CreateDbContext();

        db.Exercises.Add(new Exercise
        {
            Name = "Bench Press",
            MuscleGroup = "Chest",
            Equipment = "Barbell",
            Description = null
        });

        await db.SaveChangesAsync();

        var validator = new CreateExerciseValidator();
        var service = new ExerciseCreateService(db, validator);

        var request = new CreateExerciseRequest(
            Name: "Bench Press",
            Description: null,
            MuscleGroup: "Chest",
            Equipment: "Barbell"
        );

        var (validation, created) = await service.CreateAsync(request, CancellationToken.None);

        Assert.Null(created);
        Assert.False(validation.IsValid);

        Assert.True(validation.Errors.ContainsKey(nameof(request.Name)));
        Assert.Contains("already exists", validation.Errors[nameof(request.Name)].First());
    }
}