import { Component, OnInit } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
//import { AccountChartModalComponent } from '../account-chart-modal/account-chart-modal.component';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { AccountChartModalComponent } from '../account-chart-modal/account-chart-modal.component';
import { AccountChartService } from 'src/app/utils/services/accountChart/account-chart.service';
import { AccountChartType } from 'src/app/utils/models/account-chart-type';
import { AccountChart } from 'src/app/utils/models/account-chart';





@Component({
  selector: 'app-account-chart',
  templateUrl: './account-chart.component.html',
  styleUrls: ['./account-chart.component.scss']
})
export class AccountChartComponent implements OnInit {

  public accountType = [0,1,2,3,4];
  public accountTypeName = AccountChartType;
  
  displayedColumns: string[] = ['code', 'name','action'];
  public dataSource: any; 
  constructor(
      private toastr: ToastrService,
      public dialog: MatDialog,
      private accountChartService: AccountChartService) 
    { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(){
    this.accountChartService.getAccountCharts((data) => {
      this.dataSource = data as AccountChart[];
    });
  }

  
  public getDatasource(id)
  {
    if(this.dataSource)
      return this.dataSource.filter(x => x.type == id);
    else
      return null;
  }

  public openDialog(action,obj) {

    // if(action == 'Add')
    //   obj = {type: obj.type, showInRegister: false} as AccountChart;

    obj.action = action;
    const dialogRef = this.dialog.open(AccountChartModalComponent, {
      width: '350px',
      data:obj
    });
 
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    });
  }

}
