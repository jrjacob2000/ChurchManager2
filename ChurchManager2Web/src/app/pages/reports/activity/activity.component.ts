import { Component, OnInit, Renderer2 } from '@angular/core';
import { ReportService } from 'src/app/utils/services/report/report.service';
import { IncomeStatement } from 'src/app/utils/models/report';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.scss']
})
export class ActivityComponent implements OnInit {

  isLoadingResults = false;
  colHeader : any
  beginningBalance : any;
  endingBalance: any;
  data: IncomeStatement;
  _object = Object;

  public selectedDateFrom : Date = null;
  public selectedDateTo : Date = null;
  public filterForm: FormGroup;

  constructor(private renderer: Renderer2,
    private reportService : ReportService) { }

  ngOnInit(): void {
    this.filterForm = new FormGroup({
      dateFromPicker: new FormControl('',[Validators.required]),
      dateToPicker: new FormControl('',[Validators.required]),
    });
  }

  loadData()
  {
    this.data = null;
    if (this.filterForm.valid)
    {
      this.isLoadingResults = true;
      this.reportService.getActivityReport(this.selectedDateFrom,this.selectedDateTo,(data) => {
        
        if(data)
        {
          this.data = data;
          if(this.data.incomes && this.data.incomes.length > 0)
            this.colHeader = this.data.incomes[0];
          else if (this.data.expenses && this.data.expenses.length > 0)
            this.colHeader = this.data.expenses[0];

          this.beginningBalance = data.netAssetBeginningOfPeriod[0];
          this.endingBalance = data.netAssetEndOfPeriod[0];
        }
        this.isLoadingResults = false;
      });
    }
  }

  getIncomeSum(column) : number {
    let sum : number = 0;
    for(let i = 0; i < this.data.incomes.length; i++) {
      sum += +this.data.incomes[i][column]; 
    }
    return sum;
  }

  getExpenseSum(column) : number {
    let sum : number = 0;
    for(let i = 0; i < this.data.expenses.length; i++) {
      sum += +this.data.expenses[i][column]; 
    }
    return sum;  
  }

  checkIfNumber(value)
  {
    return !isNaN(+value);
  }

  public printReport() : void
  {
    this.renderer.removeClass(document.body, 'sidebar-open');
    this.renderer.addClass(document.body, 'sidebar-collapse');
    window.print();
  }

}
