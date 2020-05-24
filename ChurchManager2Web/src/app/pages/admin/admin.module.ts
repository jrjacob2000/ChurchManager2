import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { ListComponent } from './list/list.component';
import { RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { UserModalComponent } from './user-modal/user-modal.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ListComponent, 
    UserModalComponent
  ],
  imports: [
    MatTableModule,
    MatDialogModule ,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatIconModule,
    CommonModule,
    AdminRoutingModule,
    RouterModule,
    FormsModule
    
  ],
  entryComponents: [
    UserModalComponent
  ],
})
export class AdminModule { }
