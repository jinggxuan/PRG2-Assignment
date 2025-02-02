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
            // Check if the flight number is already in the collection
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights[flight.FlightNumber] = flight; // Add the flight if it doesn't exist
                return true; // Indicate that the flight was added successfully
            }
            return false;  // Return false if the flight already exists in the collection
        }

        // Method to calculate the total fees for all flights of the airline
        public double CalculateFees()
        {
            double totalFees = 0;  // Initialize the total fees counter

            // Iterate over all flights and sum up their respective fees
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();  // Polymorphism calls the overridden CalculateFees method in the flight class
            }

            return totalFees; // Return the total fees for the airline
        }

        // Method to remove a flight from the airline's flight collection
        public bool RemoveFlight(Flight flight)
        {
            // Check if the flight exists in the collection
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber); // Remove the flight by its flight number
                return true; // Indicate that the flight was removed successfully
            }
            return false;  // Return false if the flight does not exist in the collection
        }

        public override string ToString()
        {
            string flightsInfo = string.Join(", ", Flights.Values);
            return $"Name: {Name} \t Code: {Code} \t Flights: [{flightsInfo}]";
        }
    }
}
