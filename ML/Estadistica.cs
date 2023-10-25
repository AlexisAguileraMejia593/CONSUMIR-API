using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Estadistica
    {
        public Estadistica()
        {

        }
        public Estadistica(decimal norte, decimal sur, decimal este, decimal oeste)
        {
            Norte = norte;
            Sur = sur;
            Este = este;
            Oeste = oeste;
        }
        public decimal Norte { get; set; }
        public decimal Sur { get; set; }
        public decimal Este { get; set; }
        public decimal Oeste { get; set; }
    }
}
