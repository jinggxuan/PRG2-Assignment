using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        public Airline(string name, string code, Dictionary<string, Flight> flights)
        {
            Name = name;
            Code = code;
            Flights = new Dictionary<string, Flight>();
        }

        public bool AddFlight(Flight flight)
        {
            // add flight into dictionary Flights, and adds Flight into flights.csv
            // if successful, returns true
            Flights[flight.FlightNumber] = flight;
            return true;
        }

        public double CalculateFees()
        {
            // calculates total fees of airlines for all the flights under it
            // includes discounts
            // use Flight.CalculateFees
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
        }


        public bool RemoveFlight(Flight flight)
        {
            // goes through dictionary to check whether this flight exists
            // removes flight if it exists, returns true
            Flights.Remove(flight.FlightNumber);
            return true;
        }

        public override string ToString()
        {
            return $"Name: {Name} \t Code: {Code} \t Flight: {Flights}";
        }
    }
}

