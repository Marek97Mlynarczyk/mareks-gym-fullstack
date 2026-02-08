using MareksGym.Api.Application.Common;
using MareksGym.Api.Contracts.Exercises.Create;

namespace MareksGym.Api.Application.Exercises.Create;

public class CreateExerciseValidator
{
    public ValidationResult Validate(CreateExerciseRequest? request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(string.Empty, "Request body is required.");
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(nameof(request.Name), "Name is required.");
        }
        else if (request.Name.Trim().Length > 100)
        {
            result.AddError(nameof(request.Name), "Name must be 100 characters or less.");
        }

        if (!string.IsNullOrWhiteSpace(request.MuscleGroup) && request.MuscleGroup.Trim().Length > 50)
        {
            result.AddError(nameof(request.MuscleGroup), "Muscle Group must be 50 characters or less.");
        }

        if (!string.IsNullOrWhiteSpace(request.Equipment) && request.Equipment.Trim().Length > 50)
        {
            result.AddError(nameof(request.Equipment), "Equipment must be 50 characters or less.");
        }

        if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Trim().Length > 500)
        {
            result.AddError(nameof(request.Description), "Description must be 500 characters or less.");
        }

        return result;
    }
}