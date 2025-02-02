using System;
using System.Collections.Generic;
using System.IO;

//==========================================================
// Student Number	: S10266823
// Student Name	: Maniar Naisha Keyur
// Partner Name	: Wong Jing Xuan
//==========================================================

namespace S10266823_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }

        public Terminal(string terminalName, Dictionary<string, Airline> airlines, Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGate, Dictionary<string, double> gateFees)
        {
            TerminalName = terminalName;
            Airlines = airlines ?? new Dictionary<string, Airline>();
            Flights = flights ?? new Dictionary<string, Flight>();
            BoardingGates = boardingGate ?? new Dictionary<string, BoardingGate>();
            GateFees = gateFees ?? new Dictionary<string, double>();
        }

        public bool AddAirline(Airline airline)
        {
            // takes in input of Airline name and code, adds them into the airlines.csv
            // if successful, add airline to dictionary and return true
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines.Add(airline.Code, airline);
                return true;
            }
            return false; // Prevent duplicate airline entries
        }

        // Method to retrieve the airline associated with a specific flight number
        public Airline GetAirlineFromFlight(string flightNumber)
        {
            // Iterate through all airlines
            foreach (var airline in Airlines.Values)
            {
                // Check if the airline has the flight number in its collection
                if (airline.Flights.ContainsKey(flightNumber))
                    return airline;  // Return the airline if it contains the flight
            }
            return null;  // Return null if no airline with the flight number is found
        }

        // Method to print the total fees for each airline
        public void PrintAirLineFees()
        {
            // Iterate through all airlines and print their names and total fees
            foreach (var airline in Airlines.Values)
            {
                // Print airline name and its total fees by calling the CalculateFees method
                Console.WriteLine($"Airline: {airline.Name}, Total Fees: ${airline.CalculateFees()}");
            }
        }


        public override string ToString()
        {
            return $"Terminal Name: {TerminalName} \tAirlines: {Airlines} \tFlights: {Flights} \tGate Fees {GateFees}";
        }
    }
}
