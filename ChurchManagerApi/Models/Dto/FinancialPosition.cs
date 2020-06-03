using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchManagerApi.Models.Dto
{
    public class FinancialPosition
    {
        public ReportTitle ReportTitle { get; set; }
        public List<dynamic> Assets { get; set; }
        public List<dynamic> Liabilities { get; set; }

        public List<dynamic> NetAssets { get; set; }

    }
}
