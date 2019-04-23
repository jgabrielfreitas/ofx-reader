namespace OFX.Reader.Infrastructure.FileManager.OFXFile {

    public sealed class OFXTransaction {

        public string TRNTYPE { get; set; }

        public string DTPOSTED { get; set; }

        public string TRNAMT { get; set; }

        public string FITID { get; set; }

        public string MEMO { get; set; }

    }

}