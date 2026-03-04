import { Routes } from '@angular/router';
import { TrainingForm } from './features/training/pages/training-form/training-form';
import { TrainingSessionsListComponent } from './features/training/components/training-sessions-list/training-sessions-list.component';
import { MonthlyReportComponent } from './features/training/components/monthly-report/monthly-report.component';
import { LoginComponent } from './features/auth/login/login.component/login.component';
import { RegisterComponent } from './features/auth/register/register.component/register.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'login', component: LoginComponent, data: { hideNavbar: true }},
    {
        path: 'register', component: RegisterComponent, data: { hideNavbar: true }},
    {
        path: 'create', component: TrainingForm, canActivate: [authGuard]},
    {
        path: 'all-trainings', component: TrainingSessionsListComponent, canActivate: [authGuard]},
    {
        path: 'monthly-report', component: MonthlyReportComponent, canActivate: [authGuard]} 
];
