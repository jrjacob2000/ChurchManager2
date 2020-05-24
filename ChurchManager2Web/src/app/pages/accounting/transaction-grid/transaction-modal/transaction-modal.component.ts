import { Component, OnInit, Optional, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AccountChartService } from 'src/app/utils/services/accountChart/account-chart.service';
import { AccountChart } from 'src/app/utils/models/account-chart';
import { AccountChartType } from 'src/app/utils/models/account-chart-type';
import { Transaction } from 'src/app/utils/models/transaction';
import { TransactionService } from 'src/app/utils/services/transaction/transaction-service';

@Component({
  selector: 'transaction-modal',
  templateUrl: './transaction-modal.component.html',
  styleUrls: ['./transaction-modal.component.scss']
})
export class TransactionModalComponent implements OnInit {
  public action:string;
  public local_data:any;
  public loading = false;
  public modalTitle:string;

  constructor(
    public dialogRef: MatDialogRef<TransactionModalComponent>
    //@Optional() is used to prevent error if no data is passed
    , @Optional() @Inject(MAT_DIALOG_DATA) public data: AccountChart
    , private transactionService : TransactionService
    , private toastr: ToastrService) {
          this.local_data = {...data};
          this.action = this.local_data.action;
          this.modalTitle = `${ this.local_data.action }`; 
          console.log(this.local_data); 
    }

  ngOnInit(): void {
    
  }

  doAction(){
     if(this.action == 'Close')
    {
        console.log(this.local_data)
        this.transactionService.close(this.data.id,true,
          (data) =>{
            this.dialogRef.close({event:this.action,data:this.local_data});
          });
    }
    else if(this.action == 'Delete')
    {
      this.transactionService.delete(this.data.id,
        () =>
        {
          this.dialogRef.close({event:this.action,data:this.local_data});
        });     
    }
    
  }

  closeDialog(){
    this.dialogRef.close({event:'Cancel'});
  }

}
