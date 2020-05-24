import { Injectable, Inject, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountChartService {

  private url = environment.apiEndPoint + "/AccountCharts";
    
  constructor(private httpClient: HttpClient,private toastr: ToastrService) { }

  getAccountCharts(callback: Function) {
    return this.httpClient.get(this.url ).subscribe((data:any[]) =>
    {
      if(callback)
        callback(data);
      return;     
      
    },
    (error) =>{
      this.toastr.error("Failed to retrieve Account Chart");
    });;    
  }

  getAccountRegisters(callback:Function = (data) =>{}):void {
    this.httpClient.get(this.url+"/optionType/register" ).subscribe((data:any[]) =>
    {
      if(callback)
          callback(data);
        return;
      
    },
    (error) =>{
      console.log(error);
      if (error.status !== 401) {
        this.toastr.error("Failed to retrieve Account Registers");
      }
    });    
  }

  getAccounts(callback:Function = (data) =>{}):void {
    this.httpClient.get(this.url+"/optionType/account" )
      .subscribe((data:any[]) =>
      {
        if(callback)
          callback(data);
        return;
        
      },
      (error) =>{
        console.log(error);
        if (error.status !== 401) {
          this.toastr.error("Failed to retrieve Accounts");
        }
      });        
  }
  getAccountFunds(callback:Function = (data) =>{}):void {
    this.httpClient.get(this.url+"/optionType/fund" )
      .subscribe((data:any[]) =>
      {
        if(callback)
          callback(data);
        return;
        
      },
      (error) =>{
        console.log(error);
        if (error.status !== 401) {
          this.toastr.error("Failed to retrieve Funds");
        }
      });       
  }

  createAccountChart(accountChart: any,callback:Function) {
    return this.httpClient.post(this.url , accountChart).subscribe((data:any) =>
    {     
      this.toastr.success("", 'Saving success');
      if(callback)
          callback(data);
        return;   
     
    },
    (error) =>{
      this.toastr.error("Failed to create account chart");
    });

  }

  deleteAccountChart(id: any,callback?:Function) {
    return this.httpClient.delete(this.url + "/" + id).subscribe((data:any) =>
    {     
      this.toastr.success("", 'Delete success');
      if(callback)
          callback(data);
        return;   
     
    },
    (error) =>{
      this.toastr.error("Failed to delete account chart");
    });

  }

  updateAccountChart(accountChart: any,callback?:Function) {
    return this.httpClient.put(this.url +'/' + accountChart.id , accountChart).subscribe((data:any) =>
    {     
      this.toastr.success("", 'Update success');
      if(callback)
          callback(data);
        return;   
     
    },
    (error) =>{
      this.toastr.error("Failed to update account chart");
    });;

  }
}
