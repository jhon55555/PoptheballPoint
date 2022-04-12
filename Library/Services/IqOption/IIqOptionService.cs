using System;
using System.Data;
using System.Threading.Tasks;

namespace Services.IqOption
{
    public interface IIqOptionService
    {
        Task<DataSet> BalanceCallback(String guid);
        Task<DataSet> validuser(String uid);
        Task<DataSet> getcheckbalance(String uid);
        Task<DataSet> debit(String uid, String token, String gameId, String roundId, String tid, String rate, String amt, String wl, Double gen, String pdt, String udt, String mkt, String ts, String po);
        Task<DataSet> UpdateAC(String uid, String amount, String json, String general, String ctype);
        Task<DataSet> credit(String uid, String token, String gameId, String roundId, String tid, String renid, String rate, String amt, String wl, Double gen, String pdt, String udt, String po);
        Task<DataSet> Rollback(String uid, String token, String gameId, String roundId, String tid, String renid, String rate, String amt, String wl, Double gen, String pdt, String udt, String po);
    }
}
