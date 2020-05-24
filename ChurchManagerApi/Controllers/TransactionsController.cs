using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChurchManagerApi.Data;
using ChurchManagerApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace ChurchManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("grid")]
        public async Task<IActionResult> GetGrid(GridFilter filter)
        {
            Guid? acctRegisterId = filter.AccountRegisterId; 
            DateTime? datefrom = filter.DateFrom;
            DateTime? dateto = filter.DateTo;
            string sort = filter.Sort;
            string order = filter.Order;
            int pagesize = filter.PageSize;
            int page = filter.Page;

            if (page == 0)
                return base.BadRequest("invalid page");

            int totalRecord = 0;

            var tlist = await GetQuery(acctRegisterId,datefrom,dateto, sort, order, pagesize, page, out totalRecord);
            var result = new TransactionGridResult()
            {
                RecordCount = totalRecord,
                Items = tlist
            };

            return base.Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {

            if (id == null)
            {
                return BadRequest("id is required");
            }
            Transaction transaction = await _context.Transactions.Include("TransactionLines")
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }

            var tranLines = transaction.TransactionLines;

            var transview = new Transaction();
            transview.Id = transaction.Id;
            transview.TransactionDate = DateTime.SpecifyKind( transaction.TransactionDate, DateTimeKind.Utc);
            transview.Payee = transaction.Payee;
            transview.Comment = transaction.Comment;
            transview.AccountRegisterId = transaction.AccountRegisterId;
            transview.TransactionLines = transaction.TransactionLines
                                .Where(y => y.AccountId != transaction.AccountRegisterId)
                                .Select(x => new TransactionLine()
                                {
                                    Id = x.Id,
                                    TransactionId = x.TransactionId,
                                    AccountId = x.AccountId,
                                    FundId = x.FundId,
                                    Amount = x.Amount > 0 ? x.Amount : x.Amount * -1
                                }).ToList();



            var amount = tranLines.Where(x => x.AccountId == transaction.AccountRegisterId).Sum(s => s.Amount);
            if (amount > 0)
                transview.Deposit = amount;
            else
                transview.Payment = amount * -1; //diplay as positive

         

            return base.Ok(transview);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> GetDelete(Guid id)
        {

            if (id == null)
            {
                return BadRequest("id is required");
            }
            Transaction transaction = await _context.Transactions.Include("TransactionLines")
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();


            return base.Ok();
        }


        [HttpPost("close/{id}")]
        public async Task<IActionResult> CloseTransaction(Guid id, bool isclosed)
        {

            if (id == null)
            {
                return BadRequest("id is required");
            }
            Transaction transaction = await _context.Transactions
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.IsClosed = isclosed;
            transaction.DateLastEdited = DateTime.Now;
            transaction.EditedBy = this.User.GetUserId();

            _context.Update(transaction);
            _context.SaveChanges();


            return base.Ok();
        }


        [HttpPost]
        public async Task<ActionResult<Transaction>> TransactionUpsert(Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("parameter cannot be null");

            if (!transaction.Payment.HasValue && !transaction.Deposit.HasValue)
                return BadRequest("Either Payment or Deposite is required");

            var result = await Upsert(transaction);

            if (result.Errors.Count > 0)
                return BadRequest(result.Errors);

            return result.Data;
        }

        private Task<List<TransactionGrid>> GetQuery(Guid? acctRegisterId, DateTime? datefrom, DateTime? dateto,  string sort, string order, int pagesize, int page, out int recordCount)
        {
            var transviewquery = (from t in _context.Transactions
                                  join tl in _context.TransactionLines on t.Id equals tl.TransactionId
                                  join a in _context.AccountCharts on tl.AccountId equals a.Id
                                  join f in _context.AccountCharts on tl.FundId equals f.Id
                                  where tl.AccountId != t.AccountRegisterId &&
                                  (t.AccountRegisterId == acctRegisterId || acctRegisterId == null) &&
                                  (((t.TransactionDate >= datefrom || datefrom == null)&& (t.TransactionDate <= dateto || dateto == null)) )
                                  group new { t, tl, a, f } by new { Id = t.Id } into tgrp
                                  select new TransactionGrid()
                                  {
                                      Id = tgrp.Key.Id.Value,
                                      TransactionDate = tgrp.Max(x => x.t.TransactionDate),
                                      Payee = tgrp.Max(x => x.t.Payee),
                                      Comment = tgrp.Max(x => x.t.Comment),
                                      AccountRegisterId = tgrp.Max(x => x.t.AccountRegisterId),
                                      //AccountId = tgrp.Count() > 1 ? (Guid?)null : tgrp.FirstOrDefault().tl.AccountId,
                                      AccountName = tgrp.Count() > 1 ? "- split -" : tgrp.Max(x => x.a.Name),
                                      //AccountFundId = tgrp.Count() > 1 ? (Guid?)null : tgrp.FirstOrDefault().tl.FundId,
                                      FundName = tgrp.Count() > 1 ? "- split -" : tgrp.Max(x => x.f.Name),
                                      Payment = tgrp.Sum(s => s.tl.Amount) > 0 ? tgrp.Sum(s => s.tl.Amount) : (decimal?)null,
                                      Deposit = tgrp.Sum(s => s.tl.Amount) < 0 ? tgrp.Sum(s => s.tl.Amount) * -1 : (decimal?)null,
                                      CreatedDate = tgrp.Max(x => x.t.DateEntered),
                                      IsClosed = tgrp.Max(x => x.t.IsClosed ? "true" : "false")
                                  });


            Expression<Func<TransactionGrid, object>> sortExpression;
            sortExpression = (o => o.TransactionDate);
            if (string.IsNullOrEmpty(order))
                order = "asc";

            switch (sort.ToLower())
            {
                case "transactiondate":
                    sortExpression = (x => x.TransactionDate);
                    break;
                case "payee":
                    sortExpression = x => x.Payee;
                    break;
                case "accountname":
                    sortExpression = x => x.AccountName;
                    break;
                case "fundname":
                    sortExpression = (x => x.FundName);
                    break;
                default:
                    break;

            }

            recordCount = transviewquery.Count();

            if (order.ToLower() == "desc")
                transviewquery = transviewquery.OrderByDescending(sortExpression);
            else
                transviewquery = transviewquery.OrderBy(sortExpression);

            var queryResult = transviewquery
               .Skip(pagesize * (page - 1))
               .Take(pagesize)
               .ToList();


            queryResult.ForEach(x => {
                x.TransactionDate = DateTime.SpecifyKind(x.TransactionDate, DateTimeKind.Utc);
            });

            return Task.FromResult<List<TransactionGrid>>(queryResult);
        }

        private async Task<UpsertResult> Upsert(Transaction transactionParam )
        {
            var errors = new List<string>();
            //TODO:Add validation for payment its there's still a balance in fund or register account.
            var transactionId = transactionParam.Id;
            Transaction transaction;
            if (transactionId != null && transactionId != Guid.Empty)
            {
                transaction = await _context.Transactions.Include("TransactionLines")
                    .Where(x => x.Id == transactionId).FirstOrDefaultAsync();
                if (transaction == null)
                {
                    return null;
                }

                transaction.DateLastEdited = DateTime.Now;
                transaction.EditedBy = this.User.GetUserId();
            }
            else
            {
                if (transactionParam.TransactionLines.Count() == 0)
                    errors.Add("At least 1 line is required");
                transaction = new Transaction();
                transaction.Id = Guid.NewGuid();
                transaction.DateEntered = DateTime.Now;
                transaction.EnteredBy = this.User.GetUserId(); 
            }

            try
            {

                transaction.AccountRegisterId = transactionParam.AccountRegisterId;
                transaction.TransactionDate = transactionParam.TransactionDate;
                transaction.Comment = transactionParam.Comment;
                transaction.Payee = transactionParam.Payee;
                transaction.IsClosed = false;
                transaction.Deleted = false;
                transaction.Payment = transactionParam.Payment;
                transaction.Deposit = transactionParam.Deposit;
                //TODO: client id
                //transaction.OwnerGroupId = Operator().OwnerGroupId;


                //lines validation
                var lines = new List<TransactionLine>();
                foreach(var item in transactionParam.TransactionLines)              
                {
                    if (transactionParam.Deposit == null && transactionParam.Payment == null)
                        errors.Add("amount cannot be empty");

                    if (item.AccountId == Guid.Empty || transactionParam.AccountRegisterId == Guid.Empty || item.FundId == Guid.Empty)
                        errors.Add("Either Account Registry, Account, or Fund are invalid");

                    bool isDeposit = transactionParam.Deposit.HasValue;
                    Guid creditId = isDeposit ? item.AccountId : transactionParam.AccountRegisterId;
                    Guid debitId = isDeposit ? transactionParam.AccountRegisterId : item.AccountId;
                    decimal amount = item.Amount;

                    var creditAccount =_context.AccountCharts.Find(creditId);
                    var debitAccount = _context.AccountCharts.Find(debitId);

                    decimal assetBal = 0;
                    if (creditAccount.Type == AccountChartTypeEnum.Asset)
                        assetBal = GetAccountBalance(creditId, item.FundId);
                    
                    decimal liabilityBal = 0;
                    if (debitAccount.Type == AccountChartTypeEnum.Liability)
                        liabilityBal = GetAccountBalance(debitId, item.FundId);

                    if (debitAccount.Type == AccountChartTypeEnum.Liability)
                    {
                        if (liabilityBal < amount)
                            errors.Add(string.Format("You are over paying to your {0}. You current balance is only {1}.", debitAccount.Name, liabilityBal));                       
                    }

                    if (creditAccount.Type == AccountChartTypeEnum.Asset )
                    {
                        if (assetBal < amount)
                            errors.Add(string.Format("You only have {0} in your fund in {1}. its not enough for the payment of {2}", assetBal, creditAccount.Name, amount));
                    }


                    if (errors.Count() == 0)
                    {
                        var itemLines = GetDebitCreditPair(transaction.Id.Value, creditId, debitId, item.FundId, amount);
                        lines.AddRange(itemLines);
                    }
                    
                };


                if (ModelState.IsValid && errors.Count() == 0)
                {

                    if (transactionId == null || transactionId == Guid.Empty)
                    {
                        transaction.TransactionLines = lines;
                        _context.Transactions.Add(transaction);
                    }
                    else
                    {
                        transaction.TransactionLines.ToList().ForEach(x =>
                            _context.TransactionLines.Remove(x)
                        );

                        lines.ForEach(x =>
                            x.Id = null
                        ); ;
                        transaction.TransactionLines = lines;
                        
                        _context.Update(transaction);
                    }

                    _context.SaveChanges();
                    //return new UpsertResult() { Data= transaction, Errors = errors} ;
                }
            }
            catch (Exception ex)
            {
                //return null;
                errors.Add(ex.Message);
            }

            return new UpsertResult() { Data = transaction, Errors = errors }; ; 

        }

        private decimal GetAccountBalance(Guid registryAccountId, Guid fundId)
        {
            //Todo: registry account validation: should be assets and liability only

            var query = (from tl in _context.TransactionLines
                         join a in _context.AccountCharts on tl.AccountId equals a.Id
                         join f in _context.AccountCharts on tl.FundId equals f.Id
                         where f.Id == fundId && a.Id == registryAccountId
                         //group new { tl, a, f } by new { Fund = f.Id, Account = a.Id } into grp
                         select a.Type == AccountChartTypeEnum.Liability ? tl.Amount * -1 : tl.Amount).ToList();

            return query == null? 0 : query.Sum();
        }

        private List<TransactionLine> GetDebitCreditPair(Guid transactionId, Guid creditAccountId, Guid debitAccountId, Guid fundId, decimal amount)
        {

            var lines = new List<TransactionLine>() {
                new TransactionLine(){
                    Id =Guid.NewGuid(),
                    TransactionId = transactionId,
                    AccountId = debitAccountId,
                    FundId =fundId,
                    Amount = amount //debit is always positive
                },
                 new TransactionLine(){
                    Id =Guid.NewGuid(),
                    TransactionId = transactionId,
                    AccountId = creditAccountId,
                    FundId = fundId,
                    Amount = amount * -1//credit is always negative
                },
            };

            return lines;
        }

        private class UpsertResult
        {
            internal Transaction Data { get; set; }
            internal List<string> Errors { get; set; }
        }

        public class GridFilter
        {
            public Guid AccountRegisterId { get; set; }
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
            public int PageSize { get; set; }
            public int Page { get; set; }
            public string Sort { get; set; }
            public string Order { get; set; }

        }
    
    }
}
