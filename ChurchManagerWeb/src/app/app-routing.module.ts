import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './modules/auth/guard/auth.guard';


const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  {path: 'login', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)},
  {path: 'members', canActivate: [AuthGuard] , loadChildren: () => import('./modules/pages/pages.module').then(m => m.PagesModule)}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
