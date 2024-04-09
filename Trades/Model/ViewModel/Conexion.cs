using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trades.Model.ViewModel
{
    public class Conexion
    {
        public Inputs InputsToOutputs { get; set; }
        public IEnumerable<OutPuts> Outs { get; set; }
    }
}
