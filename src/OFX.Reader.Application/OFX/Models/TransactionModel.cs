using System;

namespace OFX.Reader.Application.OFX.Models {

    public sealed class TransactionModel {

        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public int TransactionId { get; set; }
        public string TransactionDescription { get; set; }

    }

}