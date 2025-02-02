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
    public class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "On Time";
        public string BoardingGate { get; set; }  // New property to track boarding gate

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
        }

        public virtual double CalculateFees()
        {
            double fees = 0;

            // Check if it's an arriving or departing flight to/from Singapore
            if (Destination == "Singapore (SIN)")
            {
                fees += 500;  // Arriving Flight
            }
            else if (Origin == "Singapore (SIN)")
            {
                fees += 800;  // Departing Flight
            }

            // Add Boarding Gate Base Fee if a Boarding Gate is assigned
            if (!string.IsNullOrEmpty(BoardingGate))
            {
                fees += 300;  // Boarding Gate Fee
            }

            return fees;
        }

        public int CompareTo(Flight other)
        {
            if (other == null) return 1;
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            return $"Flight Number: {FlightNumber} \tOrigin: {Origin} \nDestination: {Destination} \tExpected Time: {ExpectedTime} " +
                   $"Boarding Gate: {BoardingGate}";
        }
    }
}
