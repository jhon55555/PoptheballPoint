using System;
using System.Data;
using System.Threading.Tasks;

namespace Services.Ludolands
{
    public interface ILudolandsService
    {
        Task<DataSet> validuser(String uid, String gtype);
        Task<DataSet> getcheckbalance(String uid);
        Task<DataSet> debit(String uid, String token, String gameId, String roundId, String tid, String rate, String amt, String wl, Double gen, String pdt, String udt);
        Task<DataSet> UpdateAC(String uid, String amount, String json, String general, String ctype);
        Task<DataSet> credit(String uid, String token, String gameId, String roundId, String tid, String renid, String rate, String amt, String wl, Double gen, String pdt, String udt);
        Task<DataSet> Rollback(String uid, String token, String gameId, String roundId, String tid, String renid, String rate, String amt, String wl, Double gen, String pdt, String udt);
    }
}
