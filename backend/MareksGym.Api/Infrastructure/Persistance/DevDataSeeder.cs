using MareksGym.Api.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Infrastructure.Persistence;

public static class DevDataSeeder
{
    public static async Task SeedExercisesAsync(ApplicationDbContext db)
    {
        // If there is already data, I do nothing to avoid duplicates.
        var hasAnyExercises = await db.Exercises.AnyAsync();
        if (hasAnyExercises)
        {
            return;
        }

        /* I keep seed values consistent so my frontend filters look clean.
        MuscleGroup: Chest, Back, Legs, Shoulders, Arms, Core
        Equipment: Barbell, Dumbbell, Machine, Cable, Bodyweight */
        var exercises = new List<Exercise>
        {
            // Chest
            new() { Name = "Barbell Bench Press", MuscleGroup = "Chest", Equipment = "Barbell" },
            new() { Name = "Incline Dumbbell Bench Press", MuscleGroup = "Chest", Equipment = "Dumbbell" },
            new() { Name = "Decline Dumbbell Press", MuscleGroup = "Chest", Equipment = "Dumbbell" },
            new() { Name = "Machine Chest Press", MuscleGroup = "Chest", Equipment = "Machine" },
            new() { Name = "Cable Fly", MuscleGroup = "Chest", Equipment = "Cable" },
            new() { Name = "Push-Up", MuscleGroup = "Chest", Equipment = "Bodyweight" },

            // Back
            new() { Name = "Barbell Row", MuscleGroup = "Back", Equipment = "Barbell" },
            new() { Name = "One-Arm Dumbbell Row", MuscleGroup = "Back", Equipment = "Dumbbell" },
            new() { Name = "Lat Pulldown", MuscleGroup = "Back", Equipment = "Machine" },
            new() { Name = "Seated Cable Row", MuscleGroup = "Back", Equipment = "Cable" },
            new() { Name = "Pull-Up", MuscleGroup = "Back", Equipment = "Bodyweight" },
            new() { Name = "Straight-Arm Cable Pulldown", MuscleGroup = "Back", Equipment = "Cable" },

            // Legs
            new() { Name = "Barbell Back Squat", MuscleGroup = "Legs", Equipment = "Barbell" },
            new() { Name = "Romanian Deadlift", MuscleGroup = "Legs", Equipment = "Barbell" },
            new() { Name = "Leg Press", MuscleGroup = "Legs", Equipment = "Machine" },
            new() { Name = "Leg Extension", MuscleGroup = "Legs", Equipment = "Machine" },
            new() { Name = "Seated Leg Curl", MuscleGroup = "Legs", Equipment = "Machine" },
            new() { Name = "Dumbbell Walking Lunge", MuscleGroup = "Legs", Equipment = "Dumbbell" },
            new() { Name = "Standing Calf Raise", MuscleGroup = "Legs", Equipment = "Machine" },

            // Shoulders
            new() { Name = "Barbell Overhead Press", MuscleGroup = "Shoulders", Equipment = "Barbell" },
            new() { Name = "Dumbbell Shoulder Press", MuscleGroup = "Shoulders", Equipment = "Dumbbell" },
            new() { Name = "Dumbbell Lateral Raise", MuscleGroup = "Shoulders", Equipment = "Dumbbell" },
            new() { Name = "Cable Lateral Raise", MuscleGroup = "Shoulders", Equipment = "Cable" },
            new() { Name = "Face Pull", MuscleGroup = "Shoulders", Equipment = "Cable" },
            new() { Name = "Rear Delt Machine Fly", MuscleGroup = "Shoulders", Equipment = "Machine" },

            // Arms
            new() { Name = "EZ-Bar Curl", MuscleGroup = "Arms", Equipment = "Barbell" },
            new() { Name = "Dumbbell Hammer Curl", MuscleGroup = "Arms", Equipment = "Dumbbell" },
            new() { Name = "Cable Triceps Pushdown", MuscleGroup = "Arms", Equipment = "Cable" },
            new() { Name = "Overhead Cable Triceps Extension", MuscleGroup = "Arms", Equipment = "Cable" },
            new() { Name = "Dumbbell Incline Curl", MuscleGroup = "Arms", Equipment = "Dumbbell" },
            new() { Name = "Bodyweight Bench Dip", MuscleGroup = "Arms", Equipment = "Bodyweight" },

            // Core
            new() { Name = "Plank", MuscleGroup = "Core", Equipment = "Bodyweight" },
            new() { Name = "Hanging Knee Raise", MuscleGroup = "Core", Equipment = "Bodyweight" },
            new() { Name = "Cable Crunch", MuscleGroup = "Core", Equipment = "Cable" },
            new() { Name = "Dead Bug", MuscleGroup = "Core", Equipment = "Bodyweight" },
            new() { Name = "Russian Twist", MuscleGroup = "Core", Equipment = "Bodyweight" },
        };

        await db.Exercises.AddRangeAsync(exercises);
        await db.SaveChangesAsync();
    }
}