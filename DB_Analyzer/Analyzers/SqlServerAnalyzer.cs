using DB_Analyzer.Exceptions;
using DB_Analyzer.Exceptions.AnalyzerExceptions;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Analyzers
{
    internal class SqlServerAnalyzer : DbAnalyzer
    {
        public SqlServerAnalyzer(SqlConnection connection)
            : base(connection)
        { }

        public async override Task<int> GetNumberOfTables()
        {
            string query = "SELECT COUNT(*) " +
                "FROM SYS.TABLES";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return (int)await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetTablesNames()
        {
            List<string> tablesNames = new List<string>();

            string query = "SELECT TABLE_NAME " +
                "FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_TYPE = 'BASE TABLE'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read()) 
                    {
                        tablesNames.Add(reader.GetFieldValue<string>("table_name"));
                    }

                    return tablesNames;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetTablesFullInfo()
        {
            string query = "SELECT * " +
                "FROM SYS.TABLES";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<int> GetNumberOfStoredProcedures()
        {
            string query = "SELECT COUNT(*) " +
                "FROM SYS.PROCEDURES";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return (int)await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetStoredProceduresNames()
        {
            List<string> storedProceduresNames = new List<string>();

            string query = "SELECT NAME " +
                "FROM SYS.PROCEDURES";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        storedProceduresNames.Add(reader.GetFieldValue<string>("NAME"));
                    }

                    return storedProceduresNames;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetStoredProceduresFullInfo()
        {
            string query = "SELECT * " +
                "FROM SYS.PROCEDURES";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<int> GetNumberOfScalarFunctions()
        {
            string query = "SELECT COUNT(*)  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%scalar_function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return (int)(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetScalarFunctionsNames()
        {
            List<string> functionsNames = new List<string>();

            string query = "SELECT NAME  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%scalar_function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        functionsNames.Add(reader.GetFieldValue<string>("NAME"));
                    }

                    return functionsNames;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetScalarFunctionsFullInfo()
        {
            string query = "SELECT *  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%scalar_function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<int> GetNumberOfTableValuedFunctions()
        {
            string query = "SELECT COUNT(*)  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%table_valued_function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return (int)(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetTableValuedFunctionsNames()
        {
            List<string> functionsNames = new List<string>();

            string query = "SELECT NAME  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%table_valued_function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        functionsNames.Add(reader.GetFieldValue<string>("NAME"));
                    }

                    return functionsNames;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetTableValuedFunctionsFullInfo()
        {
            string query = "SELECT *  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%table_valued_function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<int> GetNumberOfFunctions()
        {
            string query = "SELECT COUNT(*)  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return (int)(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetFunctionsNames()
        {
            List<string> functionsNames = new List<string>();

            string query = "SELECT NAME  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        functionsNames.Add(reader.GetFieldValue<string>("NAME"));
                    }

                    return functionsNames;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetFunctionsFullInfo()
        {
            string query = "SELECT *  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%function%'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        //temp
        public async override Task<bool> GetBool()
        {
            return true;
        }
        //temp
    }
}
