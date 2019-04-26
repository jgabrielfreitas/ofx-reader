using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FastMember;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Domain.Entities;
using OFX.Reader.Persistence.Configuration;

namespace OFX.Reader.Persistence {

    public sealed class TransactionRepository : ITransactionRepository {

        private readonly IDatabaseConnector _databaseConnector;
        
        public TransactionRepository(IDatabaseConnector databaseConnector) => this._databaseConnector = databaseConnector;

        #region SQL

        private const string GET_COUNT_SQL = @"select COUNT(*) from [transaction];";

        private const string GET_TRANSACTIONS_BY_ID_SQL = @"select transaction_id from [transaction] where bank_id = @bankId and transaction_id in @Ids;";
        
        #endregion

        public async Task<long[]> GetTransactionsById(int bankId, long[] transactionIdCollection) {

            using (SqlConnection connection = new SqlConnection(this._databaseConnector.GetConnectionString())) {
                IEnumerable<long> result = await connection.QueryAsync<long>(
                    GET_TRANSACTIONS_BY_ID_SQL, 
                    new {
                        bankId,
                        Ids = transactionIdCollection
                    });
                return result.ToArray();
            }
        }

        public async Task<int> Create(List<TransactionEntity> transactionEntityCollection) {

            int insertedRows;

            Dictionary<string, string> tableMapping = new Dictionary<string, string> {
                {"Id", "id"},
                {"BankId", "bank_id"},
                {"AccountId", "account_id"},
                {"FileId", "file_id"},
                {"TransactionId", "transaction_id"},
                {"TransactionDate", "transaction_date"},
                {"TransactionType", "transaction_type"},
                {"TransactionDescription", "transaction_description"}
            };

            string[] membersExposedToReader = tableMapping.Keys.ToArray();
            
            using (SqlConnection connection = new SqlConnection(this._databaseConnector.GetConnectionString())) {
                
                connection.Open();

                long countStart = await connection.QuerySingleAsync<int>(GET_COUNT_SQL);
                
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                    
                    bulkCopy.DestinationTableName = "[transaction]";
                    
                    using (ObjectReader reader = ObjectReader.Create(transactionEntityCollection, membersExposedToReader)) {
                        
                        DataTable transactionDataTable = new DataTable();
                        transactionDataTable.Load(reader);
                        
                        foreach (string member in membersExposedToReader) {
                            bulkCopy.ColumnMappings.Add(member, tableMapping[member]);
                        }
                        
                        bulkCopy.WriteToServer(transactionDataTable); 
                    }
                }
                
                long countEnd = await connection.QuerySingleAsync<long>(GET_COUNT_SQL);
                
                connection.Close();

                insertedRows = (int)(countEnd - countStart);
            }
            
            return insertedRows;
        }

    }

}