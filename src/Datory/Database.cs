﻿using Dapper;
using Datory.DatabaseImpl;
using Datory.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datory
{
    public class Database : IDatabase
    {
        public Database(DatabaseType databaseType, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) return;

            if (databaseType == DatabaseType.MySql || databaseType == DatabaseType.OceanBase)
            {
                if (!Utilities.ContainsIgnoreCase(connectionString, "SslMode="))
                {
                    connectionString = connectionString.TrimEnd(';') + ";SslMode=Preferred;";
                }
                if (!Utilities.ContainsIgnoreCase(connectionString, "CharSet="))
                {
                    connectionString = connectionString.TrimEnd(';') + ";CharSet=utf8;";
                }
            }
            else if (databaseType == DatabaseType.SQLite)
            {
                if (connectionString.Contains("=~/"))
                {
                    var projDirectoryPath = Directory.GetCurrentDirectory();
                    connectionString = connectionString.Replace("=~/", $"={projDirectoryPath}/");
                    connectionString = connectionString.Replace('/', Path.DirectorySeparatorChar);
                }
                if (!Utilities.ContainsIgnoreCase(connectionString, "Version="))
                {
                    connectionString = connectionString.TrimEnd(';') + ";Version=3;";
                }
            }
            else if (databaseType == DatabaseType.Dm)
            {
                if (!Utilities.ContainsIgnoreCase(connectionString, "encoding="))
                {
                    connectionString = connectionString.TrimEnd(';') + ";encoding=utf-8;";
                }
            }

            DatabaseType = databaseType;
            ConnectionString = connectionString;
        }

        public DatabaseType DatabaseType { get; }

        public string ConnectionString { get; }

        public string DatabaseName => Utilities.GetConnectionStringDatabase(ConnectionString);

        public DbConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString)) return null;

            DbConnection conn = null;
            if (DatabaseType == DatabaseType.MySql || DatabaseType == DatabaseType.OceanBase)
            {
                conn = MySqlImpl.Instance.GetConnection(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.MariaDB)
            {
                conn = MariaDBImpl.Instance.GetConnection(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.SqlServer)
            {
                conn = SqlServerImpl.Instance.GetConnection(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.PostgreSql)
            {
                conn = PostgreSqlImpl.Instance.GetConnection(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.SQLite)
            {
                conn = SQLiteImpl.Instance.GetConnection(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.KingbaseES)
            {
                conn = KingbaseESImpl.Instance.GetConnection(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.Dm)
            {
                conn = DmImpl.Instance.GetConnection(ConnectionString);
            }

            return conn;
        }

        public async Task<(bool IsConnectionWorks, string ErrorMessage)> IsConnectionWorksAsync()
        {
            var retVal = false;
            var errorMessage = string.Empty;
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();
                if (connection.State == ConnectionState.Open)
                {
                    retVal = true;
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return (retVal, errorMessage);
        }

        public string GetQuotedIdentifier(string identifier)
        {
            return DbUtils.GetQuotedIdentifier(DatabaseType, identifier);
        }

        public async Task<string> AddIdentityColumnIdIfNotExistsAsync(string tableName, List<TableColumn> columns)
        {
            var identityColumnName = string.Empty;
            if (columns != null)
            {
                foreach (var column in columns)
                {
                    if (column.IsIdentity || Utilities.EqualsIgnoreCase(column.AttributeName, "id"))
                    {
                        identityColumnName = column.AttributeName;
                        break;
                    }
                }
            }
            else
            {
                columns = new List<TableColumn>();
            }

            if (string.IsNullOrEmpty(identityColumnName))
            {
                identityColumnName = nameof(Entity.Id);
                var sqlString =
                    DbUtils.GetAddColumnsSqlString(DatabaseType, tableName, $"{identityColumnName} {DbUtils.GetAutoIncrementDataType(DatabaseType, true)}");

                using (var connection = GetConnection())
                {
                    await connection.ExecuteAsync(sqlString);
                }

                columns.Insert(0, new TableColumn
                {
                    AttributeName = identityColumnName,
                    DataType = DataType.Integer,
                    IsPrimaryKey = false,
                    IsIdentity = true
                });
            }

            return identityColumnName;
        }

        public async Task AlterTableAsync(string tableName, IEnumerable<TableColumn> tableColumns, IEnumerable<string> dropColumnNames = null)
        {
            var list = new List<string>();

            var columnNameList = await GetColumnNamesAsync(tableName);
            foreach (var tableColumn in tableColumns)
            {
                if (!Utilities.ContainsIgnoreCase(columnNameList, tableColumn.AttributeName))
                {
                    list.Add(DbUtils.GetAddColumnsSqlString(DatabaseType, tableName, DbUtils.GetColumnSqlString(DatabaseType, tableColumn)));
                }
            }

            if (dropColumnNames != null)
            {
                foreach (var columnName in columnNameList)
                {
                    if (Utilities.ContainsIgnoreCase(dropColumnNames, columnName))
                    {
                        var sql = DbUtils.GetDropColumnsSqlString(DatabaseType, tableName, columnName);
                        if (!string.IsNullOrEmpty(sql))
                        {
                            list.Add(sql);
                        }
                    }
                }
            }

            if (list.Count <= 0) return;

            foreach (var sqlString in list)
            {
                using var connection = GetConnection();
                connection.Execute(sqlString);
            }
        }

        public async Task CreateTableAsync(string tableName, IEnumerable<TableColumn> tableColumns)
        {
            var sqlBuilder = new StringBuilder();

            tableName = GetQuotedIdentifier(Utilities.FilterSql(tableName));

            sqlBuilder.Append($@"CREATE TABLE {tableName} (").AppendLine();

            var primaryKeyColumns = new List<TableColumn>();
            TableColumn identityColumn = null;

            foreach (var tableColumn in tableColumns)
            {
                if (Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.Id)))
                {
                    tableColumn.DataType = DataType.Integer;
                    tableColumn.IsIdentity = true;
                    tableColumn.IsPrimaryKey = true;
                }
                else if (Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.Guid)))
                {
                    tableColumn.DataType = DataType.VarChar;
                    tableColumn.DataLength = 50;
                }
                else if (Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.CreatedDate)))
                {
                    tableColumn.DataType = DataType.DateTime;
                }
                else if (Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.LastModifiedDate)))
                {
                    tableColumn.DataType = DataType.DateTime;
                }
                else if (Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.ExtendValues)))
                {
                    tableColumn.DataType = DataType.Text;
                }
            }

            foreach (var tableColumn in tableColumns)
            {
                if (string.IsNullOrEmpty(tableColumn.AttributeName)) continue;

                if (tableColumn.IsIdentity)
                {
                    identityColumn = tableColumn;
                }

                if (tableColumn.IsPrimaryKey)
                {
                    primaryKeyColumns.Add(tableColumn);
                }

                if (tableColumn.DataType == DataType.VarChar && tableColumn.DataLength == 0)
                {
                    tableColumn.DataLength = DbUtils.VarCharDefaultLength;
                }
                else if (tableColumn.DataType == DataType.VarChar && tableColumn.DataLength == -1)
                {
                    tableColumn.DataType = DataType.Text;
                }

                var columnSql = DbUtils.GetColumnSqlString(DatabaseType, tableColumn);
                if (!string.IsNullOrEmpty(columnSql))
                {
                    sqlBuilder.Append(columnSql).Append(",");
                }
            }

            if (DatabaseType != DatabaseType.SQLite && DatabaseType != DatabaseType.KingbaseES)
            {
                if (identityColumn != null)
                {
                    var primaryKeySql = DbUtils.GetPrimaryKeySqlString(DatabaseType, tableName, identityColumn.AttributeName);
                    if (!string.IsNullOrEmpty(primaryKeySql))
                    {
                        sqlBuilder.Append(primaryKeySql).Append(",");
                    }
                }
                else if (primaryKeyColumns.Count > 0)
                {
                    foreach (var tableColumn in primaryKeyColumns)
                    {
                        var primaryKeySql = DbUtils.GetPrimaryKeySqlString(DatabaseType, tableName, tableColumn.AttributeName);
                        if (!string.IsNullOrEmpty(primaryKeySql))
                        {
                            sqlBuilder.Append(primaryKeySql).Append(",");
                        }
                    }
                }
            }

            sqlBuilder.Length--;

            sqlBuilder.AppendLine().Append(DatabaseType == DatabaseType.MySql || DatabaseType == DatabaseType.OceanBase
                ? ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
                : ")");

            using var connection = GetConnection();
            await connection.ExecuteAsync(sqlBuilder.ToString());
        }

        public async Task CreateIndexAsync(string tableName, string indexName, params string[] columns)
        {
            if (columns == null || columns.Length == 0) return;

            var fullTableName = GetQuotedIdentifier(Utilities.FilterSql(tableName));
            var fullIndexName = GetQuotedIdentifier(Utilities.FilterSql(indexName));
            var sqlString = new StringBuilder($@"CREATE INDEX {fullIndexName} ON {fullTableName}(");

            foreach (var column in columns)
            {
                var columnName = column;
                var columnOrder = "ASC";
                var i = column.IndexOf(" ", StringComparison.Ordinal);
                if (i != -1)
                {
                    columnName = column.Substring(0, i);
                    columnOrder = column.Substring(i + 1);
                }
                sqlString.Append($"{GetQuotedIdentifier(columnName)} {columnOrder}, ");
            }

            sqlString.Length -= 2;
            sqlString.Append(")");

            using var connection = GetConnection();
            await connection.ExecuteAsync(sqlString.ToString());
        }

        public async Task<List<string>> GetColumnNamesAsync(string tableName)
        {
            var allTableColumnInfoList = await GetTableColumnsAsync(tableName);
            return allTableColumnInfoList.Select(tableColumnInfo => tableColumnInfo.AttributeName).ToList();
        }

        public string GetTableName<T>() where T : Entity
        {
            return ReflectionUtils.GetTableName(typeof(T));
        }

        public List<TableColumn> GetTableColumns<T>() where T : Entity
        {
            return ReflectionUtils.GetTableColumns(typeof(T));
        }

        public List<TableColumn> GetTableColumns(IEnumerable<TableColumn> tableColumns)
        {
            var columns = new List<TableColumn>
            {
                new TableColumn
                {
                    AttributeName = nameof(Entity.Id),
                    DataType = DataType.Integer,
                    IsIdentity = true,
                    IsPrimaryKey = true
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.Guid),
                    DataType = DataType.VarChar,
                    DataLength = 50
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.CreatedDate),
                    DataType = DataType.DateTime
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.LastModifiedDate),
                    DataType = DataType.DateTime
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.ExtendValues),
                    DataType = DataType.Text
                }
            };
            if (tableColumns != null)
            {
                columns.AddRange(tableColumns.Where(tableColumn =>
                    !Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.Id)) &&
                    !Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.Guid)) &&
                    !Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.CreatedDate)) &&
                    !Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.LastModifiedDate)) &&
                    !Utilities.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.ExtendValues))));
            }

            return columns;
        }

        public async Task<List<TableColumn>> GetTableColumnsAsync(string tableName)
        {
            List<TableColumn> list = null;

            if (DatabaseType == DatabaseType.MySql || DatabaseType == DatabaseType.OceanBase)
            {
                list = await MySqlImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.MariaDB)
            {
                list = await MariaDBImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.SqlServer)
            {
                list = await SqlServerImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.PostgreSql)
            {
                list = await PostgreSqlImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.SQLite)
            {
                list = await SQLiteImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.KingbaseES)
            {
                list = await KingbaseESImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.Dm)
            {
                list = await DmImpl.Instance.GetTableColumnsAsync(ConnectionString, tableName);
            }

            return list;
        }

        public async Task DropTableAsync(string tableName)
        {
            using var connection = GetConnection();
            tableName = GetQuotedIdentifier(Utilities.FilterSql(tableName));
            await connection.ExecuteAsync($"DROP TABLE {tableName}");
        }

        public async Task<List<string>> GetDatabaseNamesAsync()
        {
            List<string> tableNames = null;

            if (DatabaseType == DatabaseType.MySql || DatabaseType == DatabaseType.OceanBase)
            {
                tableNames = await MySqlImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.MariaDB)
            {
                tableNames = await MariaDBImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.SqlServer)
            {
                tableNames = await SqlServerImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.PostgreSql)
            {
                tableNames = await PostgreSqlImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.SQLite)
            {
                tableNames = await SQLiteImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.KingbaseES)
            {
                tableNames = await KingbaseESImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.Dm)
            {
                tableNames = await DmImpl.Instance.GetDatabaseNamesAsync(ConnectionString);
            }

            return tableNames;
        }

        public async Task<bool> IsTableExistsAsync(string tableName)
        {
            var exists = false;

            if (DatabaseType == DatabaseType.MySql || DatabaseType == DatabaseType.OceanBase)
            {
                exists = await MySqlImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.MariaDB)
            {
                exists = await MariaDBImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.SqlServer)
            {
                exists = await SqlServerImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.PostgreSql)
            {
                exists = await PostgreSqlImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.SQLite)
            {
                exists = await SQLiteImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.KingbaseES)
            {
                exists = await KingbaseESImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }
            else if (DatabaseType == DatabaseType.Dm)
            {
                exists = await DmImpl.Instance.IsTableExistsAsync(ConnectionString, tableName);
            }

            return exists;
        }

        public async Task<List<string>> GetTableNamesAsync()
        {
            List<string> tableNames = null;

            if (DatabaseType == DatabaseType.MySql || DatabaseType == DatabaseType.OceanBase)
            {
                tableNames = await MySqlImpl.Instance.GetTableNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.MariaDB)
            {
                tableNames = await MariaDBImpl.Instance.GetTableNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.SqlServer)
            {
                tableNames = await SqlServerImpl.Instance.GetTableNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.PostgreSql)
            {
                tableNames = await PostgreSqlImpl.Instance.GetTableNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.SQLite)
            {
                tableNames = await SQLiteImpl.Instance.GetTableNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.KingbaseES)
            {
                tableNames = await KingbaseESImpl.Instance.GetTableNamesAsync(ConnectionString);
            }
            else if (DatabaseType == DatabaseType.Dm)
            {
                tableNames = await DmImpl.Instance.GetTableNamesAsync(ConnectionString);
            }

            return tableNames;
        }
    }
}
