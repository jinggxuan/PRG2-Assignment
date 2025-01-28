using System;
using S10266823_PRG2Assignment;

//==========================================================
// Student Number	: S10266823
// Student Name	: Maniar Naisha Keyur
// Partner Name	: Wong Jing Xuan
//==========================================================

void LoadAirlines(Dictionary<string, Airline> airlines)
{
    if (!File.Exists("airlines.csv"))
    {
        Console.WriteLine("Error: airlines.csv not found.");
        return;
    }

    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        string? s = sr.ReadLine(); // Skip header
        while ((s = sr.ReadLine()) != null)
        {
            string[] airlineData = s.Split(',');
            if (airlineData.Length >= 2)
            {
                string name = airlineData[0].Trim();
                string code = airlineData[1].Trim();

                // Ensure an empty flight dictionary is initialized
                airlines[code] = new Airline(name, code, new Dictionary<string, Flight>());
            }
        }
    }
    Console.WriteLine("Airlines loaded successfully.");
}

void LoadBoardingGates(Dictionary<string, BoardingGate> boardingGates, Dictionary<string, double> gateFees)
{
    if (!File.Exists("boardinggates.csv"))
    {
        Console.WriteLine("Error: boardinggates.csv not found.");
        return;
    }

    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string? s = sr.ReadLine(); // Skip header
        while ((s = sr.ReadLine()) != null)
        {
            string[] boardingGate = s.Split(',');
            if (boardingGate.Length >= 4)
            {
                string gateName = boardingGate[0].Trim();
                bool supportsDDJB = boardingGate[1].Trim() == "1";
                bool supportsCFFT = boardingGate[2].Trim() == "1";
                bool supportsLWTT = boardingGate[3].Trim() == "1";

                BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);
                boardingGates[gateName] = gate;   // Store in dictionary
                gateFees[gateName] = gate.CalculateFees();  // Store in GateFees
            }
        }
    }
    Console.WriteLine("Boarding gates loaded successfully.");
}

// for checking output of LoadBoardingGates function (delete once code is done)
//Console.WriteLine("\n=== Boarding Gates Loaded ===");
//foreach (var gate in terminal.BoardingGates.Values)
//{
//    if (terminal.GateFees.TryGetValue(gate.GateName, out double fee))
//    {
//        Console.WriteLine($"Gate Name: {gate.GateName}, Fees: {fee}");
//    }
//    else
//    {
//        Console.WriteLine($"Gate Name: {gate.GateName}, Fee: Not Found");
//    }
//}

void LoadFlights(Dictionary<string, Flight> flights)
{
    if (!File.Exists("flights.csv"))
    {
        Console.WriteLine("Error: flights.csv not found.");
        return;
    }

    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        string? line = sr.ReadLine(); // Skip header
        while ((line = sr.ReadLine()) != null)
        {
            string[] flightData = line.Split(',');

            if (flightData.Length >= 4) // Ensure at least 4 fields exist
            {
                string flightNumber = flightData[0].Trim();
                string origin = flightData[1].Trim();
                string destination = flightData[2].Trim();
                DateTime expectedTime = DateTime.Parse(flightData[3].Trim());

                Flight flight;

                // Handle different flight types
                if (flightData.Length >= 5)
                {
                    string flightType = flightData[4].Trim();
                    switch (flightType)
                    {
                        case "CFFT":
                            flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
                            break;
                        case "DDJB":
                            flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
                            break;
                        case "LWTT":
                            flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
                            break;
                        default:
                            flight = new Flight(flightNumber, origin, destination, expectedTime);
                            break;
                    }
                }
                else
                {
                    flight = new Flight(flightNumber, origin, destination, expectedTime);
                }

                flights[flightNumber] = flight;
            }
        }
    }
    Console.WriteLine("Flights loaded successfully.");
}


// Initialize Terminal instance
Terminal terminal = new Terminal("T5", new Dictionary<string, Airline>(), new Dictionary<string, Flight>(), new Dictionary<string, BoardingGate>(), new Dictionary<string, double>());

LoadAirlines(terminal.Airlines);
LoadBoardingGates(terminal.BoardingGates, terminal.GateFees);
LoadFlights(terminal.Flights);


foreach (var flight in terminal.Flights.Values)
{
    Console.WriteLine(flight);
}

// List all boarding gates
void DisplayBoardingGates(Dictionary<string, BoardingGate> boardingGates)
{
    Console.WriteLine($"{"Gate Name",-15} {"DDJB",-10} {"CFFT",-10} {"LWTT",-10}");
    foreach (KeyValuePair<string, BoardingGate> kvp in boardingGates)
    {
        BoardingGate boardingGate = kvp.Value;
        Console.WriteLine($"{boardingGate.GateName,-15} {boardingGate.SupportsDDJB,-10} {boardingGate.SupportsCFFT,-10} {boardingGate.SupportsLWTT,-10}");
    }
}
LoadBoardingGates(terminal.BoardingGates, terminal.GateFees);
DisplayBoardingGates(terminal.BoardingGates);

foreach (var flight in terminal.Flights.Values)
{
    Console.WriteLine(flight);
}
