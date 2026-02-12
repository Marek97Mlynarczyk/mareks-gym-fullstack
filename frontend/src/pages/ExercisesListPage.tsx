import { useEffect, useState } from "react";
import { httpGet } from "../api/http";
import type { ExercisesPageResponse } from "../types/exercises";

export function ExercisesListPage() {
  const [data, setData] = useState<ExercisesPageResponse | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // I'm doing a simple call first just to confirm frontend can reach my .NET API
    httpGet<ExercisesPageResponse>("/api/Exercises?page=1&pageSize=20")
      .then(setData)
      .catch((e) => setError(String(e)));
  }, []);

  if (error) return <pre>{error}</pre>;
  if (!data) return <div>Loading exercises..</div>;

  return (
    <div style={{ padding: 16 }}>
      <h1>Exercises</h1>

      <ul>
        {data.items.map((x) => (
          <li key={x.id}>
            {x.name} — {x.muscleGroup ?? "N/A"} — {x.equipment ?? "N/A"}
          </li>
        ))}
      </ul>
    </div>
  );
}