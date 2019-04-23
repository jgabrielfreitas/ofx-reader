using System.IO;
using Xunit;
using OFX.Reader.Infrastructure.FileManager;
using OFX.Reader.Infrastructure.FileManager.OFXFile;

namespace OFX.Reader.Infrastructure.Tests.FileManager {

    public class OFXFileReaderTest {

        private readonly OFXDirectorySettings _settings;
        private const string FILE_NAME = "extrato_santander.ofx";
        
        public OFXFileReaderTest() {
            this._settings = this.Init();
        }

        [Fact]
        public void OpenOFXFileTest() {
            
            OFXFileReader ofxFileReader = new OFXFileReader(this._settings);
            OFXDocument fileContent = ofxFileReader.Read(Path.Combine(this._settings.OFXFileDirectory, FILE_NAME));
            
            Assert.NotNull(fileContent);
            Assert.NotNull(fileContent.OFXTransactionCollection);
            Assert.False(string.IsNullOrEmpty(fileContent.ACCTID));
            Assert.False(string.IsNullOrEmpty(fileContent.BANKID));
        }

        private OFXDirectorySettings Init() {
            return new OFXDirectorySettings();
        }
    }
}