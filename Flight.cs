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
    class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "On Time";

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
        }
        public virtual double CalculateFees()
        {
            if (Destination == "Singapore (SIN)")
            {
                return 500;
            }
            else if (Origin == "Singapore (SIN)")
            {
                return 800;
            }
            else
            {
                return 0;
            }
        }

        public int CompareTo(Flight other)
        {
        if (other == null) return 1;
        return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            return $"Flight Number: {FlightNumber} \tOrigin: {Origin} \nDestination: {Destination} \tExpected Time: {ExpectedTime} ";

        }
    }
}
