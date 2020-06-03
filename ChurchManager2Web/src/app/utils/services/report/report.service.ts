import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environments/environment';
import { Transaction, TransactionGridResult, TransactionGridFilter } from '../../models/transaction';
import { Observable } from 'rxjs';
import { IncomeStatement, FinancialPosition } from '../../models/report';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private url = environment.apiEndPoint + "/reports";
    
  constructor(private httpClient: HttpClient,private toastr: ToastrService) { }

  getActivityReport(dateFrom:Date, dateTo:Date, callback: Function): void {
    
    const requestUrl = `${this.url}/activities`;
    
    const param = {
      DateFrom : dateFrom,
      DateTo : dateTo
    }

    this.httpClient.post<IncomeStatement>(requestUrl,param).subscribe((data : IncomeStatement) =>
    {
      if(callback)
          callback(data);
        return;      
    },
    (error) =>{
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Failed retrieving report');
        if(callback)
          callback(null);
      }
    });    
  }

  getFinancialPositionReport(callback: Function): void {
    
    const requestUrl = `${this.url}/financialPosition`;
    
  
    this.httpClient.get<FinancialPosition>(requestUrl).subscribe((data : FinancialPosition) =>
    {
      if(callback)
          callback(data);
        return;      
    },
    (error) =>{
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Failed retrieving report');
        if(callback)
          callback(null);
      }
    });    
  }
}
