using MareksGym.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Application.Exercises.Delete;

public class ExerciseDeleteService
{
    private readonly ApplicationDbContext _dbContext;

    public ExerciseDeleteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _dbContext.Exercises
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
        {
            return false;
        }

        _dbContext.Exercises.Remove(entity);

        await _dbContext.SaveChangesAsync(ct);

        return true;
    }
}