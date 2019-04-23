using System.Collections.Generic;

namespace OFX.Reader.Application.OFX.Models {

    public sealed class FinancialExchangeModel {

        public string FileId { get; set; }
        public int BankId { get; set; }
        public string AccountId { get; set; }
        public List<TransactionModel> TransactionCollection { get; } = new List<TransactionModel>();

    }

}