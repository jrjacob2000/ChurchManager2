import { TransactionSplit } from './transaction-split';

  export class Transaction {
    id : string = null;
    accountRegisterId : string;
    transactionDate : Date = null; 
    payee:string = null;
    payment:number = null;
    deposit:number = null;
    comment:string = null;
    transactionLines : TransactionSplit[] = null;
  }

  export interface TransactionGridResult {
    items: TransactionGridData[];
    recordCount: number;
  }
  
  export interface TransactionGridData {
    transactionDate: string;
    payee: string;
    accountName: string;
    fundName: string;
    payment: number;
    deposit: number;
    isClosed: boolean;
  }

  export class TransactionGridFilter {
    constructor(
      AccountRegisterId: string,
      DateFrom: Date,
      DateTo: Date,
      PageSize: number,
      Page: number,
      Sort: string,
      Order: string,   
    ){
      this.AccountRegisterId= AccountRegisterId;
      this.DateFrom= DateFrom;
      this.DateTo= DateTo;
      this.PageSize= PageSize;
      this.Page= Page;
      this.Sort= Sort;
      this.Order= Order;
    }
      AccountRegisterId: string;
      DateFrom: Date;
      DateTo: Date;
      PageSize: number;
      Page: number;
      Sort: string;
      Order: string;
  }