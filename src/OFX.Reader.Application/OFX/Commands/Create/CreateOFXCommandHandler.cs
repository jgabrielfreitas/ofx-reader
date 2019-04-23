using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OFX.Reader.Application.Interfaces.Infrastructure;
using OFX.Reader.Application.OFX.Models;

namespace OFX.Reader.Application.OFX.Commands.Create {

    public sealed class CreateOFXCommandHandler : IRequestHandler<CreateOFXCommand, FinancialExchangeModel> {

        private readonly IOFXFileReader _ofxFileReader;
        public CreateOFXCommandHandler(IOFXFileReader ofxFileReader) {
            this._ofxFileReader = ofxFileReader;
        }

        public async Task<FinancialExchangeModel> Handle(CreateOFXCommand request, CancellationToken cancellationToken) {
            
            FinancialExchangeModel financialExchange = this._ofxFileReader.Parse(request.OFXFileName);
            
            //todo: read by id
            
            

            return financialExchange;

        }

    }

}