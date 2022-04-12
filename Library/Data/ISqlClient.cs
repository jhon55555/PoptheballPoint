using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Data
{
    public interface ISqlClient
    {
        DataSet Execute(string sSQL, string sConnectionString, IList<SqlParameter> sqlParameter);
        DataSet ExecuteTOut(string spName, string connectionString, IList<SqlParameter> sqlParameter);
        Task<DataSet> Executeasync(string sSQL, string sConnectionString, IList<SqlParameter> sqlParameter);
    }
}
