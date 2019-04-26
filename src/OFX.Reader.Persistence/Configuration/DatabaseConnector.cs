namespace OFX.Reader.Persistence.Configuration {

    public sealed class DatabaseConnector {

        private readonly DatabaseSettings _settings;
        
        public DatabaseConnector(DatabaseSettings settings) => this._settings = settings;
        
        public string GetConnectionString() => $"Server={this._settings.Host}," +
                                               $"{this._settings.Port};" +
                                               $"Database={this._settings.Database};" +
                                               $"User Id={this._settings.User};" +
                                               $"Password={this._settings.Password};" +
                                               $"{this._settings.Configurations}";

    }

}