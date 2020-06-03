using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChurchManagerApi.Data;
using ChurchManagerApi.Models;
using ChurchManagerApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reports
        [HttpPost("activities")]
        public IActionResult GetActivities(ReportFilter filter)
        {
            if (filter.DateFrom == null || filter.DateTo == null)
                return null;

            var dateFrom = filter.DateFrom;
            var dateTo = filter.DateTo;

            if (dateFrom > dateTo)
                return BadRequest("invalid date range");

            var grpId = this.User.GetUserId();
            //TODO:
            //var church = db.Churches.Where(x => x.OwnerGroupId == grpId).FirstOrDefault();

            var reporTitle = new ReportTitle()
            {
                ChurchName = "Maranatha",//church.Name,
                Period = string.Format("Period: {0} - {1}", dateFrom.Value.ToShortDateString(), dateTo.Value.ToShortDateString())
            };

            var resut = new IncomeStatement()
            {
                Incomes = GetReportByType(AccountChartTypeEnum.Income, dateFrom.Value, dateTo.Value),
                Expenses = GetReportByType(AccountChartTypeEnum.Expenses, dateFrom.Value, dateTo.Value),
                NetAssetEndOfPeriod = GetNetAsset(null, dateTo.Value, NetAsset.End),
                NetAssetBeginningOfPeriod = GetNetAsset(null, dateFrom.Value, NetAsset.Begginning),
                ReportTitle = reporTitle
            };

            return Ok(resut);
        }

        [HttpGet("FinancialPosition")]
        public IActionResult FinancialPosition()
        {
            var grpId = this.User.GetUserId();
            //TODO:
            //var church = db.Churches.Where(x => x.OwnerGroupId == grpId).FirstOrDefault();
            var reporTitle = new ReportTitle()
            {
                ChurchName = "Maranatha",
                Period = string.Format("As of: {0}", DateTime.Now.ToShortDateString())
            };

            var result = new FinancialPosition()
            {
                ReportTitle = reporTitle,
                Assets = GetFinancialPositionByType(AccountChartTypeEnum.Asset),
                Liabilities = GetFinancialPositionByType(AccountChartTypeEnum.Liability),
                NetAssets = GetNetAsset(null, null, NetAsset.Net),
            };

            return Ok(result);
        }

        private List<dynamic> GetFinancialPositionByType(AccountChartTypeEnum accountType)
        {
            var query = (from tl in _context.TransactionLines
                         join a in _context.AccountCharts on tl.AccountId equals a.Id
                         join f in _context.AccountCharts on tl.FundId equals f.Id
                         where a.Type == accountType
                         group new { tl, a, f } by new { Fund = f.Id, Account = a.Id } into grp
                         select new
                         {
                             Fund = grp.Max(x => x.f.Name),
                             Account = grp.Max(x => x.a.Name),
                             Amount = grp.Sum(s => s.tl.Amount) > 0 ? grp.Sum(s => s.tl.Amount) : grp.Sum(s => s.tl.Amount) * -1

                         }).ToList();


            var pivotTable = query.ToPivotTable(
                item => item.Fund,
              item => item.Account,
              items => items.Any() ? items.Sum(x => x.Amount) : 0);


            return pivotTable.ToDynamicList();
        }








        private List<dynamic> GetReportByType(AccountChartTypeEnum accountType, DateTime dateFrom, DateTime dateTo)
        {
            

            var transview = (from f in _context.AccountCharts 
                            from line in (from t in _context.Transactions
                                         join tl in _context.TransactionLines on t.Id equals tl.TransactionId
                                         join a in _context.AccountCharts on tl.AccountId equals a.Id
                                         //join f in _context.AccountCharts on tl.FundId equals f.Id
                                         where a.Type == accountType && (t.TransactionDate >= dateFrom && t.TransactionDate <= dateTo)
                                         group new { tl, a } by new { Fund = tl.FundId } into grp
                                         select new
                                         {
                                             Fund = grp.Key.Fund,
                                             //Type = grp.FirstOrDefault().a.Type,
                                             Account = grp.Max(x => x.a.Name),
                                             Amount = grp.Sum(s => s.tl.Amount)

                                         }).Where(x => f.Id == x.Fund).DefaultIfEmpty()
                            where f.Type == AccountChartTypeEnum.FundBalance
                            select new { 
                                Fund = f.Name,
                                Account = line.Account,
                                Amount = line.Amount < 0 ? line.Amount * -1 : line.Amount
                            }).ToList();

            var pivotTable = transview.ToPivotTable(
                item => item.Fund,
              item => item.Account,
              items => items.Any() ? items.Sum(x => x.Amount) : 0);


            return pivotTable.ToDynamicList();

        }

        private List<dynamic> GetNetAsset(DateTime? dateFrom, DateTime? dateTo, NetAsset type)
        {
            string accountName;
            switch (type)
            {
                case NetAsset.Begginning:
                    accountName = "NET ASSETS, BEGGINNING OF PERIOD";
                    break;
                case NetAsset.End:
                    accountName = "NET ASSETS, END OF PERIOD";
                    break;
                default:
                    accountName = "TOTAL NET ASSETS";
                    break;
            }

            var transview = (from t in _context.Transactions
                             join tl in _context.TransactionLines on t.Id equals tl.TransactionId
                             join a in _context.AccountCharts on tl.AccountId equals a.Id
                             join f in _context.AccountCharts on tl.FundId equals f.Id
                             where (a.Type == AccountChartTypeEnum.Asset || a.Type == AccountChartTypeEnum.Liability) &&
                             ((dateFrom == null || t.TransactionDate >= dateFrom) && (dateTo == null || (type == NetAsset.Begginning ? t.TransactionDate < dateTo : t.TransactionDate <= dateTo)))
                             group new { tl, a, f } by new { Fund = f.Id } into grp
                             select new
                             {
                                 Fund = grp.Max(x => x.f.Name),
                                 Account = accountName,
                                 Amount = grp.Sum(s => s.tl.Amount)

                             }).ToList();

            var pivotTable = transview.ToPivotTable(
                item => item.Fund,
              item => item.Account,
              items => items.Any() ? items.Sum(x => x.Amount) : 0);


            return pivotTable.ToDynamicList();

        }
    }
}
