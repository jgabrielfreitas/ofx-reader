namespace OFX.Reader.Persistence.Configuration {

    public sealed class DatabaseSettings {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Configurations { get; set; }
    }

}