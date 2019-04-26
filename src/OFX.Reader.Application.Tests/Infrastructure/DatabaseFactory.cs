using System.Collections.Generic;
using System.Threading.Tasks;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Domain.Entities;

namespace OFX.Reader.Application.Tests.Infrastructure {

    public sealed class DatabaseFactory : ITransactionRepository {

        public async Task<long[]> GetTransactionsById(int bankId, string[] transactionIdCollection) {

            long[] foundIds = new long[5];
            
            for (int i = 0; i < 5; i++) {
                foundIds[i] = i;
            }

            return foundIds;
        }

        public async Task<int> Create(List<TransactionEntity> transactionEntityCollection) {
            return transactionEntityCollection.Count;
        }

    }

}