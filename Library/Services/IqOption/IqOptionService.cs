using Data;
using Newtonsoft.Json;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Services.IqOption
{
    public class IqOptionService : IIqOptionService
    {
        #region Fields

        private readonly ISqlClient _sqlClient;
        private readonly Config _config;
        private readonly ErrorLogService _errorLogService;
        #endregion
        public IqOptionService(ISqlClient sqlClient, Config config, ErrorLogService errorLogService)
        {
            _config = config;
            _sqlClient = sqlClient;
            _errorLogService = errorLogService;

        }

        public async Task<DataSet> BalanceCallback(String guid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "guid", Value = guid });
                parameters.Add(new SqlParameter() { ParameterName = "Statement", Value = "userbalance" });
                ds = await _sqlClient.Executeasync("binary_login", _config.Conn_CasinoMaster, parameters);
                _errorLogService.WriteLog("BalanceCallback+binary_login+userbalance", guid + "Res:" + JsonConvert.SerializeObject(ds));
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter() { ParameterName = "Userid", Value = ds.Tables[0].Rows[0]["u_id"] });
                    parameters1.Add(new SqlParameter() { ParameterName = "CasinoType", Value = "binary" });
                    parameters1.Add(new SqlParameter() { ParameterName = "Statement", Value = "authuser" });
                    ds1 = await _sqlClient.Executeasync("TpCasinoUserData", _config.Conn_AccD, parameters1);
                    _errorLogService.WriteLog("BalanceCallback+TpCasinoUserData+authuser", guid + "Res:" + JsonConvert.SerializeObject(ds1));
                    return ds1;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<DataSet> validuser(String uid)
        {
            try
            {
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "CasinoType", Value = "binary" });
                parameters.Add(new SqlParameter() { ParameterName = "statement", Value = "placebetcheck" });
                ds = await _sqlClient.Executeasync("tpCasino_PlacebetCheck", _config.Conn_AccD, parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> getcheckbalance(String uid)
        {
            try
            {
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "CasinoType", Value = "binary" });
                parameters.Add(new SqlParameter() { ParameterName = "statement", Value = "getbalance" });
                ds = await _sqlClient.Executeasync("tpCasino_PlacebetCheck", _config.Conn_AccD, parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> debit(String uid, String token, String gameId, String roundId, String tid, String rate, String amt, String wl, Double gen, String pdt, String udt, String mkt, String ts, String po)
        {
            try
            {
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "guid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "token", Value = token });
                parameters.Add(new SqlParameter() { ParameterName = "gameid", Value = gameId });
                parameters.Add(new SqlParameter() { ParameterName = "roundid", Value = roundId });
                parameters.Add(new SqlParameter() { ParameterName = "txnid", Value = tid });
                parameters.Add(new SqlParameter() { ParameterName = "rate", Value = po });
                parameters.Add(new SqlParameter() { ParameterName = "amount", Value = amt });
                parameters.Add(new SqlParameter() { ParameterName = "winloss", Value = wl });
                parameters.Add(new SqlParameter() { ParameterName = "general", Value = gen });
                parameters.Add(new SqlParameter() { ParameterName = "placedate", Value = pdt });
                parameters.Add(new SqlParameter() { ParameterName = "updatedate", Value = udt });
                parameters.Add(new SqlParameter() { ParameterName = "marketname", Value = mkt });
                parameters.Add(new SqlParameter() { ParameterName = "timeslot", Value = ts });
                parameters.Add(new SqlParameter() { ParameterName = "marketvalue", Value = rate });
                parameters.Add(new SqlParameter() { ParameterName = "statement", Value = "debit" });
                ds = await _sqlClient.Executeasync("binary_placebet", _config.Conn_CasinoMaster, parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> credit(String uid, String token, String gameId, String roundId, String tid, String renid, String rate, String amt, String wl, Double gen, String pdt, String udt, String po)
        {
            try
            {//@guid,@token,@gameid,@roundid,@txnid,@referenceid,@rate,@amount,@winloss,@general,@placedate,@updatedate
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "guid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "token", Value = token });
                parameters.Add(new SqlParameter() { ParameterName = "gameid", Value = gameId });
                parameters.Add(new SqlParameter() { ParameterName = "roundid", Value = roundId });
                parameters.Add(new SqlParameter() { ParameterName = "txnid", Value = tid });
                parameters.Add(new SqlParameter() { ParameterName = "referenceid", Value = renid });
                parameters.Add(new SqlParameter() { ParameterName = "marketvalue", Value = rate });
                parameters.Add(new SqlParameter() { ParameterName = "rate", Value = po });
                parameters.Add(new SqlParameter() { ParameterName = "amount", Value = amt });
                parameters.Add(new SqlParameter() { ParameterName = "winloss", Value = wl });
                parameters.Add(new SqlParameter() { ParameterName = "general", Value = gen });
                parameters.Add(new SqlParameter() { ParameterName = "placedate", Value = pdt });
                parameters.Add(new SqlParameter() { ParameterName = "updatedate", Value = udt });
                parameters.Add(new SqlParameter() { ParameterName = "statement", Value = "credit" });
                ds = await _sqlClient.Executeasync("binary_placebet", _config.Conn_CasinoMaster, parameters);
                _errorLogService.WriteLog("credit+binary_placebet+credit", renid + "Res:" + JsonConvert.SerializeObject(ds));
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> Rollback(String uid, String token, String gameId, String roundId, String tid, String renid, String rate, String amt, String wl, Double gen, String pdt, String udt, String po)
        {
            try
            {//@guid,@token,@gameid,@roundid,@txnid,@referenceid,@rate,@amount,@winloss,@general,@placedate,@updatedate
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "guid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "token", Value = token });
                parameters.Add(new SqlParameter() { ParameterName = "gameid", Value = gameId });
                parameters.Add(new SqlParameter() { ParameterName = "roundid", Value = roundId });
                parameters.Add(new SqlParameter() { ParameterName = "txnid", Value = tid });
                parameters.Add(new SqlParameter() { ParameterName = "referenceid", Value = renid });
                parameters.Add(new SqlParameter() { ParameterName = "marketvalue", Value = rate });
                parameters.Add(new SqlParameter() { ParameterName = "rate", Value = po });
                parameters.Add(new SqlParameter() { ParameterName = "amount", Value = amt });
                parameters.Add(new SqlParameter() { ParameterName = "winloss", Value = wl });
                parameters.Add(new SqlParameter() { ParameterName = "general", Value = gen });
                parameters.Add(new SqlParameter() { ParameterName = "placedate", Value = pdt });
                parameters.Add(new SqlParameter() { ParameterName = "updatedate", Value = udt });
                parameters.Add(new SqlParameter() { ParameterName = "statement", Value = "rollback" });
                ds = await _sqlClient.Executeasync("binary_placebet", _config.Conn_CasinoMaster, parameters);
                _errorLogService.WriteLog("Rollback+binary_placebet+rollback", renid + "Res:" + JsonConvert.SerializeObject(ds));
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> UpdateAC(String uid, String amount, String json, String general, String ctype)
        {
            try
            {
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "Amount", Value = amount });
                parameters.Add(new SqlParameter() { ParameterName = "AdminJson", Value = json });
                parameters.Add(new SqlParameter() { ParameterName = "CommonGeneral", Value = general });
                parameters.Add(new SqlParameter() { ParameterName = "CasinoType", Value = ctype });
                ds = await _sqlClient.Executeasync("tpBalanceUpdate", _config.Conn_AccD, parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
