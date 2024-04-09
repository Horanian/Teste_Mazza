using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Trades.Model
{
    public class Inputs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  double Value { get; set; }
        [Required]
        public string ClientSector { get; set; }
        [Required]
        public string ClientName { get; set; }
    }
}
