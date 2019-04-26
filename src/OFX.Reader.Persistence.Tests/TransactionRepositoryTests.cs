using System;
using System.Collections.Generic;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Domain.Entities;
using OFX.Reader.Persistence.Configuration;
using Xunit;

namespace OFX.Reader.Persistence.Tests {

    public class TransactionRepositoryTests {

        private readonly ITransactionRepository _transactionRepository;

        public TransactionRepositoryTests() {
            IDatabaseConnector databaseConnector = new DatabaseConnector(this.InitDatabaseSettings());
            this._transactionRepository = new TransactionRepository(databaseConnector);
        }

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
            
            long[] transactionIdCollection = new long[1];

            var result = this._transactionRepository.GetTransactionsById(1, transactionIdCollection).Result;
            
            Assert.True(result.Length > 0);
        }

        private DatabaseSettings InitDatabaseSettings() =>
            new DatabaseSettings {
                Host = "armoredboar.database.windows.net",
                Port = "1433",
                Database = "",
                User = "",
                Password = "",
                Configurations =
                    "Pooling=true;Timeout=60;Connection Lifetime=300;Application Name=OFX.Reader.Persistence.Tests"
            };

    }

}