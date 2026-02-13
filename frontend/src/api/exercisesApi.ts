import { httpGet } from "./http";
import type { Exercise } from "../types/exercise";
import type { PagedResult } from "../types/pagedResult";

// I keep the endpoint paths here so components stay clean
export async function getExercises(page: number, pageSize: number) {
  const query = `?page=${page}&pageSize=${pageSize}`;
  return await httpGet<PagedResult<Exercise>>(`/api/Exercises${query}`);
}