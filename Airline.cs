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
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        public Airline(string name, string code, Dictionary<string, Flight> flights)
        {
            Name = name;
            Code = code;
            Flights = flights ?? new Dictionary<string, Flight>();
        }

        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights[flight.FlightNumber] = flight;
                return true;
            }
            return false;  // Return false if flight already exists
        }

        public double CalculateFees()
        {
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();  // Polymorphism calls the overridden CalculateFees method
            }

            // Apply promotions and discounts
            int arrivalDepartureCount = 0;
            int flightsBefore11AMOrAfter9PM = 0;
            int flightsFromDXB_BKK_NRT = 0;

            foreach (var flight in Flights.Values)
            {
                // Conditions for promotions and discounts
                if (flight.Destination == "Singapore (SIN)" || flight.Origin == "Singapore (SIN)")
                    arrivalDepartureCount++;
                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                    flightsBefore11AMOrAfter9PM++;
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                    flightsFromDXB_BKK_NRT++;
            }

            // Apply promotion-based discounts
            totalFees -= 350 * (arrivalDepartureCount / 3);  // Discount for every 3 arriving/departing flights
            totalFees -= 110 * flightsBefore11AMOrAfter9PM;  // Discount for flights before 11 AM or after 9 PM
            totalFees -= 25 * flightsFromDXB_BKK_NRT;  // Discount for flights from DXB, BKK, or NRT

            // Additional discount for airlines with more than 5 flights
            if (Flights.Count > 5)
            {
                totalFees -= totalFees * 0.03;  // 3% off total fees
            }

            return totalFees;
        }

        public bool RemoveFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber);
                return true;
            }
            return false;  // Return false if flight does not exist
        }

        public override string ToString()
        {
            string flightsInfo = string.Join(", ", Flights.Values);
            return $"Name: {Name} \t Code: {Code} \t Flights: [{flightsInfo}]";
        }
    }
}
