using System;
using System.Collections.Generic;
using System.IO;

namespace prg2_final_assgn
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, double> GateFees { get; set; }

        public Terminal(string terminalName, Dictionary<string, Airline> airlines, Dictionary<string, Flight> flights, Dictionary<string, double> gateFees)
        {
            TerminalName = terminalName;
            Airlines = airlines ?? new Dictionary<string, Airline>();
            Flights = flights ?? new Dictionary<string, Flight>();
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

        public Airline GetAirlineFromFlight(string flightNumber)
        {
            foreach (var airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flightNumber))
                    return airline;
            }
            return null;
        }

        public void PrintAirLineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"Airline: {airline.Name}, Total Fees: ${airline.CalculateFees()}");
            }
        }

        public override string ToString()
        {
            return $"Terminal Name: {TerminalName} \tAirlines: {Airlines} \tFlights: {Flights} \tGate Fees {GateFees}";
        }
    }
}
