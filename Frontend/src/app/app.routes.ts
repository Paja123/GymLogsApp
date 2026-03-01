import { Routes } from '@angular/router';
import { TrainingForm } from './features/training/pages/training-form/training-form';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'create',
        pathMatch: 'full'
    },
    {
        path: 'create',
        component: TrainingForm
    }
];
