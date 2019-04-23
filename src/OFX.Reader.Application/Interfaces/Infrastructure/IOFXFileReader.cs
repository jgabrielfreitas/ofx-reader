using OFX.Reader.Application.OFX.Models;

namespace OFX.Reader.Application.Interfaces.Infrastructure {

    public interface IOFXFileReader {

        FinancialExchange Parse(string fileName);

    }

}