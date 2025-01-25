using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    public class DDJBFlight : Flight
    {
        public double RequestFee { get; set; } = 300;
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { }
        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
