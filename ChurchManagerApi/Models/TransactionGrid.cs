using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchManagerApi.Models
{
    public class TransactionGrid
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Payee { get; set; }
        public string Comment { get; set; }
        public Guid AccountRegisterId { get; set; }
        public string AccountName { get; set; }
        public string FundName { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Deposit { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IsClosed { get; set; }
    }
}
