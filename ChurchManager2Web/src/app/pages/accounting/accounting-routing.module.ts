import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from 'src/app/layout/layout.component';
import { AuthGuard } from 'src/app/utils/guards/auth.guard';
import { AccountChartComponent } from './account-chart/account-chart.component';
import { CreateTransactionComponent } from './create-transaction/create-transaction.component';
import { TransactionGridComponent } from './transaction-grid/transaction-grid.component';
import { EditTransactionComponent } from './edit-transaction/edit-transaction.component';


const routes: Routes = [
  {
    path: '',
    component: LayoutComponent, canActivate: [AuthGuard],
      canActivateChild: [AuthGuard],
          children: [
            {path: 'transactions/grid',component: TransactionGridComponent,canActivate: [AuthGuard]},
            {path: 'transactions/create',component: CreateTransactionComponent,canActivate: [AuthGuard]},
            {path: 'transactions/:id',component: EditTransactionComponent,canActivate: [AuthGuard]},
            {path: 'accountchart',component: AccountChartComponent,canActivate: [AuthGuard]},  
            {path: '',component: AccountChartComponent,canActivate: [AuthGuard]},         
          ]
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountingRoutingModule { }
