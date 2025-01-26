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
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            GateFees = new Dictionary<string, double>();
        }

        public bool AddAirline(Airline airline)
        // takes in input of Airline name and code, adds them into the airlines.csv
        // if successful, add airline to dictionary and return true
        {
            Airlines.Add(airline.Code, airline);
            return true;
        }

        public bool AddBoardingGate(BoardingGate gate)
        // adds boarding gate to boardinggate.csv
        // if successful, returns true
        // stores new gate in GateFees with the total amount to use the gate?
        {
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

        public string override ToString()
        {
            return $"Terminal Name: {TerminalName} \tAirlines: {Airlines} \tFlights: {Flights} \tGate Fees {GateFees}";
        }
    }
}
