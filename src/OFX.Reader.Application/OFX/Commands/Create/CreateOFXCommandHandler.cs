using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace OFX.Reader.Application.OFX.Commands.Create {

    public sealed class CreateOFXCommandHandler : IRequestHandler<CreateOFXCommand> {

        public async Task<Unit> Handle(CreateOFXCommand request, CancellationToken cancellationToken) {
            
            
            
            return Unit.Value;
        }

    }

}