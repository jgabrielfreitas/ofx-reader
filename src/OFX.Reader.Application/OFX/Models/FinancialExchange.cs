using System.Collections.Generic;

namespace OFX.Reader.Application.OFX.Models {

    public sealed class FinancialExchange {

        public string FileId { get; set; }
        public int BankId { get; set; }
        public string AccountId { get; set; }
        public List<Transaction> TransactionCollection { get; } = new List<Transaction>();

    }

}