
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
    static void Main(string[] args)
        {
            Dictionary<string, Airline> airlineDictionary = new Dictionary<string, Airline>();
            Dictionary<string, BoardingGate> boardingGateDictionary = new Dictionary<string, BoardingGate>();
            Dictionary<string, double> gateFeesDictionary = new Dictionary<string, double>();

            // Load the airlines
            LoadAirlines("airlines.csv", airlineDictionary);

            // Load the boarding gates
            LoadBoardingGates("boardinggates.csv", boardingGateDictionary, gateFeesDictionary);

            // Display the loaded data
            Console.WriteLine("Airlines:");
            foreach (var airline in airlineDictionary.Values)
            {
                Console.WriteLine(airline);
            }

            Console.WriteLine("\nBoarding Gates:");
            foreach (var gate in boardingGateDictionary.Values)
            {
                Console.WriteLine(gate);
            }

            Console.WriteLine("\nGate Fees:");
            foreach (var gateFee in gateFeesDictionary)
            {
                Console.WriteLine($"Gate: {gateFee.Key}, Fee: {gateFee.Value:C}");
            }
        }

        static void LoadAirlines(string filePath, Dictionary<string, Airline> airlineDictionary)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    reader.ReadLine(); // Skip header row
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == 2)
                        {
                            string code = data[0].Trim();
                            string name = data[1].Trim();
                            Airline airline = new Airline(code, name);
                            airlineDictionary[code] = airline;
                        }
                    }
                }
                Console.WriteLine("Airlines loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading airlines: {ex.Message}");
            }
        }

        static void LoadBoardingGates(string filePath, Dictionary<string, BoardingGate> boardingGateDictionary, Dictionary<string, double> gateFeesDictionary)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    reader.ReadLine(); // Skip header row
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length >= 4)
                        {
                            string gateName = data[0].Trim();
                            bool supportsCFFT = bool.Parse(data[1].Trim());
                            bool supportsDDJB = bool.Parse(data[2].Trim());
                            bool supportsLWTT = bool.Parse(data[3].Trim());

                            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);
                            boardingGateDictionary[gateName] = gate;

                            // Calculate Gate Fees and store in the dictionary
                            double fee = gate.CalculateFees();
                            gateFeesDictionary[gateName] = fee;
                        }
                    }
                }
                Console.WriteLine("Boarding gates loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }
    }
}

