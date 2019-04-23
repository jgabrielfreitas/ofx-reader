using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OFX.Reader.Application.Interfaces.Infrastructure;
using OFX.Reader.Application.OFX.Models;

namespace OFX.Reader.Application.OFX.Commands.Create {

    public sealed class CreateOFXCommandHandler : IRequestHandler<CreateOFXCommand> {

        private readonly IOFXFileReader _ofxFileReader;
        public CreateOFXCommandHandler(IOFXFileReader ofxFileReader) {
            this._ofxFileReader = ofxFileReader;
        }

        public async Task<Unit> Handle(CreateOFXCommand request, CancellationToken cancellationToken) {

            FinancialExchange financialExchange = this._ofxFileReader.Parse("");
            
            return Unit.Value;
        }

    }

}