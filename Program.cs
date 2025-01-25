//test
// more testing
//pls work

// Program.cs

namespace prg2_final_assgn
{
    class Program
    {
        static void Main(string[] args)
        {
            Terminal terminal = new Terminal();

            terminal.LoadAirlines("airlines.csv");
            terminal.LoadBoardingGates("boardinggates.csv");
            terminal.LoadFlights("flights.csv");

            terminal.Start();
        }
    }
}

