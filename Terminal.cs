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

        public void LoadAirlines(string filePath) { /* ... */ }
        public void LoadBoardingGates(string filePath) { /* ... */ }
        public void LoadFlights(string filePath) { /* ... */ }
        public void Start() { /* ... */ }
        private void DisplayMenu() { /* ... */ }
        private void ListAirlines() { /* ... */ }
        private void ListBoardingGates() { /* ... */ }
        private void ListFlights() { /* ... */ }
    }
}
