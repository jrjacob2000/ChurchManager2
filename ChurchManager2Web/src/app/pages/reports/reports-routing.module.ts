import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from 'src/app/layout/layout.component';
import { AuthGuard } from 'src/app/utils/guards/auth.guard';
import { ActivityComponent } from './activity/activity.component';
import { FinancialPositionComponent } from './financial-position/financial-position.component';



const routes: Routes = [{
  path: '',
  component: LayoutComponent, canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
        children: [     
          {path: 'activity',component: ActivityComponent,canActivate: [AuthGuard]},
          {path: 'financialPosition',component: FinancialPositionComponent,canActivate: [AuthGuard]},                
        ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
