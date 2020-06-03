import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { BlankComponent } from './pages/blank/blank.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { RegisterComponent } from './register/register.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AuthGuard } from './utils/guards/auth.guard';
import { NonAuthGuard } from './utils/guards/non-auth.guard';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent, canActivate: [AuthGuard],
      canActivateChild: [AuthGuard],
          children: [
            {
              path: 'profile',
              component: ProfileComponent
            },
            {
              path: 'blank',
              component: BlankComponent
            },
            {
              path: '',
              component: DashboardComponent
            }
          ]
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [NonAuthGuard]
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [NonAuthGuard]
  },
  {path: 'admin', loadChildren: () => import('./pages/admin/admin.module').then(m => m.AdminModule)},
  {path: 'accounting', loadChildren: () => import('./pages/accounting/accounting.module').then(m => m.AccountingModule)},
  {path: 'reports', loadChildren: () => import('./pages/reports/reports.module').then(m => m.ReportsModule)},
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
