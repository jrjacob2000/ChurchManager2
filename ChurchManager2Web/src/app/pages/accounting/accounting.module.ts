import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatExpansionModule} from '@angular/material/expansion';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';


import { AccountingRoutingModule } from './accounting-routing.module';
import { AccountChartComponent } from './account-chart/account-chart.component';
import { AccountChartModalComponent } from './account-chart-modal/account-chart-modal.component';
import { CreateTransactionComponent } from './create-transaction/create-transaction.component';
import { TransactionGridComponent } from './transaction-grid/transaction-grid.component';
import { EditTransactionComponent } from './edit-transaction/edit-transaction.component';
import { TransactionModalComponent } from './transaction-grid/transaction-modal/transaction-modal.component';



@NgModule({
  declarations: [
    AccountChartComponent, 
    AccountChartModalComponent, 
    CreateTransactionComponent, 
    TransactionGridComponent,
    TransactionModalComponent,
    EditTransactionComponent
  ],
  imports: [
    CommonModule,
    MatExpansionModule,
    MatInputModule,
    MatTableModule,
    MatDialogModule,
    MatFormFieldModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule, 
    FormsModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    AccountingRoutingModule,
    MatPaginatorModule,
    MatSortModule
  ],
  entryComponents: [
    AccountChartModalComponent,
    TransactionModalComponent
  ],
})
export class AccountingModule { }
