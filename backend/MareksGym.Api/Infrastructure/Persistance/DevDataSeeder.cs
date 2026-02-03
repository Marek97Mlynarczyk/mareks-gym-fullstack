using MareksGym.Api.Infrastructure.Persistence.Entities;

namespace MareksGym.Api.Infrastructure.Persistence;

public static class DevDataSeeder
{
    public static void SeedExercises(ApplicationDbContext db)
    {
        // If there is already data, do nothing
        if (db.Exercises.Any())
        {
            return;
        }

        db.Exercises.AddRange(
            new Exercise
            {
                Name = "Barbell Bench Press",
                MuscleGroup = "Chest",
                Equipment = "Barbell"
            },
            new Exercise
            {
                Name = "Incline Dumbbell Bench",
                MuscleGroup = "Chest",
                Equipment = "Dumbbell"
            },
            new Exercise
            {
                Name = "Barbell Row",
                MuscleGroup = "Back",
                Equipment = "Barbell"
            }
        );

        db.SaveChanges();
    }
}