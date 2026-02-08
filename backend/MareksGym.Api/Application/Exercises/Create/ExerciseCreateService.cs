using MareksGym.Api.Application.Common;
using MareksGym.Api.Contracts.Exercises.Create;
using MareksGym.Api.Infrastructure.Persistence;
using MareksGym.Api.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Application.Exercises.Create;

public class ExerciseCreateService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CreateExerciseValidator _validator;

    public ExerciseCreateService(ApplicationDbContext dbContext, CreateExerciseValidator validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<(ValidationResult Validation, CreateExerciseResponse? Created)> CreateAsync(
        CreateExerciseRequest? request,
        CancellationToken cancellationToken)
    {
        // First I validate the request body (required fields, max lengths, etc.)
        ValidationResult validation = _validator.Validate(request);

        // If validation failed (or request is missing), I stop here and return the errors
        if (!validation.IsValid || request is null)
        {
            return (validation, null);
        }

        // I trim the name so extra spaces don't get saved to the database
        string name = request.Name.Trim();

        // MuscleGroup is optional, so I only save it if it has a real value
        string? muscleGroup = null;
        if (!string.IsNullOrWhiteSpace(request.MuscleGroup))
        {
            muscleGroup = request.MuscleGroup.Trim();
        }

        // Equipment is optional as well
        string? equipment = null;
        if (!string.IsNullOrWhiteSpace(request.Equipment))
        {
            equipment = request.Equipment.Trim();
        }

        // Description is optional and can be longer text
        string? description = null;
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            description = request.Description.Trim();
        }

        // I check if an exercise with the same key details already exists in the database. I treat Name + MuscleGroup + Equipment as a unique combination
        bool exerciseAlreadyExists = await _dbContext.Exercises.AnyAsync(
            exercise =>
                exercise.Name == name &&
                exercise.MuscleGroup == muscleGroup &&
                exercise.Equipment == equipment,
            cancellationToken
        );

        if (exerciseAlreadyExists)
        {
            // I attach the error to the Name field because that's the main thing the user types
            validation.AddError(nameof(request.Name), "This exercise already exists.");
            return (validation, null);
        }

        // Map request to entity
        Exercise entity = new Exercise
        {
            Name = name,
            MuscleGroup = muscleGroup,
            Equipment = equipment,
            Description = description
        };

        // Add + SaveChangesAsync writes it into the database
        _dbContext.Exercises.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // After SaveChanges, EF populates entity.Id
        CreateExerciseResponse created = new CreateExerciseResponse(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.MuscleGroup,
            entity.Equipment
        );

        return (validation, created);
    }
}