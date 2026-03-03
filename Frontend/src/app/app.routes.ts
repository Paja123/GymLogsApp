import { Routes } from '@angular/router';
import { TrainingForm } from './features/training/pages/training-form/training-form';
import { TrainingSessionsListComponent } from './features/training/components/training-sessions-list/training-sessions-list.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'create',
        pathMatch: 'full'
    },
    {
        path: 'create',
        component: TrainingForm
    },
    {
        // path: 'list',
        // loadComponent: () => import('./features/training/components/training-sessions-list/training-sessions-list.component')
        // .then(m => m.TrainingSessionsListComponent)
        path: 'all',
        component: TrainingSessionsListComponent
    }
];
