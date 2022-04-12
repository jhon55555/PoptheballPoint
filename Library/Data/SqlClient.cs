using Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class SqlClient : ISqlClient
    {
        public DataSet Execute(string sSQL, string sConnectionString, IList<SqlParameter> sqlParameter)
        {
            //using (var newConnection = new SqlConnection(sConnectionString))
            //using (var mySQLAdapter = new SqlDataAdapter(sSQL, newConnection))
            //{
            //    mySQLAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            //    if (sqlParameter != null) mySQLAdapter.SelectCommand.Parameters.AddRange(sqlParameter);

            //    DataSet myDataSet = new DataSet();
            //    mySQLAdapter.Fill(myDataSet);
            //    return myDataSet;
            //}
            using (var connaction = new SqlConnection(sConnectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connaction;
                    command.CommandText = sSQL;
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameter.Any())
                    {
                        foreach (var item in sqlParameter)
                        {
                            command.Parameters.Add(item);
                        }
                    }
                    using (var da = new SqlDataAdapter())
                    {
                        da.SelectCommand = command;

                        var ds = new DataSet();
                        da.Fill(ds);

                        connaction.Close();
                        connaction.Dispose();
                        command.Dispose();
                        da.Dispose();

                        return ds;
                    }
                }
            }
        }
        public DataSet ExecuteTOut(string spName, string connectionString, IList<SqlParameter> sqlParameter)
        {
            using (var connaction = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connaction;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 50;
                    if (sqlParameter.Any())
                    {
                        foreach (var item in sqlParameter)
                        {
                            command.Parameters.Add(item);
                        }
                    }
                    using (var da = new SqlDataAdapter())
                    {
                        da.SelectCommand = command;

                        var ds = new DataSet();
                        da.Fill(ds);

                        connaction.Close();
                        connaction.Dispose();
                        command.Dispose();
                        da.Dispose();

                        return ds;
                    }
                }
            }
        }
        public Task<DataSet> Executeasync(string sSQL, string sConnectionString, IList<SqlParameter> sqlParameter)
        {
            //using (var newConnection = new SqlConnection(sConnectionString))
            //using (var mySQLAdapter = new SqlDataAdapter(sSQL, newConnection))
            //{
            //    mySQLAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            //    if (sqlParameter != null) mySQLAdapter.SelectCommand.Parameters.AddRange(sqlParameter);

            //    DataSet myDataSet = new DataSet();
            //    mySQLAdapter.Fill(myDataSet);
            //    return myDataSet;
            //}
            return Task.Run(() =>
            {
                using (var connaction = new SqlConnection(sConnectionString))
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connaction;
                        command.CommandText = sSQL;
                        command.CommandType = CommandType.StoredProcedure;

                        if (sqlParameter.Any())
                        {
                            foreach (var item in sqlParameter)
                            {
                                command.Parameters.Add(item);
                            }
                        }
                        using (var da = new SqlDataAdapter())
                        {
                            da.SelectCommand = command;

                            var ds = new DataSet();
                            try
                            {
                                da.Fill(ds);
                            }
                            catch (System.Exception)
                            {
                            }
                            finally
                            {
                                connaction.Close();
                                connaction.Dispose();
                                command.Dispose();
                                da.Dispose();
                            }
                            return ds;
                        }
                    }
                }
            });
        }
    }
}
