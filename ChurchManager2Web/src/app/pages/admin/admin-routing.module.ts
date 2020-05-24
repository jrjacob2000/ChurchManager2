import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';
import { AuthGuard } from 'src/app/utils/guards/auth.guard';
import { LayoutComponent } from 'src/app/layout/layout.component';


const routes: Routes = [
  {
    path: '',
    component: LayoutComponent, canActivate: [AuthGuard],
      canActivateChild: [AuthGuard],
          children: [
            {path: 'list',component: ListComponent,canActivate: [AuthGuard]},  
            {path: '',component: ListComponent,canActivate: [AuthGuard]},         
          ]
  }
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
