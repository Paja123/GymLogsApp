export interface TrainingSessionResponseDto {
  id: string;
  trainingType: string;
  duration: number;
  caloriesBurned?: number | null;
  intensityLevel: number;
  tirednessLevel: number;
  date: string;
  notes?: string | null;
}
