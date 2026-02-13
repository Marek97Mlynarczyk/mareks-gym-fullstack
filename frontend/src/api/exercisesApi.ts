import type { Exercise, PagedResult } from "../types/exercise";

// I keep the API base URL in one constant so I don't repeat it everywhere
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7099";

// I keep API calls in this file so my React components don't contain HTTP details
export async function getExercises(page: number, pageSize: number): Promise<PagedResult<Exercise>> {
  const url = `${API_BASE_URL}/api/Exercises?page=${page}&pageSize=${pageSize}`;

  // I call the backend endpoint and check if it succeeded
  const response = await fetch(url);

  if (!response.ok) {
    // I throw an error so the UI can show something helpful
    throw new Error(`Failed to fetch exercises (HTTP ${response.status})`);
  }

  // I parse JSON into the shape my UI expects
  return (await response.json()) as PagedResult<Exercise>;
}