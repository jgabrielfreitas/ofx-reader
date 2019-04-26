using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OFX.Reader.Application.Interfaces.Infrastructure;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Application.OFX.Models;
using OFX.Reader.Common;
using OFX.Reader.Domain.Entities;

namespace OFX.Reader.Application.OFX.Commands.Create {

    public sealed class CreateOFXCommandHandler : IRequestHandler<CreateOFXCommand, FinancialExchangeModel> {

        private readonly IOFXFileReader _ofxFileReader;
        private readonly ITransactionRepository _transactionRepository;
        
        public CreateOFXCommandHandler(IOFXFileReader ofxFileReader, ITransactionRepository transactionRepository) {
            this._ofxFileReader = ofxFileReader;
            this._transactionRepository = transactionRepository;
        }

        public async Task<FinancialExchangeModel> Handle(CreateOFXCommand request, CancellationToken cancellationToken) {
            
            FinancialExchangeModel financialExchange = this._ofxFileReader.Parse(request.OFXFileName);

            if (financialExchange == null) {
                //todo: throw exception
                return null;
            }

            long[] queryResult = await this._transactionRepository
                .GetTransactionsById(financialExchange.BankId, financialExchange.TransactionCollection
                    .Select(t => t.TransactionId)
                    .ToArray());

            IEnumerable<TransactionModel> transactionsToBePersisted = financialExchange.TransactionCollection
                .ExceptWhere(t => queryResult.Contains(t.TransactionId)).ToList();

            if (!transactionsToBePersisted.Any()) {
                //todo: validate
                return null;
            }
            
            List<TransactionEntity> transactionEntityCollection = transactionsToBePersisted
                .Select(transactionModel => new TransactionEntity {
                    BankId = financialExchange.BankId,
                    FileId = financialExchange.FileId,
                    AccountId = financialExchange.AccountId,
                    TransactionId = transactionModel.TransactionId,
                    TransactionDate = transactionModel.TransactionDate,
                    TransactionType = transactionModel.TransactionType,
                    TransactionDescription = transactionModel.TransactionDescription
                }).ToList();

            int insertResult = await this._transactionRepository.Create(transactionEntityCollection);

            if (insertResult == 0) {
                //todo: throw exception
            }

            if (insertResult != transactionsToBePersisted.Count()) {
                //todo: validate
            }
            
            financialExchange.TransactionCollection.Clear();
            financialExchange.TransactionCollection.AddRange(transactionsToBePersisted);

            return financialExchange;

        }
        
    }

}