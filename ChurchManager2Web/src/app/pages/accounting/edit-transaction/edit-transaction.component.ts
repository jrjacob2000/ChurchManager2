import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { FormControl, FormGroup, Validators, FormBuilder, ValidatorFn, ValidationErrors, AbstractControl, FormArray } from '@angular/forms';
import { CustomValidators } from 'src/app/utils/models/custom-validators';
import { AccountChartService } from 'src/app/utils/services/accountChart/account-chart.service';
import { Transaction } from 'src/app/utils/models/transaction';
import { TransactionService } from 'src/app/utils/services/transaction/transaction-service';
import { TransactionSplit } from 'src/app/utils/models/transaction-split';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-transaction',
  templateUrl: './edit-transaction.component.html',
  styleUrls: ['./edit-transaction.component.scss'],
})
export class EditTransactionComponent implements OnInit {

  @ViewChild(MatTable,{static:true}) table: MatTable<any>;
  @ViewChild('transactionForm', { static: false }) transactionForm;
  
  public transHeaderForm : FormGroup;   
  public transaction: Transaction = new Transaction();
  public currentBalance:number = 0;

  dataSource: MatTableDataSource<any>;
  displayedColumns = ['account', 'fund', 'percent','amount', 'action']


  public accountOptions = []
  public fundOptions = []
  public registerOptions = []

  public get transactionLines() {
    return this.transHeaderForm.get('transactionLines') as FormArray;
  }

  constructor(
    public formBuilder: FormBuilder, 
    private accountChartService : AccountChartService,
    private transactionService : TransactionService,
    private route: ActivatedRoute,
    private router: Router) { 
    
  }

  getParameterId() : string
  {
    var transactionId = this.route.snapshot.paramMap.get('id');
    return transactionId;
  }

  ngOnInit(): void {
    this.accountChartService.getAccountRegisters((x) =>{
      this.registerOptions = x.map(r =>{
        return {value : r.Key, displayText : r.Value};
      });
    });

    this.accountChartService.getAccountFunds((x) =>{
      this.fundOptions = x.map(r =>{
        return {value : r.Key, displayText : r.Value};
      });
    });

    this.accountChartService.getAccounts((x) =>{
      this.accountOptions = x.map(r =>{
        return {value : r.Key, displayText : r.Value};
      });
    });

    this.transHeaderForm = this.formBuilder.group(
      {
        'register': ["",Validators.required],
        'deposit': "",
        'payment': "",
        'transactionDate': ["",Validators.required],
        'comment':"",
        'payee': ["",Validators.required],
        transactionLines: this.formBuilder.array([])
      },
      { 
        validator: CustomValidators.requiredEither('deposit', 'payment')}
      );

      this.loadData();
  }


  loadData(){

    let id = this.getParameterId();
    this.transactionService.getById(id, (data) => {
      this.transaction.id = id;
      this.transaction.accountRegisterId = data.accountRegisterId;
      this.transaction.transactionDate = data.transactionDate;
      this.transaction.comment = data.comment;
      this.transaction.deposit = data.deposit;
      this.transaction.payee = data.payee;
      this.transaction.payment = data.payment;
    
   
      data.transactionLines.forEach(x =>
      {
            let row = this.formBuilder.group({
              account: [x.accountId,Validators.required],
              fund: [x.fundId,Validators.required],
              amount: [x.amount,Validators.required],
              percent: (x.amount/this.getTotalAmount()) * 100
            });

          this.transactionLines.push(row);
      });
      this.table.renderRows();
    });
        
  }

  createItem(): FormGroup {

    return this.formBuilder.group({
      account: ["",Validators.required],
      fund: ["",Validators.required],
      amount: ["",Validators.required],
      percent: ""
    });
  }

  //** Function to save the record */
  public onSubmit()
  {
    this.calculateBalance();
    this.transHeaderForm.markAllAsTouched();
      
    if(this.transHeaderForm.valid && this.currentBalance == 0)
    {  
      let valueFromForm = this.transHeaderForm.value;
      let id = this.getParameterId();
      
      let trans: Transaction = {
        id : id,
        accountRegisterId : valueFromForm.register,
        transactionDate : valueFromForm.transactionDate,
        payee : valueFromForm.payee,
        payment : valueFromForm.payment,
        deposit : valueFromForm.deposit,
        comment : valueFromForm.comment,
        transactionLines : valueFromForm.transactionLines.map(x => {
            return ({
              accountId : x.account,
              fundId : x.fund,
              amount : x.amount
            } as TransactionSplit);
        })
      };                    
      this.transactionService.update(trans,() => {
        console.log(trans.transactionDate);
        this.router.navigateByUrl('/accounting/transactions/grid');
      });
    }
    else
      console.log("transHeaderForm is invalid");
    
   
  }
  public paymentChanged(event)
  {
    this.transaction.deposit = null;
    this.setFirstRowAmount(event);
  }
  public depositChanged(event)
  {
    this.transaction.payment = null;
    this.setFirstRowAmount(event);
  }

  public amountChanged(event,element)
  {
    let value = event.target.value;
    let totalValue = this.getTotalAmount();

    if(totalValue > 0)
    {
      let percent = (value/totalValue) * 100;

      this.calculateBalance();
      element.get('percent').setValue(percent); 
    }     
  }

 

  private setFirstRowAmount(event) {
    if(this.transactionLines.controls.length == 1)
    {
        let amt:number = Number(event.target.value);
        let percent = 100;
        (this.transactionLines.controls[0].get('amount') as FormControl).setValue(amt);
        (this.transactionLines.controls[0].get('percent') as FormControl).setValue(percent);
    }
  }

  public addrow()
  {
    this.transactionLines.push(this.createItem());
    this.table.renderRows();

  }

  public deleteRow(item){
    this.transactionLines.removeAt(item);
    this.table.renderRows();
    this.calculateBalance();  
  }

  public percentChanged(event,element)
  {
    let percent = event.target.value;
    let totalValue = this.getTotalAmount();

    let value = (percent / 100) * totalValue;

    element.get('amount').setValue(value); 
    this.calculateBalance();        
  }

  public calculateBalance()
  {
    
    const sum = this.transactionLines.value 
      .map(item => item.amount )
      .reduce((prev, curr) => {
          let val1 = 0;
          let val2 = 0;
          if(prev)
            val1 = parseFloat(prev);
          if(curr)
          val2 = parseFloat(curr);
          return (val1  + val2);
        });

        this.currentBalance = this.getTotalAmount() - sum;
  }

  private getTotalAmount():number
  {
    let totalValue = 0;
    if(this.transaction.payment)
    {
      totalValue = this.transaction.payment
    }
    else if(this.transaction.deposit)
    {
      totalValue = this.transaction.deposit
    }
    return totalValue;
  }
}
