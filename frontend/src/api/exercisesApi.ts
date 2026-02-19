import { httpGet, httpPost, httpDelete } from "./http";
import type { Exercise } from "../types/exercise";
import type { PagedResult } from "../types/pagedResult";

export type GetExercisesParams = {
  search?: string;
  muscleGroup?: string;
  equipment?: string;
  page: number;
  pageSize: number;
};

// I keep the endpoint paths here so components stay clean
export async function getExercises(params: GetExercisesParams, signal?: AbortSignal) {
  const query = new URLSearchParams();

  if (params.search && params.search.trim().length > 0) {
    query.set("search", params.search.trim());
  }

  if (params.muscleGroup && params.muscleGroup.trim().length > 0) {
    query.set("muscleGroup", params.muscleGroup.trim());
  }

  if (params.equipment && params.equipment.trim().length > 0) {
    query.set("equipment", params.equipment.trim());
  }

  query.set("page", String(params.page));
  query.set("pageSize", String(params.pageSize));

  return await httpGet<PagedResult<Exercise>>(`/api/exercises?${query.toString()}`, signal);
}

export type CreateExerciseRequest = {
  name: string;
  description?: string | null;
  muscleGroup?: string | null;
  equipment?: string | null;
};

export async function createExercise(
  request: CreateExerciseRequest
) {
  return await httpPost<any, CreateExerciseRequest>(
    "/api/exercises",
    request
  );
}

export async function deleteExercise(id: number) {
  return await httpDelete(`/api/exercises/${id}`);
}