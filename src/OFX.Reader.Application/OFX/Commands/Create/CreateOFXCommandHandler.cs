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

            if (financialExchange == null) return null;

            int[] queryResult = await this._transactionRepository
                .GetTransactionsById(financialExchange.TransactionCollection
                .Select(t => t.TransactionId)
                .ToArray());
            
            List<TransactionEntity> transactionEntityCollection = financialExchange.TransactionCollection
                .ExceptWhere(t => queryResult.Contains(t.TransactionId))
                .Select(transactionModel => new TransactionEntity {
                    BankId = financialExchange.BankId,
                    FileId = financialExchange.FileId,
                    AccountId = financialExchange.AccountId,
                    TransactionId = transactionModel.TransactionId,
                    TransactionDate = transactionModel.TransactionDate,
                    TransactionType = transactionModel.TransactionType,
                    TransactionDescription = transactionModel.TransactionDescription
                }).ToList();

            await this._transactionRepository.Create(transactionEntityCollection);

            return financialExchange;

        }
        
    }

}