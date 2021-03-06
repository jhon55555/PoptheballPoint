using TukTuk.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models.TukTuk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Common;
using Services.TukTuk;
using Services.Jwt;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TukTuk.Controllers
{

    [ApiController]
    [Route("api/game")]
    public class TukTukController : ControllerBase
    {//test
        private readonly ITokenManager _tokenManager;
        private readonly IJwtHandler _jwtHandler;
        private readonly ITukTukService _popballservice;
        private readonly ErrorLog _Elog;
        private readonly HttpHelper _httpHelper;
        private readonly Config _config;
        //private readonly IHostingEnvironment _env;ajksd
        public TukTukController(ITokenManager tokenManager, IJwtHandler jwtHandler, /*IHostingEnvironment env,*/ ITukTukService popballservice, ErrorLog Elog, Config config, HttpHelper httpHelper)
        {
            _tokenManager = tokenManager;
            _jwtHandler = jwtHandler;
            _popballservice = popballservice;
            _Elog = Elog;
            _config = config;
            _httpHelper = httpHelper;
        }
        public IActionResult Return401(string msg)
        {
            return new ObjectResult(new { status = 401, msg = msg });
        }
        public IActionResult Return100(string msg)
        {
            return new ObjectResult(new { status = 100, msg = msg });
        }
        public IActionResult Return200(string msg, Object data = null)
        {
            if (data == null)
                return new ObjectResult(new { status = 200, msg = msg });
            return new ObjectResult(new { status = 200, msg = msg, data = data });
        }
        public IActionResult Return300(string msg)
        {
            return new ObjectResult(new { status = 300, msg = msg });
        }
        public IActionResult Return400(string msg)
        {
            return new ObjectResult(new { status = 400, msg = msg });
        }
        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            var iv = aesAlg.IV;

            var decryptedContent = msEncrypt.ToArray();

            var result = new byte[iv.Length + decryptedContent.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

            return Convert.ToBase64String(result);
        }
        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length]; //changes here

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            // Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length); // changes here
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var result = srDecrypt.ReadToEnd();

            return result;
        }
        [Route("balancecallback")]
        public async Task<IActionResult> BalanceCallback([FromBody] BalanceCallback balanceCallback)
        {
            try
            {
                long timeStamp = DateTime.Now.ToFileTime();
                Response.Headers.Add("X-Timestamp", timeStamp.ToString());
                _Elog.WriteLogAll("BalanceCallback", JsonConvert.SerializeObject(balanceCallback));
                var res = DecryptString(balanceCallback.Body, _config.Secret);
                _Elog.WriteLogAll("BalanceCallback+1", res);
                var json = new
                {
                    Balance = 1000.00,
                    Currency = "Inr"
                };
                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
                var res1 = DecryptString(balde, _config.Secret);

                return Ok(new { Body = balde });

            }
            catch (Exception ex)
            {
                _Elog.WriteLog("BalanceCallback", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
                return Return400(ex.Message);
            }
        }
        [Route("beturl")]
        public async Task<IActionResult> BetUrl([FromBody] BalanceCallback balanceCallback)
        {
            try
            {
                long timeStamp = DateTime.Now.ToFileTime();
                Response.Headers.Add("X-Timestamp", timeStamp.ToString());
                _Elog.WriteLogAll("BetUrl", JsonConvert.SerializeObject(balanceCallback));
                var res = DecryptString(balanceCallback.Body, _config.Secret);
                _Elog.WriteLogAll("BetUrl+1", res);
                var json = new
                {
                    Balance = 1000.00,
                    ErrorCode = 0,
                    Currency = "Inr"
                };
                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
                var res1 = DecryptString(balde, _config.Secret);

                return Ok(new { Body = balde });

            }
            catch (Exception ex)
            {
                _Elog.WriteLog("BetUrl", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
                return Return400(ex.Message);
            }

            //try
            //{
            //    long timeStamp = DateTime.Now.ToFileTime();
            //    Response.Headers.Add("X-Timestamp", timeStamp.ToString());
            //    _Elog.WriteLogAll("BetUrl", JsonConvert.SerializeObject(balanceCallback));
            //    var res = DecryptString(balanceCallback.Body, _config.Secret);
            //    _Elog.WriteLogAll("BetUrl+1", res);
            //    var obj = JsonConvert.DeserializeObject<BetUrlres>(res);
            //    var TblData1 = await _popballservice.validuser(obj.PlayerId);
            //    _Elog.WriteLogAll("BetUrl+validuser : ", JsonConvert.SerializeObject(TblData1));
            //    if (TblData1 != null && TblData1.Tables.Count > 0 && TblData1.Tables[0].Rows.Count > 0 && TblData1.Tables[0].Rows[0]["id"].ToString() == "1")
            //    {
            //        var TblData = await _popballservice.debit(obj.PlayerId, obj.Session, obj.GameId, obj.RoundId, obj.Id, obj.Odd, obj.Stake, obj.Win, Convert.ToDouble(TblData1.Tables[0].Rows[0]["general"]) - Convert.ToDouble(TblData1.Tables[0].Rows[0]["exposer"]), obj.Created, obj.Updated);
            //        _Elog.WriteLogAll("BetUrl+debit : ", JsonConvert.SerializeObject(TblData));
            //        if (TblData != null && TblData.Tables.Count > 0 && TblData.Tables[0].Rows.Count > 0 && TblData.Tables[0].Rows[0]["id"].ToString() == "0" && TblData.Tables[0].Rows[0]["istransfer"].ToString() == "1")
            //        {
            //            DataSet TblData2 = await _popballservice.UpdateAC(TblData.Tables[0].Rows[0]["uid"].ToString(), TblData.Tables[0].Rows[0]["amount"].ToString(), TblData.Tables[0].Rows[0]["AdminJson"].ToString(), TblData.Tables[0].Rows[0]["CommonGeneral"].ToString(), TblData.Tables[0].Rows[0]["gametype"].ToString());
            //            _Elog.WriteLogAll("BetUrl+UpdateAC : ", JsonConvert.SerializeObject(TblData2));
            //            if (TblData2 != null && TblData2.Tables.Count > 0 && TblData2.Tables[0].Rows.Count > 0 && TblData2.Tables[0].Rows[0][0].ToString() == "1" && TblData.Tables[0].Rows[0]["id"].ToString()=="0")
            //            {
            //                var json = new
            //                {
            //                    ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                    Balance = (!string.IsNullOrEmpty(TblData2.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData2.Tables[0].Rows[0]["general"]) : 0) - (!string.IsNullOrEmpty(TblData2.Tables[0].Rows[0]["exposer"].ToString()) ? Convert.ToDecimal(TblData2.Tables[0].Rows[0]["exposer"]) : 0),
            //                };
            //                _Elog.WriteLogAll("BetUrl+1 : ", JsonConvert.SerializeObject(json));
            //                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //                var res1 = DecryptString(balde, _config.Secret);
            //                _Elog.WriteLogAll("BetUrl1+res11 : ", JsonConvert.SerializeObject(res1));
            //                return Ok(new { Body = balde });
            //            }
            //            else
            //            {
            //                var json = new
            //                {
            //                    ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                    Balance = (!string.IsNullOrEmpty(TblData.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData.Tables[0].Rows[0]["general"]) : 0),
            //                };
            //                _Elog.WriteLogAll("BetUrl+2 : ", JsonConvert.SerializeObject(json));
            //                var balde22 = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //                var res11 = DecryptString(balde22, _config.Secret);
            //                _Elog.WriteLogAll("BetUrl+res11 : ", JsonConvert.SerializeObject(res11));
            //                return Ok(new { Body = balde22 });
            //            }
            //        }
            //        else
            //        {
            //            var json = new
            //            {
            //                ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                Balance = (!string.IsNullOrEmpty(TblData.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData.Tables[0].Rows[0]["general"]) : 0),
            //            };
            //            _Elog.WriteLogAll("BetUrl+3 : ", JsonConvert.SerializeObject(json));
            //            var balde1 = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //            var res11 = DecryptString(balde1, _config.Secret);
            //            _Elog.WriteLogAll("BetUrl+res11 : ", JsonConvert.SerializeObject(res11));
            //            return Ok(new { Body = balde1 });
            //        }
            //    }

            //    var balde2 = EncryptString("", _config.Secret);
            //    var res12 = DecryptString(balde2, _config.Secret);
            //    return Ok(new { Body = balde2 });
            //}
            //catch (Exception ex)
            //{
            //    _Elog.WriteLog("BetUrl", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
            //    return Return400(ex.Message);
            //}
        }
        [Route("betresult")]
        public async Task<IActionResult> BetResult([FromBody] BalanceCallback balanceCallback)
        {
            try
            {
                //var json = JsonConvert.Serialize({
                //PlayerId: string
                //PlayerName: string});
                long timeStamp = DateTime.Now.ToFileTime();
                Response.Headers.Add("X-Timestamp", timeStamp.ToString());
                _Elog.WriteLogAll("BetResult", JsonConvert.SerializeObject(balanceCallback));
                var res = DecryptString(balanceCallback.Body, _config.Secret);
                _Elog.WriteLogAll("BetResult+1", res);
                var json = new
                {
                    Balance = 1000.00,
                    ErrorCode = 0,
                    Currency = "Inr"
                };
                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
                var res1 = DecryptString(balde, _config.Secret);

                return Ok(new { Body = balde });

            }
            catch (Exception ex)
            {
                _Elog.WriteLog("BetResult", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
                return Return400(ex.Message);
            }

            //try
            //{
            //    long timeStamp = DateTime.Now.ToFileTime();
            //    Response.Headers.Add("X-Timestamp", timeStamp.ToString());
            //    _Elog.WriteLogAll("BetResult", JsonConvert.SerializeObject(balanceCallback));
            //    var res = DecryptString(balanceCallback.Body, _config.Secret);
            //    _Elog.WriteLogAll("BetResult+1", res);
            //    var obj = JsonConvert.DeserializeObject<BetResultres>(res);
            //    var TblData1 = await _popballservice.validuser(obj.PlayerId);
            //    _Elog.WriteLogAll("BetResult+validuser : ", JsonConvert.SerializeObject(TblData1));
            //    if (TblData1 != null && TblData1.Tables.Count > 0 && TblData1.Tables[0].Rows.Count > 0 && TblData1.Tables[0].Rows[0]["id"].ToString() == "1")
            //    {
            //        var TblData = await _popballservice.credit(obj.PlayerId, obj.Session, obj.GameId, obj.RoundId, obj.Id, obj.Refference, obj.Odd, obj.Stake, obj.Win, Convert.ToDouble(TblData1.Tables[0].Rows[0]["general"]) - Convert.ToDouble(TblData1.Tables[0].Rows[0]["exposer"]), obj.Created, obj.Updated);
            //        _Elog.WriteLogAll("BetResult+credit : ", JsonConvert.SerializeObject(TblData));
            //        if (TblData != null && TblData.Tables.Count > 0 && TblData.Tables[0].Rows.Count > 0 && TblData.Tables[0].Rows[0]["id"].ToString() == "0" && TblData.Tables[0].Rows[0]["istransfer"].ToString() == "1")
            //        {
            //            DataSet TblData2 = await _popballservice.UpdateAC(TblData.Tables[0].Rows[0]["uid"].ToString(), TblData.Tables[0].Rows[0]["amount"].ToString(), TblData.Tables[0].Rows[0]["AdminJson"].ToString(), TblData.Tables[0].Rows[0]["CommonGeneral"].ToString(), TblData.Tables[0].Rows[0]["gametype"].ToString());
            //            _Elog.WriteLogAll("BetResult+UpdateAC : ", JsonConvert.SerializeObject(TblData2));
            //            if (TblData2 != null && TblData2.Tables.Count > 0 && TblData2.Tables[0].Rows.Count > 0 && TblData2.Tables[0].Rows[0][0].ToString() == "1")
            //            {
            //                var json = new
            //                {
            //                    ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                    Balance = (!string.IsNullOrEmpty(TblData2.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData2.Tables[0].Rows[0]["general"]) : 0) - (!string.IsNullOrEmpty(TblData2.Tables[0].Rows[0]["exposer"].ToString()) ? Convert.ToDecimal(TblData2.Tables[0].Rows[0]["exposer"]) : 0),
            //                };
            //                _Elog.WriteLogAll("BetResult : ", JsonConvert.SerializeObject(json));
            //                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //                var res1 = DecryptString(balde, _config.Secret);
            //                return Ok(new { Body = balde });
            //            }
            //            else
            //            {
            //                var json = new
            //                {
            //                    ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                    Balance = (!string.IsNullOrEmpty(TblData.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData.Tables[0].Rows[0]["general"]) : 0),
            //                };
            //                _Elog.WriteLogAll("BetResult+3 : ", JsonConvert.SerializeObject(json));
            //                var balde1 = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //                var res11 = DecryptString(balde1, _config.Secret);
            //                _Elog.WriteLogAll("BetResult+res11 : ", JsonConvert.SerializeObject(res11));
            //                return Ok(new { Body = balde1 });
            //            }
            //        }
            //        else
            //        {
            //            var json = new
            //            {
            //                ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                Balance = (!string.IsNullOrEmpty(TblData.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData.Tables[0].Rows[0]["general"]) : 0),
            //            };
            //            _Elog.WriteLogAll("BetResult : ", JsonConvert.SerializeObject(json));
            //            var balde1 = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //            var res11 = DecryptString(balde1, _config.Secret);
            //            return Ok(new { Body = balde1 });
            //        }
            //    }

            //    var balde2 = EncryptString("", _config.Secret);
            //    var res12 = DecryptString(balde2, _config.Secret);
            //    return Ok(new { Body = balde2 });
            //}
            //catch (Exception ex)
            //{
            //    _Elog.WriteLog("BetResult", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
            //    return Return400(ex.Message);
            //}
        }
        [Route("rollback")]
        public async Task<IActionResult> Rollback([FromBody] BalanceCallback balanceCallback)
        {
            try
            {
                //var json = JsonConvert.Serialize({
                //PlayerId: string
                //PlayerName: string});
                long timeStamp = DateTime.Now.ToFileTime();
                Response.Headers.Add("X-Timestamp", timeStamp.ToString());
                _Elog.WriteLogAll("BetResult", JsonConvert.SerializeObject(balanceCallback));
                var res = DecryptString(balanceCallback.Body, _config.Secret);
                _Elog.WriteLogAll("BetResult+1", res);
                var json = new
                {
                    Balance = 1000.00,
                    ErrorCode = 0,
                    Currency = "Inr"
                };
                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
                var res1 = DecryptString(balde, _config.Secret);

                return Ok(new { Body = balde });

            }
            catch (Exception ex)
            {
                _Elog.WriteLog("BetResult", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
                return Return400(ex.Message);
            }
            //try
            //{
            //    long timeStamp = DateTime.Now.ToFileTime();
            //    Response.Headers.Add("X-Timestamp", timeStamp.ToString());
            //    _Elog.WriteLogAll("Rollback", JsonConvert.SerializeObject(balanceCallback));
            //    var res = DecryptString(balanceCallback.Body, _config.Secret);
            //    _Elog.WriteLogAll("Rollback+1", res);
            //    var obj = JsonConvert.DeserializeObject<BetResultres>(res);
            //    var TblData1 = await _popballservice.validuser(obj.PlayerId);
            //    _Elog.WriteLogAll("Rollback+validuser : ", JsonConvert.SerializeObject(TblData1));
            //    if (TblData1 != null && TblData1.Tables.Count > 0 && TblData1.Tables[0].Rows.Count > 0 && TblData1.Tables[0].Rows[0]["id"].ToString() == "1")
            //    {
            //        var TblData = await _popballservice.Rollback(obj.PlayerId, obj.Session, obj.GameId, obj.RoundId, obj.Id, obj.Refference, obj.Odd, obj.Stake, obj.Win, Convert.ToDouble(TblData1.Tables[0].Rows[0]["general"]) - Convert.ToDouble(TblData1.Tables[0].Rows[0]["exposer"]), obj.Created, obj.Updated);
            //        _Elog.WriteLogAll("Rollback+Rollback : ", JsonConvert.SerializeObject(TblData));
            //        if (TblData != null && TblData.Tables.Count > 0 && TblData.Tables[0].Rows.Count > 0 && TblData.Tables[0].Rows[0]["id"].ToString() == "0" && TblData.Tables[0].Rows[0]["istransfer"].ToString() == "1")
            //        {
            //            DataSet TblData2 = await _popballservice.UpdateAC(TblData.Tables[0].Rows[0]["uid"].ToString(), TblData.Tables[0].Rows[0]["amount"].ToString(), TblData.Tables[0].Rows[0]["AdminJson"].ToString(), TblData.Tables[0].Rows[0]["CommonGeneral"].ToString(), TblData.Tables[0].Rows[0]["gametype"].ToString());
            //            _Elog.WriteLogAll("Rollback+UpdateAC : ", JsonConvert.SerializeObject(TblData2));
            //            if (TblData2 != null && TblData2.Tables.Count > 0 && TblData2.Tables[0].Rows.Count > 0 && TblData2.Tables[0].Rows[0][0].ToString() == "1")
            //            {
            //                var json = new
            //                {
            //                    ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                    Balance = (!string.IsNullOrEmpty(TblData2.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData2.Tables[0].Rows[0]["general"]) : 0) - (!string.IsNullOrEmpty(TblData2.Tables[0].Rows[0]["exposer"].ToString()) ? Convert.ToDecimal(TblData2.Tables[0].Rows[0]["exposer"]) : 0),
            //                };
            //                _Elog.WriteLogAll("Rollback : ", JsonConvert.SerializeObject(json));
            //                var balde = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //                var res1 = DecryptString(balde, _config.Secret);
            //                return Ok(new { Body = balde });
            //            }
            //            else
            //            {
            //                var json = new
            //                {
            //                    ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                    Balance = (!string.IsNullOrEmpty(TblData.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData.Tables[0].Rows[0]["general"]) : 0),
            //                };
            //                _Elog.WriteLogAll("Rollback+3 : ", JsonConvert.SerializeObject(json));
            //                var balde1 = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //                var res11 = DecryptString(balde1, _config.Secret);
            //                _Elog.WriteLogAll("Rollback+res11 : ", JsonConvert.SerializeObject(res11));
            //                return Ok(new { Body = balde1 });
            //            }
            //        }
            //        else
            //        {
            //            var json = new
            //            {
            //                ErrorCode = Convert.ToInt32(TblData.Tables[0].Rows[0]["id"]),
            //                Balance = (!string.IsNullOrEmpty(TblData.Tables[0].Rows[0]["general"].ToString()) ? Convert.ToDecimal(TblData.Tables[0].Rows[0]["general"]) : 0),
            //            };
            //            _Elog.WriteLogAll("Rollback : ", JsonConvert.SerializeObject(json));
            //            var balde1 = EncryptString(JsonConvert.SerializeObject(json), _config.Secret);
            //            var res11 = DecryptString(balde1, _config.Secret);
            //            return Ok(new { Body = balde1 });
            //        }
            //    }

            //    var balde2 = EncryptString("", _config.Secret);
            //    var res12 = DecryptString(balde2, _config.Secret);
            //    return Ok(new { Body = balde2 });
            //}
            //catch (Exception ex)
            //{
            //    _Elog.WriteLog("Rollback", ex.Message.ToString(), " : Req" + JsonConvert.SerializeObject(balanceCallback));
            //    return Return400(ex.Message);
            //}
        }
    }
}
