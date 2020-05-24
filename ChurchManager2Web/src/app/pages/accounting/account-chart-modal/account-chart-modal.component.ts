import { Component, OnInit, Optional, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AccountChartService } from 'src/app/utils/services/accountChart/account-chart.service';
import { AccountChart } from 'src/app/utils/models/account-chart';
import { AccountChartType } from 'src/app/utils/models/account-chart-type';

@Component({
  selector: 'app-account-chart-modal',
  templateUrl: './account-chart-modal.component.html',
  styleUrls: ['./account-chart-modal.component.scss']
})
export class AccountChartModalComponent implements OnInit {
  public action:string;
  public local_data:any;
  public loading = false;
  public modalTitle:string;

  constructor(
    public dialogRef: MatDialogRef<AccountChartModalComponent>
    //@Optional() is used to prevent error if no data is passed
    , @Optional() @Inject(MAT_DIALOG_DATA) public data: AccountChart
    , private accountChartService :  AccountChartService
    , private toastr: ToastrService) {
          this.local_data = {...data};
          this.action = this.local_data.action;
          this.modalTitle = `${ this.local_data.action } ${ AccountChartType[this.local_data.type] }`;  
    }

  ngOnInit(): void {
    
  }

  doAction(){
    if(this.action == 'Add')
    {
        console.log(this.local_data)
        this.accountChartService.createAccountChart(this.local_data as AccountChart,(data) =>
        {
          this.dialogRef.close({event:this.action,data:this.local_data});
        });
        
    }
    else if(this.action == 'Update')
    {
        console.log(this.local_data)
        this.accountChartService.updateAccountChart(this.local_data as AccountChart,
          (data) =>{
            this.dialogRef.close({event:this.action,data:this.local_data});
          });
    }
    else if(this.action == 'Delete')
    {
      this.accountChartService.deleteAccountChart(this.data.id,
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
