using S10266823_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number	: S10266823
// Student Name	: Maniar Naisha Keyur
// Partner Name	: Wong Jing Xuan
//==========================================================

namespace S10266823_PRG2Assignment
{
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; } = 150;

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
            : base(flightNumber, origin, destination, expectedTime) { }

        public override double CalculateFees()
        {
            double fees = base.CalculateFees();
            fees += RequestFee;  // Add the special request fee for CFFT
            return fees;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
