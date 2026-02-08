using MareksGym.Api.Contracts.Exercises;
using MareksGym.Api.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Application.Exercises;

public class ExerciseSearchStoredProcEfService
{
    private readonly ApplicationDbContext _db;

    public ExerciseSearchStoredProcEfService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<GetExercisesPageResponse> SearchAsync(
        string? search,
        string? muscleGroup,
        string? equipment,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        // Build SQL parameters explicitly so it's clear what's passed to SQL
        var parameters = new[]
        {
            new SqlParameter("@Search", search ?? (object)DBNull.Value),
            new SqlParameter("@MuscleGroup", muscleGroup ?? (object)DBNull.Value),
            new SqlParameter("@Equipment", equipment ?? (object)DBNull.Value),
            new SqlParameter("@Page", page),
            new SqlParameter("@PageSize", pageSize)
        };

        // Call the stored procedure and map rows to ExerciseSearchRow
        var rows = await _db.Set<ExerciseSearchRow>()
            .FromSqlRaw(
                "EXEC dbo.SearchExercises @Search, @MuscleGroup, @Equipment, @Page, @PageSize",
                parameters)
            .AsNoTracking()
            .ToListAsync(ct);

        // TotalCount needs a separate call in EF Core
        var totalCount = rows.Count;

        var items = rows.Select(x => new GetExercisesResponse(
            x.Id,
            x.Name,
            x.MuscleGroup,
            x.Equipment
        )).ToList();

        return new GetExercisesPageResponse(items, page, pageSize, totalCount);
    }
}