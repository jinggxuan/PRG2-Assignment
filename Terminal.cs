using System;
using System.Collections.Generic;
using System.IO;

namespace prg2_final_assgn
{
    public class Terminal
    {
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

        // Load Airlines from a file
        public void LoadAirlines(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length >= 2)
                    {
                        string code = data[0].Trim();
                        string name = data[1].Trim();
                        airlines[code] = new Airline(code, name);
                    }
                }
                Console.WriteLine("Airlines loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading airlines: {ex.Message}");
            }
        }

        // Load Boarding Gates from a file
        public void LoadBoardingGates(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length >= 2)
                    {
                        string gateNumber = data[0].Trim();
                        string specialRequest = data[1].Trim();
                        boardingGates[gateNumber] = new BoardingGate(gateNumber, specialRequest);
                    }
                }
                Console.WriteLine("Boarding gates loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }

        // Load Flights from a file
        public void LoadFlights(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length >= 5)
                    {
                        string flightNumber = data[0].Trim();
                        string airlineCode = data[1].Trim();
                        string origin = data[2].Trim();
                        string destination = data[3].Trim();
                        string time = data[4].Trim();

                        flights[flightNumber] = new Flight(flightNumber, airlineCode, origin, destination, time);
                    }
                }
                Console.WriteLine("Flights loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading flights: {ex.Message}");
            }
        }

        // List all flights with basic information
        public void ListFlights()
        {
            Console.WriteLine("\nList of Flights:");
            foreach (var flight in flights.Values)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, Airline: {flight.AirlineCode}, Origin: {flight.Origin}, Destination: {flight.Destination}, Time: {flight.Time}");
            }
        }

        // List all boarding gates
        public void ListBoardingGates()
        {
            Console.WriteLine("\nList of Boarding Gates:");
            foreach (var gate in boardingGates.Values)
            {
                Console.WriteLine($"Gate Number: {gate.GateNumber}, Special Request: {gate.SpecialRequest}, Assigned Flight: {gate.AssignedFlightNumber}");
            }
        }

        public void Start()
        {
            Console.WriteLine("Welcome to Terminal 5 - Flight Information Display System");

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListFlights();
                        break;
                    case "2":
                        ListBoardingGates();
                        break;
                    case "0":
                        Console.WriteLine("Exiting the system. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. List all flights");
            Console.WriteLine("2. List all boarding gates");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }
    }

    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Airline(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    public class BoardingGate
    {
        public string GateNumber { get; set; }
        public string SpecialRequest { get; set; }
        public string AssignedFlightNumber { get; set; }

        public BoardingGate(string gateNumber, string specialRequest)
        {
            GateNumber = gateNumber;
            SpecialRequest = specialRequest;
            AssignedFlightNumber = null;
        }
    }

    public class Flight
    {
        public string FlightNumber { get; set; }
        public string AirlineCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Time { get; set; }

        public Flight(string flightNumber, string airlineCode, string origin, string destination, string time)
        {
            FlightNumber = flightNumber;
            AirlineCode = airlineCode;
            Origin = origin;
            Destination = destination;
            Time = time;
        }
    }
}
