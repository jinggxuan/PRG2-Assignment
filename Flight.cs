using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    class Flight
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

        public override string ToString()
        {
            return $"Flight Number: {FlightNumber} \tOrigin: {Origin} \tDestination: {Destination} \tExpected Time: {ExpectedTime} \tStatus: {Status}";

        }
    }
}
