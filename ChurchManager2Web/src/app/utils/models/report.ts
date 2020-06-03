  export interface IncomeStatement {
    incomes: any;
    expenses: any;
    netAssetEndOfPeriod: any;
    netAssetBeginningOfPeriod: any;
    reportTitle: ReportTitle;
    
  }

  export interface ReportTitle
  {
    churchName : string;
    period : string;
  }

  export interface FinancialPosition
  {
    reportTitle: ReportTitle;
    assets : any;
    liabilities : any;
    netAssets : any;

  }

