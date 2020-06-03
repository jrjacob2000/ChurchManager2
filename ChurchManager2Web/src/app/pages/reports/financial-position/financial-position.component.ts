
import { Component, OnInit, Renderer2 } from '@angular/core';
import { ReportService } from 'src/app/utils/services/report/report.service';
import { IncomeStatement, FinancialPosition } from 'src/app/utils/models/report';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-financial-position',
  templateUrl: './financial-position.component.html',
  styleUrls: ['./financial-position.component.scss']
})
export class FinancialPositionComponent implements OnInit {


  isLoadingResults = false;
  colHeader : any
  netAssets : any; 
  data: FinancialPosition;
  _object = Object;

  public selectedDateFrom : Date = null;
  public selectedDateTo : Date = null;

  constructor(private renderer: Renderer2,
    private reportService : ReportService) { }

  ngOnInit(): void {
    this.loadData()
  }

  loadData()
  {
    this.data = null;
  
      this.isLoadingResults = true;
      this.reportService.getFinancialPositionReport((data : FinancialPosition) => {
        
        if(data)
        {
          this.data = data;
          if(this.data.assets && this.data.assets.length > 0)
            this.colHeader = this.data.assets[0];
          else if (this.data.liabilities && this.data.liabilities.length > 0)
            this.colHeader = this.data.liabilities[0];

          this.netAssets = data.netAssets[0];
        }
        this.isLoadingResults = false;
      });
    
  }

  getAssetSum(column) : number {
    let sum : number = 0;
    for(let i = 0; i < this.data.assets.length; i++) {
      sum += +this.data.assets[i][column]; 
    }
    return sum;
  }

  getLiabilitySum(column) : number {
    let sum : number = 0;
    for(let i = 0; i < this.data.liabilities.length; i++) {
      sum += +this.data.liabilities[i][column]; 
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
