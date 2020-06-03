using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchManagerApi.Models.Dto
{
    public class IncomeStatement
    {
        public ReportTitle ReportTitle { get; set; }
        public List<dynamic> Incomes { get; set; }
        public List<dynamic> Expenses { get; set; }

        public List<dynamic> NetAssetEndOfPeriod { get; set; }
        public List<dynamic> NetAssetBeginningOfPeriod { get; set; }
    }
}
