using OFX.Reader.Application.OFX.Models;

namespace OFX.Reader.Application.Interfaces.Infrastructure {

    public interface IOFXFileReader {

        FinancialExchangeModel Parse(string fileName);

    }

}