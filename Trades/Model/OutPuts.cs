using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trades.Model
{
    public class OutPuts
    {
        [Key]
        public int ID { get; set; }
        public string TradeType { get; set; }
        public string Trade {get;set;}
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Inputs InputId {get;set;}

    }
}
