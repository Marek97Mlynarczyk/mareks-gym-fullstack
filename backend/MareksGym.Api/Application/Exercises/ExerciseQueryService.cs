using MareksGym.Api.Contracts.Exercises;
using MareksGym.Api.Infrastructure.Persistence;
using MareksGym.Api.Contracts.Exercises;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Application.Exercises;

public class ExerciseQueryService
{
    private readonly ApplicationDbContext _dbContext;

    public ExerciseQueryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetExercisesPageResponse> GetExercisesAsync(
        string? search,
        string? muscleGroup,
        string? equipment,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        // Ensure page number is always valid
        if (page < 1)
        {
            page = 1;
        }

        // Ensure page size is within a safe range
        if (pageSize < 1)
        {
            pageSize = 20;
        }
        else if (pageSize > 100)
        {
            pageSize = 100;
        }

        // Start with all exercises (read-only query)
        IQueryable<Infrastructure.Persistence.Entities.Exercise> query =
            _dbContext.Exercises.AsNoTracking();

        // Apply name search if provided
        if (!string.IsNullOrWhiteSpace(search))
        {
            string searchTerm = search.Trim();
            query = query.Where(exercise => exercise.Name.Contains(searchTerm));
        }

        // Filter by muscle group if provided
        if (!string.IsNullOrWhiteSpace(muscleGroup))
        {
            string muscleGroupFilter = muscleGroup.Trim();
            query = query.Where(exercise => exercise.MuscleGroup == muscleGroupFilter);
        }

        // Filter by equipment if provided
        if (!string.IsNullOrWhiteSpace(equipment))
        {
            string equipmentFilter = equipment.Trim();
            query = query.Where(exercise => exercise.Equipment == equipmentFilter);
        }

        // Always return results in a predictable order
        query = query.OrderBy(exercise => exercise.Name);

        // Get total count before paging
        int totalCount = await query.CountAsync(cancellationToken);

        // Apply paging
        List<GetExercisesResponse> exercises = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(exercise => new GetExercisesResponse(
                exercise.Id,
                exercise.Name,
                exercise.MuscleGroup,
                exercise.Equipment
            ))
            .ToListAsync(cancellationToken);

        return new GetExercisesPageResponse(
            exercises,
            page,
            pageSize,
            totalCount
        );
    }

    public async Task<GetExerciseByIdResponse?> GetExerciseByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return null;
        }

        var exercise = await _dbContext.Exercises
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new GetExerciseByIdResponse(
                x.Id,
                x.Name,
                x.Description,
                x.MuscleGroup,
                x.Equipment
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return exercise;
    }
}