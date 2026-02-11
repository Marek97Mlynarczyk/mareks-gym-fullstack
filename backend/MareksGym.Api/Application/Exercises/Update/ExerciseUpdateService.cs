using MareksGym.Api.Application.Common;
using MareksGym.Api.Contracts.Exercises.Update;
using MareksGym.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Application.Exercises.Update;

public class ExerciseUpdateService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UpdateExerciseValidator _validator;

    public ExerciseUpdateService(ApplicationDbContext dbContext, UpdateExerciseValidator validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<(ValidationResult Validation, UpdateExerciseResponse? Updated)> UpdateAsync(
        int id,
        UpdateExerciseRequest? request,
        CancellationToken ct)
    {
        // I validate the request first. If it's invalid, I stop and return errors
        ValidationResult validation = _validator.Validate(request);
        if (!validation.IsValid || request == null)
        {
            return (validation, null);
        }

        // I try to find the exercise in the database by id
        var entity = await _dbContext.Exercises.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity == null)
        {
            validation.AddError("id", "Exercise not found.");
            return (validation, null);
        }

        // I clean up input strings so spaces don't create fake differences in the database
        string name = request.Name.Trim();

        string? muscleGroup = request.MuscleGroup;
        if (string.IsNullOrWhiteSpace(muscleGroup))
            muscleGroup = null;
        else
            muscleGroup = muscleGroup.Trim();

        string? equipment = request.Equipment;
        if (string.IsNullOrWhiteSpace(equipment))
            equipment = null;
        else
            equipment = equipment.Trim();

        string? description = request.Description;
        if (string.IsNullOrWhiteSpace(description))
            description = null;
        else
            description = description.Trim();

        // I check if another exercise already has the same unique combo of Name + MuscleGroup + Equipment
        // I exclude the current entity (x.Id != id), because it's allowed to match itself
        bool duplicateExists = await _dbContext.Exercises.AnyAsync(
            x => x.Id != id &&
                 x.Name == name &&
                 x.MuscleGroup == muscleGroup &&
                 x.Equipment == equipment,
            ct
        );

        // If a duplicate exists, I return a validation error and stop
        if (duplicateExists)
        {
            validation.AddError(nameof(request.Name), "This exercise already exists.");
            return (validation, null);
        }

        // I update the entity with the cleaned values.
        entity.Name = name;
        entity.MuscleGroup = muscleGroup;
        entity.Equipment = equipment;
        entity.Description = description;

        // I save changes to the database.
        await _dbContext.SaveChangesAsync(ct);

        // I return a response DTO so the controller doesn't expose the EF entity directly
        var updated = new UpdateExerciseResponse(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.MuscleGroup,
            entity.Equipment
        );

        return (validation, updated);
    }
}