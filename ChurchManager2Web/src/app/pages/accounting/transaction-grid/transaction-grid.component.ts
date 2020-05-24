
import {HttpClient} from '@angular/common/http';
import {Component, OnInit, ViewChild, AfterViewInit, EventEmitter, ElementRef} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {merge, Observable, of as observableOf, BehaviorSubject} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { TransactionGridResult, TransactionGridData, TransactionGridFilter } from 'src/app/utils/models/transaction';
import { TransactionService } from 'src/app/utils/services/transaction/transaction-service';
import { ToastrService } from 'ngx-toastr';
import { AccountChartService } from 'src/app/utils/services/accountChart/account-chart.service';
import { MatSelect } from '@angular/material/select';
import { TransactionModalComponent } from './transaction-modal/transaction-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-transaction-grid',
  templateUrl: './transaction-grid.component.html',
  styleUrls: ['./transaction-grid.component.scss']
})

export class TransactionGridComponent implements AfterViewInit,OnInit {
  displayedColumns: string[] = ['transactionDate', 'payee', 'accountName', 'fundName','payment','deposit','action'];
  data: TransactionGridData[] = [];

  resultsLength = 0;
  isLoadingResults = true;
  reload$ : EventEmitter<any> = new EventEmitter();
  searchClicked$ : EventEmitter<any> = new EventEmitter();

  public registerOptions = []
  public selectedRegister : string = '';
  public selectedDateFrom : Date = null;
  public selectedDateTo : Date = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatSelect) select: MatSelect;
  

  constructor(
    private transactionService : TransactionService,
    private toastr: ToastrService,
    private accountChartService : AccountChartService,
    public dialog: MatDialog) {
    }

  ngOnInit(): void {
    
  }

  public search()
  {

    if((this.selectedDateFrom <= this.selectedDateTo) ||
      this.selectedDateFrom || this.selectedDateTo)
      {      
        this.searchClicked$.emit();
      }
  }

  ngAfterViewInit() {
    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.select.valueChange.subscribe(() => this.paginator.pageIndex = 0);

    this.accountChartService.getAccountRegisters((x) =>{      
      this.registerOptions = x.map(r =>{
          return {value : r.Key, displayText : r.Value};
        });

      //default to first option  
      this.selectedRegister = x[0].Key;

      merge(this.sort.sortChange, this.paginator.page, this.searchClicked$, this.reload$)
        .pipe(
          startWith({}),
          switchMap(() => {
            console.log("merge " );
            this.isLoadingResults = true;

            const  filter = new TransactionGridFilter(this.selectedRegister,this.selectedDateFrom, this.selectedDateTo,this.paginator.pageSize,this.paginator.pageIndex, this.sort.active, this.sort.direction)
            return this.transactionService.getGrid(filter );
          }),
          map(data => {
            // Flip flag to show that loading has finished.
            this.isLoadingResults = false;
            this.resultsLength = data.recordCount;

            return data.items;
          }),
          catchError(() => {
            this.toastr.error("Failed to retreive transaction");
            this.isLoadingResults = false;
            return observableOf([]);
          })
        ).subscribe(data => this.data = data);
    });
  }


  openDialog(action,obj) {

    obj.action = action;
    const dialogRef = this.dialog.open(TransactionModalComponent, {
      width: '350px',
      data:obj
    });
 
    dialogRef.afterClosed().subscribe(result => {
       this.reload$.emit();
    });
  }

}



