using System.Threading;
using System.Threading.Tasks;
using OFX.Reader.Application.Interfaces.Infrastructure;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Application.OFX.Commands.Create;
using OFX.Reader.Application.Tests.Infrastructure;
using OFX.Reader.Infrastructure.FileManager;
using Xunit;

namespace OFX.Reader.Application.Tests {

    public class CreateOFXCommandHandlerTests {

        private readonly IOFXFileReader _ofxFileReader;
        private readonly ITransactionRepository _transactionRepository;
        private const string FILE_NAME = "extrato_santander.ofx";

        public CreateOFXCommandHandlerTests() {
            this._ofxFileReader = new OFXFileReader(this.InitDirectorySettings());
            this._transactionRepository = new DatabaseFactory();
        }

        [Fact]
        public async Task CreateOFXCommandTest() {
            CreateOFXCommandHandler handler = new CreateOFXCommandHandler(this._ofxFileReader, this._transactionRepository);
            var result = await handler.Handle(new CreateOFXCommand {
                OFXFileName = FILE_NAME
            }, CancellationToken.None);
            
            Assert.True(result.BankId > 0);
            Assert.True(!string.IsNullOrEmpty(result.FileId));
            Assert.True(!string.IsNullOrEmpty(result.AccountId));
            Assert.NotNull(result.TransactionCollection);
        }
        
        private OFXDirectorySettings InitDirectorySettings() => new OFXDirectorySettings();
    }

}