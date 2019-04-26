using System.Collections.Generic;
using System.Threading.Tasks;
using OFX.Reader.Domain.Entities;

namespace OFX.Reader.Application.Interfaces.Persistence {

    public interface ITransactionRepository {

        Task<long[]> GetTransactionsById(int bankId, string[] transactionIdCollection);

        Task<int> Create(List<TransactionEntity> transactionEntityCollection);

    }

}