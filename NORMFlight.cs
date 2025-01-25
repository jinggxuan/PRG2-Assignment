using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    public class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime) { };
        public double CalculateFees()
        {
            return base.CalculateFees();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
