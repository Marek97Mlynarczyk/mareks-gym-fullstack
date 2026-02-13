// I keep frontend types in one place so my UI code stays simple

export type Exercise = {
  id: number;
  name: string;
  description: string | null;
  muscleGroup: string | null;
  equipment: string | null;
};