using MediatR;
using OFX.Reader.Application.OFX.Models;

namespace OFX.Reader.Application.OFX.Commands.Create {

    public sealed class CreateOFXCommand : IRequest, IRequest<FinancialExchangeModel> {

        public string OFXFileName { get; set; }

    }

}