﻿using Dapper;
using Datory.Utils;
using Dm;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Datory.Tests")]

namespace Datory.DatabaseImpl
{
    internal class DmImpl : IDatabaseImpl
    {
        private static IDatabaseImpl _instance;
        public static IDatabaseImpl Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new DmImpl();
                return _instance;
            }
        }

        public string GetConnectionString(string server, bool isDefaultPort, int port, string userName, string password, string databaseName)
        {
            var connectionString = $"Server={server};";

            if (!isDefaultPort && port > 0)
            {
                connectionString += $"Port={port};";
            }
            else
            {
                connectionString += "Port=5236;";
            }
            connectionString += $"UserId={userName};Pwd={password};";
            if (!string.IsNullOrEmpty(databaseName))
            {
                connectionString += $"Database={databaseName};";
            }
            connectionString += "encoding=utf-8;";

            return connectionString;
        }

        public DbConnection GetConnection(string connectionString)
        {
            return new DmConnection(connectionString);
        }

        public Compiler GetCompiler(string connectionString)
        {
            return new DmCompiler();
        }

        public bool IsUseLegacyPagination(string connectionString)
        {
            return false;
        }

        public async Task<List<string>> GetDatabaseNamesAsync(string connectionString)
        {
            var databaseNames = new List<string>();

            using (var connection = GetConnection(connectionString))
            {
                using var rdr = await connection.ExecuteReaderAsync("SELECT DISTINCT object_name FROM ALL_OBJECTS WHERE OBJECT_TYPE = 'SCH'");
                while (rdr.Read())
                {
                    var dbName = rdr.GetString(0);
                    if (dbName == null) continue;
                    databaseNames.Add(dbName);
                }
            }

            return databaseNames;
        }

        public async Task<bool> IsTableExistsAsync(string connectionString, string tableName)
        {
            bool exists;
            var databaseName = Utilities.GetConnectionStringDatabase(connectionString);
            tableName = Utilities.FilterSql(tableName);

            try
            {
                // var sql = $"SELECT COUNT(*) FROM dba_tables WHERE owner = '{databaseName}' AND table_name = '{tableName}'";
                // var sql = $"SELECT COUNT(*) FROM user_tables WHERE owner = '{databaseName}' AND table_name = '{tableName}'";
                var sql = $"SELECT COUNT(*) FROM user_tables WHERE table_name = '{tableName}'";

                using var connection = GetConnection(connectionString);
                exists = await connection.ExecuteScalarAsync<int>(sql) == 1;
            }
            catch
            {
                try
                {
                    var sql = $"select 1 from {tableName} where 1 = 0";

                    using var connection = GetConnection(connectionString);
                    exists = await connection.ExecuteScalarAsync<int>(sql) == 1;
                }
                catch
                {
                    exists = false;
                }
            }

            return exists;
        }

        public async Task<List<string>> GetTableNamesAsync(string connectionString)
        {
            IEnumerable<string> tableNames;

            var owner = Utilities.GetConnectionStringDatabase(connectionString);
            using (var connection = GetConnection(connectionString))
            {
                // var sqlString = $"SELECT table_name FROM dba_tables WHERE OWNER = '{owner}'";
                // var sqlString = $"SELECT table_name FROM user_tables WHERE OWNER = '{owner}'";
                var sqlString = "SELECT table_name FROM user_tables";

                tableNames = await connection.QueryAsync<string>(sqlString);
            }

            return tableNames != null ? tableNames.Where(tableName => !string.IsNullOrEmpty(tableName)).ToList() : new List<string>();
        }

        public string ColumnIncrement(string columnName, int plusNum = 1)
        {
            return $"IFNULL({GetQuotedIdentifier(columnName)}, 0) + {plusNum}";
        }

        public string ColumnDecrement(string columnName, int minusNum = 1)
        {
            return $"IFNULL({GetQuotedIdentifier(columnName)}, 0) - {minusNum}";
        }

        public string GetAutoIncrementDataType(bool alterTable = false)
        {
            return alterTable ? "INT IDENTITY UNIQUE KEY" : "INT IDENTITY";
        }

        private string ToColumnString(DataType type, string columnName, int length)
        {
            if (type == DataType.Boolean)
            {
                return $"{columnName} bit";
            }
            if (type == DataType.DateTime)
            {
                return $"{columnName} timestamptz";
            }
            if (type == DataType.Decimal)
            {
                return $"{columnName} dec(18, 2)";
            }
            if (type == DataType.Integer)
            {
                return $"{columnName} int";
            }
            if (type == DataType.Text)
            {
                return $"{columnName} clob";
            }
            return $"{columnName} varchar({length})";
        }

        public string GetColumnSqlString(TableColumn tableColumn)
        {
            var columnName = GetQuotedIdentifier(tableColumn.AttributeName);
            if (tableColumn.IsIdentity)
            {
                return $@"{columnName} {GetAutoIncrementDataType()}";
            }

            return ToColumnString(tableColumn.DataType, columnName, tableColumn.DataLength);
        }

        public string GetPrimaryKeySqlString(string tableName, string attributeName)
        {
            return $@"PRIMARY KEY ({attributeName})";
        }

        public string GetQuotedIdentifier(string identifier)
        {
            return Wrap(identifier);
        }

        public static string Wrap(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "*")
            {
                return value;
            }

            if (Utilities.EqualsIgnoreCase(value, "Id"))
            {
                value = "ID";
            }
            return $@"""{value}""";
        }

        private DataType ToDataType(string dataTypeStr)
        {
            if (string.IsNullOrEmpty(dataTypeStr)) return DataType.VarChar;

            var dataType = DataType.VarChar;

            dataTypeStr = Utilities.TrimAndToLower(dataTypeStr);
            switch (dataTypeStr)
            {
                case "bit":
                    dataType = DataType.Boolean;
                    break;
                case "timestamptz":
                    dataType = DataType.DateTime;
                    break;
                case "dec":
                    dataType = DataType.Decimal;
                    break;
                case "int":
                    dataType = DataType.Integer;
                    break;
                case "clob":
                    dataType = DataType.Text;
                    break;
                case "varchar":
                    dataType = DataType.VarChar;
                    break;
            }

            return dataType;
        }

        public async Task<List<TableColumn>> GetTableColumnsAsync(string connectionString, string tableName)
        {
            var list = new List<TableColumn>();
            var owner = Utilities.GetConnectionStringDatabase(connectionString);
            tableName = Utilities.FilterSql(tableName);

            using (var connection = new DmConnection(connectionString))
            {
                // var sqlString =
                //     $"SELECT COLUMN_NAME AS ColumnName, DATA_TYPE AS DataType, DATA_Length AS DataLength FROM all_tab_columns WHERE OWNER = '{owner}' AND Table_Name = '{tableName}'";
                var sqlString =
                    $"SELECT COLUMN_NAME AS ColumnName, DATA_TYPE AS DataType, DATA_Length AS DataLength FROM user_tab_columns WHERE Table_Name = '{tableName}'";

                using var rdr = await connection.ExecuteReaderAsync(sqlString);
                while (rdr.Read())
                {
                    var columnName = rdr.IsDBNull(0) ? string.Empty : rdr.GetString(0);
                    var dataType = ToDataType(rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1));
                    var length = rdr.IsDBNull(2) || dataType == DataType.Text ? 0 : Convert.ToInt32(rdr.GetValue(2));
                    var isPrimaryKey = Utilities.EqualsIgnoreCase(columnName, "id");
                    var isIdentity = isPrimaryKey;

                    var info = new TableColumn
                    {
                        AttributeName = columnName,
                        DataType = dataType,
                        DataLength = length,
                        IsPrimaryKey = isPrimaryKey,
                        IsIdentity = isIdentity
                    };
                    list.Add(info);
                }
                rdr.Close();
            }

            //var columns = database.Connection.Query<dynamic>(sqlString);
            //foreach (var column in columns)
            //{
            //    var columnName = column.ColumnName;
            //    var dataType = ToMySqlDataType(column.DataType);
            //    var dataLength = column.DataLength;

            //    var length = dataLength == null || dataType == DataType.Text ? 0 : dataLength;

            //    var isPrimaryKey = Convert.ToString(column.ColumnKey) == "PRI";
            //    var isIdentity = Convert.ToString(column.Extra) == "auto_increment";

            //    var info = new TableColumn
            //    {
            //        AttributeName = columnName,
            //        DataType = dataType,
            //        DataLength = length,
            //        IsPrimaryKey = isPrimaryKey,
            //        IsIdentity = isIdentity
            //    };
            //    list.Add(info);

            //}

            return list;
        }

        public string GetAddColumnsSqlString(string tableName, string columnsSqlString)
        {
            tableName = GetQuotedIdentifier(Utilities.FilterSql(tableName));
            return $"ALTER TABLE {tableName} ADD ({columnsSqlString})";
        }

        public string GetOrderByRandomString()
        {
            return "RAND()";
        }

        public string GetInStr(string columnName, string inStr)
        {
            inStr = Utilities.FilterSql(inStr);
            columnName = GetQuotedIdentifier(columnName);
            
            return $"INSTR({columnName}, '{inStr}') > 0";
        }

        public string GetNotInStr(string columnName, string inStr)
        {
            inStr = Utilities.FilterSql(inStr);
            columnName = GetQuotedIdentifier(columnName);
            
            return $"INSTR({columnName}, '{inStr}') = 0";
        }
    }
}
