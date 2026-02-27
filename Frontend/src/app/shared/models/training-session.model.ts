import { TrainingType } from "./training-type.enum";

export interface TrainingSession {
    id?: string;
    userId: string;
    trainingType: TrainingType;
    duration: number;
    caloriesBurned?: number;
    intensityLevel: number;
    tirednessLevel: number;
    date: string | Date;
    notes?: string;
}