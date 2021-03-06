using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Rummy
{

    public class BetResultres
    {
        public string Id { get; set; }
        public string RoundId { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Odd { get; set; } = "0";
        public string Stake { get; set; }
        public Double Remaining { get; set; } = 0;
        public string Win { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public string Session { get; set; }
        public string GameId { get; set; }
        public string Refference { get; set; }
        public string Currency { get; set; }
    }
}
