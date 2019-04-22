namespace OFX.Reader.Infrastructure.FileManager {

    public sealed class OFXFileOpener {

        private readonly OFXDirectorySettings _settings;
        
        public OFXFileOpener(OFXDirectorySettings settings) => this._settings = settings;

        public void Open(string fileName) {
            
            string path = System.IO.Path.Combine(this._settings.OFXFileDirectory, fileName);
            
            string ofxFileText = System.IO.File.ReadAllText(path);
        }
    }
}