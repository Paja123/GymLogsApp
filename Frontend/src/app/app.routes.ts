import { Routes } from '@angular/router';
import { TrainingForm } from './features/training/pages/training-form/training-form';
import { TrainingSessionsListComponent } from './features/training/components/training-sessions-list/training-sessions-list.component';
import { MonthlyReportComponent } from './features/training/components/monthly-report/monthly-report.component';

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
        path: 'all-trainings',
        component: TrainingSessionsListComponent
    },
    {
    path: 'monthly-report',
    component: MonthlyReportComponent
    }
];
