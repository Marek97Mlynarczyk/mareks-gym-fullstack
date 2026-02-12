export type ExerciseListItem = {
  id: number;
  name: string;
  muscleGroup: string | null;
  equipment: string | null;
};

export type ExercisesPageResponse = {
  items: ExerciseListItem[];
  page: number;
  pageSize: number;
  totalCount: number;
};