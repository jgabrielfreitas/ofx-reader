using System;
using System.Collections.Generic;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Domain.Entities;
using OFX.Reader.Persistence.Configuration;
using Xunit;

namespace OFX.Reader.Persistence.Tests {

    public class TransactionRepositoryTests {

        private readonly ITransactionRepository _transactionRepository;

        public TransactionRepositoryTests() => this._transactionRepository = new TransactionRepository(new DatabaseConnector(this.InitDatabaseSettings()));

        [Fact]
        public void CreateTest() {
            
            List<TransactionEntity> transactionEntityCollection = new List<TransactionEntity>();

            for (int i = 0; i < 3; i++) {
                TransactionEntity transactionEntity = new TransactionEntity {
                    Id = 0,
                    BankId = 1,
                    AccountId = $"000{i}-0",
                    FileId = "file_id",
                    TransactionId = i,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "OTHER",
                    TransactionDescription = "transaction_description"
                };
                
                transactionEntityCollection.Add(transactionEntity);
            }

            var result = this._transactionRepository.Create(transactionEntityCollection).Result;
            
            Assert.True(result > 0);
        }

        [Fact]
        public void GetTransactionsByIdTest() {
            
            int[] transactionIdCollection = new int[1];

            var result = this._transactionRepository.GetTransactionsById(transactionIdCollection).Result;
            
            Assert.True(result.Length == 0);
        }

        private DatabaseSettings InitDatabaseSettings() =>
            new DatabaseSettings {};

    }

}