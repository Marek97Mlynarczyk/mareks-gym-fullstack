import { useEffect, useState } from "react";
import type { Exercise } from "./types/exercise";
import { getExercises } from "./api/exercisesApi";

function App() {
  // I store the exercises I fetch from the backend
  const [items, setItems] = useState<Exercise[]>([]);

  // I use this to show a simple loading message while waiting for the API
  const [loading, setLoading] = useState(true);

  // I store possible error message if something fails
  const [error, setError] = useState<string | null>(null);

  // I use useEffect so this runs once when the component loads
  useEffect(() => {
    async function loadExercises() {
      try {
        // Before fetching I reset error
        setError(null);

        // I set loading to true so I can show a "Loading.." message
        setLoading(true);

        // I call my API layer instead of using fetch directly here
        const result = await getExercises(1, 20);

        // I store the returned exercises in state
        setItems(result.items);
      } catch (e) {
        // If something goes wrong I show error message
        if (e instanceof Error) {
          setError(e.message);
        } else {
          setError("Unknown error occurred.");
        }
      } finally {
        // After the request finishes (success or error), I stop loading
        setLoading(false);
      }
    }

    loadExercises();
  }, []);

  return (
    <div style={{ padding: 24 }}>
      <h1>Exercises</h1>

      {/* I show loading only while I'm waiting for the API */}
      {loading && <p>Loading..</p>}

      {/* I show error only if it exists */}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {/* I show the list only when I'm not loading and I don't have an error */}
      {!loading && !error && (
        <ul>
          {items.map((exercise) => (
            <li key={exercise.id}>
              {exercise.name} — {exercise.muscleGroup ?? "N/A"} —{" "}
              {exercise.equipment ?? "N/A"}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default App;