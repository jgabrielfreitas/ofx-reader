using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OFX.Reader.Infrastructure.FileManager.OFXFile;
using System.Linq;
using System.Xml.Linq;
using OFX.Reader.Application.Interfaces.Infrastructure;
using OFX.Reader.Application.OFX.Models;
using OFX.Reader.Infrastructure.Extensions;

namespace OFX.Reader.Infrastructure.FileManager {

    public sealed class OFXFileReader : IOFXFileReader {

        private readonly OFXDirectorySettings _settings;

        public OFXFileReader(OFXDirectorySettings settings) => this._settings = settings;

        public FinancialExchangeModel Parse(string fileName) {
            
            string filePath = Path.Combine(this._settings.OFXFileDirectory, fileName);

            if (!File.Exists(filePath)) return null;

            string utcNow = DateTime.UtcNow.ToString("O")
                .Replace("-", string.Empty)
                .Replace(":", string.Empty)
                .Replace(".", string.Empty);
            
            string newFileName = $"{utcNow}_{fileName}";
            
            string newFilePath = Path.Combine(this._settings.OFXFileDirectory, newFileName);
            
            File.Move(filePath, newFilePath);

            OFXDocument ofxDocument = this.Read(newFilePath);

            if (ofxDocument == null) return null;
            
            FinancialExchangeModel financialExchange = new FinancialExchangeModel {
                FileId = fileName,
                BankId = int.Parse(ofxDocument.BANKID),
                AccountId = ofxDocument.ACCTID
            };

            foreach (OFXTransaction ofxTransaction in ofxDocument.OFXTransactionCollection) {
                financialExchange.TransactionCollection.Add(new TransactionModel {
                    TransactionId = int.Parse(ofxTransaction.FITID),
                    TransactionDate = DateTime.ParseExact(ofxTransaction.DTPOSTED, "yyyyMMdd", null),
                    TransactionType = ofxTransaction.TRNTYPE,
                    TransactionAmount = decimal.Parse(ofxTransaction.TRNAMT.Replace("-", ""), NumberFormatInfo.InvariantInfo),
                    TransactionDescription = ofxTransaction.MEMO
                });
            }
            
            File.Move(newFilePath, Path.Combine(this._settings.OFXProcessedFileDirectory, newFileName));

            return financialExchange;
        }

        public OFXDocument Read(string filePath) {

            XElement xElement = OfxToXml(filePath);

            if (xElement == null) return null;

            OFXDocument ofxDocument = (from node in xElement.Descendants("BANKACCTFROM")
                select new OFXDocument {
                    BANKID = node.Element("BANKID")?.Value,
                    ACCTID = node.Element("ACCTID")?.Value
                }).SingleOrDefault();

            if (ofxDocument == null) return null;

            IEnumerable<OFXTransaction> transactionCollection = from node in xElement.Descendants("STMTTRN")
                select new OFXTransaction {
                    TRNTYPE = node.Element("TRNTYPE")?.Value,
                    TRNAMT = node.Element("TRNAMT")?.Value.Replace("-", ""),
                    DTPOSTED = node.Element("DTPOSTED")?.Value,
                    MEMO = node.Element("MEMO")?.Value,
                    FITID = node.Element("FITID")?.Value
                };

            ofxDocument.OFXTransactionCollection.AddRange(transactionCollection);

            return ofxDocument;
        }

        private static XElement OfxToXml(string pathToOfxFile) {
            IEnumerable<string> lines = from line in File.ReadAllLines(pathToOfxFile)
                where line.Contains("<BANKACCTFROM>") || 
                      line.Contains("<BANKID>") || 
                      line.Contains("<ACCTID>") || 
                      line.Contains("<STMTTRN>") ||
                      line.Contains("<TRNTYPE>") ||
                      line.Contains("<DTPOSTED>") ||
                      line.Contains("<TRNAMT>") ||
                      line.Contains("<FITID>") ||
                      line.Contains("<CHECKNUM>") ||
                      line.Contains("<MEMO>")
                select line;


            XElement root = new XElement("root");
            XElement node = null;

            foreach (string line in lines) {

                if (line.IndexOf("<BANKACCTFROM>", StringComparison.Ordinal) != -1) {
                    node = new XElement("BANKACCTFROM");
                    root.Add(node);
                    continue;
                }
                
                if (line.IndexOf("<STMTTRN>", StringComparison.Ordinal) != -1) {
                    node = new XElement("STMTTRN");
                    root.Add(node);
                    continue;
                }

                string tagName = line.ReadTagName();
                XElement tag = new XElement(tagName) {
                    Value = line.ReadTagValue()
                };

                if (node != null) node.Add(tag);
            }

            return root;
        }

    }

}