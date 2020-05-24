import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environments/environment';
import { Transaction, TransactionGridResult, TransactionGridFilter } from '../../models/transaction';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private url = environment.apiEndPoint + "/Transactions";
    
  constructor(private httpClient: HttpClient,private toastr: ToastrService) { }


  create(transaction:Transaction, callback:Function = (data) =>{}):void {
    this.httpClient.post(this.url,transaction ).subscribe((data:any[]) =>
    {
      this.toastr.success("Transaction created");
      if(callback)
          callback(data);
        return;
      
    },
    (error) =>{
      console.log(error);
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Error!');
      }
    });    
  }

  update(transaction:Transaction, callback?:Function):void {
    if(!transaction.id) return;

    this.httpClient.post(this.url,transaction ).subscribe((data:any[]) =>
    {
      this.toastr.success("Transaction updated");
      if(callback)
          callback(data);
        return;
      
    },
    (error) =>{
      console.log(error);
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Error!');
      }
    });    
  }

  
  getGrid(filter : TransactionGridFilter): Observable<TransactionGridResult> {

    filter.Page = filter.Page + 1;
    const requestUrl = `${this.url}/grid`;

    return this.httpClient.post<TransactionGridResult>(requestUrl,filter);
  }
  

  getById(id:string, callback: Function): void {
    
    const requestUrl = `${this.url}/${id}`;

    this.httpClient.get<Transaction>(requestUrl).subscribe((data : Transaction) =>
    {
      if(callback)
          callback(data);
        return;      
    },
    (error) =>{
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Error!');
      }
    });    
  }

  delete(id: string, callback?:Function):void {

    const requestUrl = `${this.url}/${id}`;

    this.httpClient.delete(requestUrl).subscribe((data:any[]) =>
    {
      this.toastr.success("Transaction deleted");
      if(callback)
          callback(data);
        return;
      
    },
    (error) =>{
      console.log(error);
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Error!');
      }
    });    
  }

  close(id: string, value : boolean, callback?:Function):void {

    const requestUrl = `${this.url}/close/${id}?isClosed=${value}`;

    this.httpClient.post(requestUrl,null).subscribe((data:any[]) =>
    {
      this.toastr.success("Transaction closed");
      if(callback)
          callback(data);
        return;
      
    },
    (error) =>{
      console.log(error);
      if (error.status !== 401) {
        this.toastr.error(error.message, 'Error!');
      }
    });    
  }
}
