using System.Collections.Generic;

namespace OFX.Reader.Infrastructure.FileManager.OFXFile {

    public sealed class OFXDocument {

        public string BANKID { get; set; }
        public string ACCTID { get; set; }

        public List<OFXTransaction> OFXTransactionCollection { get; } = new List<OFXTransaction>();

    }

}