using System;

namespace OFX.Reader.Domain.Entities {

    public sealed class TransactionEntity {

        public long Id { get; set; }
        public int BankId { get; set; }
        public string AccountId { get; set; }
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDescription { get; set; }
        public string FileId { get; set; }

        public string TransactionIdHash { get; set; }

    }

}