using System.Collections.Generic;
using System.Threading.Tasks;
using OFX.Reader.Domain.Entities;

namespace OFX.Reader.Application.Interfaces.Persistence {

    public interface ITransactionRepository {

        Task<int[]> GetTransactionsById(int[] transactionIdCollection);

        Task<int> Create(List<TransactionEntity> transactionEntityCollection);

    }

}