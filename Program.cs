using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using Microsoft.VisualBasic;
using S10266823_PRG2Assignment;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                bool supportsDDJB = Convert.ToBoolean(boardingGate[1].ToLower());
                bool supportsCFFT = Convert.ToBoolean(boardingGate[2].ToLower());
                bool supportsLWTT = Convert.ToBoolean(boardingGate[3].ToLower());

                BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);
                boardingGates[gateName] = gate;   // Store in dictionary
                gateFees[gateName] = gate.CalculateFees();  // Store in GateFees
            }
        }
    }
    Console.WriteLine("Boarding gates loaded successfully.");
}


void LoadFlights(Dictionary<string, Flight> flights, Dictionary<string, Airline> airlines)
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

            if (flightData.Length >= 4) // Ensure at least Origin, Destination, Expected Time, and Flight Number
            {
                string flightNumber = flightData[0].Trim();
                string origin = flightData[1].Trim();
                string destination = flightData[2].Trim();
                string dateString = flightData[3].Trim(); // Expected time as string

                // Define possible DateTime formats in flights.csv
                string[] validFormats = {
                    "dd/MM/yyyy HH:mm",    // Example: 25/01/2025 14:30
                    "MM/dd/yyyy HH:mm",    // Example: 01/25/2025 14:30
                    "yyyy-MM-dd HH:mm:ss", // Example: 2025-01-25 14:30:00
                    "yyyy/MM/dd HH:mm",    // Example: 2025/01/25 14:30
                    "h:mm tt",             // Example: 9:30 PM 
                    "hh:mm tt"             // Example: 11:45 AM 
                };

                // Try parsing with the expected formats
                if (string.IsNullOrEmpty(dateString))
                {
                    Console.WriteLine($"Warning: Missing expected time for flight '{flightNumber}'. Skipping entry.");
                    continue; // Skip this flight entry
                }

                if (!DateTime.TryParseExact(dateString, validFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expectedTime))
                {
                    Console.WriteLine($"Warning: Invalid date format '{dateString}' for flight '{flightNumber}'. Skipping entry.");
                    continue; // Skip invalid entries
                }

                string specialRequest = flightData.Length >= 5 ? flightData[4].Trim() : ""; // Optional Special Request
                string airlineCode = flightNumber.Substring(0, 2); // Extract first two characters as airline code

                // ✅ Using long-form switch statement
                Flight flight;
                switch (specialRequest)
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

                flights[flightNumber] = flight;

                if (airlines.ContainsKey(airlineCode))
                {
                    airlines[airlineCode].Flights[flightNumber] = flight;
                }
                else
                {
                    Console.WriteLine($"Warning: Airline code '{airlineCode}' not found for flight '{flightNumber}'.");
                }
            }
        }
    }
    Console.WriteLine("Flights loaded successfully.");
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

// Initialize Terminal instance
Terminal terminal = new Terminal("T5", new Dictionary<string, Airline>(), new Dictionary<string, Flight>(), new Dictionary<string, BoardingGate>(), new Dictionary<string, double>());

LoadAirlines(terminal.Airlines);
LoadBoardingGates(terminal.BoardingGates, terminal.GateFees);
LoadFlights(terminal.Flights, terminal.Airlines); // Now associates flights with airlines

// Print all flights
Console.WriteLine("\n=== Flights Loaded ===");
foreach (var flight in terminal.Flights.Values)
{
    Console.WriteLine(flight);
}

// Print all boarding gates
DisplayBoardingGates(terminal.BoardingGates);


// Assuming we load special request codes into a dictionary
Dictionary<string, string> specialRequests = new Dictionary<string, string>();

// Load the special request codes from flights.csv
void LoadSpecialRequests(Dictionary<string, string> specialRequests)
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

            if (flightData.Length >= 5) // Ensure we have the Flight Number and Special Request Code
            {
                string flightNumber = flightData[0].Trim();
                string specialRequest = flightData[4].Trim();

                // Store the special request code in the dictionary
                specialRequests[flightNumber] = specialRequest;
            }
        }
    }
}

LoadSpecialRequests(specialRequests);


// Dictionary to track which gate is assigned to which flight
Dictionary<string, string> gateAssignments = new Dictionary<string, string>();

// The function to assign a boarding gate
void AssignBoardingGateToFlight(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, string> specialRequests)
{
    string flightNumber;
    Flight selectedFlight;

    // Loop until the user enters a valid Flight Number
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        flightNumber = Console.ReadLine()?.Trim();

        if (!string.IsNullOrEmpty(flightNumber) && flights.TryGetValue(flightNumber, out selectedFlight))
        {
            break; // Valid flight found
        }
        else
        {
            Console.WriteLine("Error: Flight not found. Please enter a valid flight number.");
        }
    }

    // Retrieve Special Request Code (if available)
    string specialRequest = specialRequests.ContainsKey(flightNumber) ? specialRequests[flightNumber] : "None";

    // Display flight details
    Console.WriteLine($"\nFlight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {specialRequest}");

    string gateName;
    BoardingGate selectedGate;

    // Loop until a valid and unassigned Boarding Gate is entered
    while (true)
    {
        Console.Write("\nEnter Boarding Gate: ");
        gateName = Console.ReadLine()?.Trim();

        if (!string.IsNullOrEmpty(gateName) && boardingGates.TryGetValue(gateName, out selectedGate))
        {
            if (!gateAssignments.ContainsKey(gateName))
            {
                gateAssignments[gateName] = flightNumber; // Assign gate
                break;
            }
            else
            {
                Console.WriteLine($"Error: Boarding Gate '{gateName}' is already assigned to flight {gateAssignments[gateName]}. Choose another gate.");
            }
        }
        else
        {
            Console.WriteLine("Error: Boarding Gate not found. Please enter a valid gate name.");
        }
    }

    // Display Boarding Gate Information
    Console.WriteLine($"\nBoarding Gate Name: {gateName}");
    Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");

    string updateStatus;
    while (true)
    {
        Console.Write("\nWould you like to update the flight status? (Y/N): ");
        updateStatus = Console.ReadLine()?.Trim().ToUpper();

        if (updateStatus == "Y" || updateStatus == "N")
        {
            break;
        }
        else
        {
            Console.WriteLine("Error: Invalid input. Please enter 'Y' or 'N'.");
        }
    }

    if (updateStatus == "Y")
    {
        int statusOption;

        while (true)
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Please select the new status of the flight: ");

            if (int.TryParse(Console.ReadLine(), out statusOption))
            {
                switch (statusOption)
                {
                    case 1:
                        selectedFlight.Status = "Delayed";
                        break;
                    case 2:
                        selectedFlight.Status = "Boarding";
                        break;
                    case 3:
                        selectedFlight.Status = "On Time";
                        break;
                    default:
                        Console.WriteLine("Error: Invalid selection. Please enter 1, 2, or 3.");
                        continue;
                }
                break;
            }
            else
            {
                Console.WriteLine("Error: Invalid selection. Please enter a number between 1 and 3.");
            }
        }
    }
    else
    {
        selectedFlight.Status = "On Time"; // Default status
    }

    Console.WriteLine($"\nFlight '{selectedFlight.FlightNumber}' has been assigned to Boarding Gate {gateName}!");
}


// AssignBoardingGateToFlight(terminal.Flights, terminal.BoardingGates, specialRequests);


void CreateNewFlight(Dictionary<string, Flight> flights, string flightsCsvPath)
{
    while (true) // Loop to allow multiple flight additions
    {
        string flightNumber, origin, destination, expectedTimeStr, specialRequestCode;
        DateTime expectedTime;

        // Get Flight Number (must be unique)
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            flightNumber = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(flightNumber) && !flights.ContainsKey(flightNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: Invalid or duplicate Flight Number. Please enter a unique Flight Number.");
            }
        }

        // Get Origin
        while (true)
        {
            Console.Write("Enter Origin: ");
            origin = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(origin))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: Origin cannot be empty. Please enter a valid Origin.");
            }
        }

        // Get Destination
        while (true)
        {
            Console.Write("Enter Destination: ");
            destination = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(destination))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: Destination cannot be empty. Please enter a valid Destination.");
            }
        }

        // Get Expected Departure/Arrival Time in dd/MM/yyyy HH:mm format
        while (true)
        {
            Console.Write("Enter Expected Departure/Arrival Time (e.g., 01/29/2025 10:30 AM): ");
            expectedTimeStr = Console.ReadLine()?.Trim();

            if (DateTime.TryParse(expectedTimeStr, out expectedTime))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: Invalid date format. Please enter a valid date and time.");
            }
        }

        // Get Special Request Code (Must be one of the valid options)
        while (true)
        {
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            specialRequestCode = Console.ReadLine()?.Trim().ToUpper();

            if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || specialRequestCode == "NONE")
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: Invalid Special Request Code. Please enter CFFT, DDJB, LWTT, or None.");
            }
        }

        // Create the Flight Object
        Flight newFlight = new Flight(flightNumber, origin, destination, expectedTime);


        // Add to the Dictionary
        flights.Add(flightNumber, newFlight);

        // Append to flights.csv
        try
        {
            using (StreamWriter writer = new StreamWriter(flightsCsvPath, true))
            {
                writer.WriteLine($"{flightNumber},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{(specialRequestCode == "NONE" ? "" : specialRequestCode)}");
            }
            Console.WriteLine($"\nFlight {flightNumber} has been added!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to CSV file: {ex.Message}");
        }

        // Ask if the user wants to add another flight
        Console.Write("Would you like to add another flight? (Y/N): ");
        string response = Console.ReadLine()?.Trim().ToUpper();

        if (response != "Y")
        {
            break;
        }
    }
}

// CreateNewFlight(terminal.Flights, "flights.csv");


void DisplayFlightDetailsFromAirline(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    // List all available airlines
    Console.WriteLine("\nAvailable Airlines:");
    foreach (var airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code} - {airline.Name}");
    }

    // Prompt the user to select an airline by code
    Console.Write("\nEnter 2-letter Airline Code (e.g., SQ or MH): ");
    string airlineCode = Console.ReadLine()?.Trim().ToUpper();

    if (airlines.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine($"\nFlights for Airline {selectedAirline.Name} ({selectedAirline.Code}):");

        // Display each flight's details
        foreach (var flight in selectedAirline.Flights.Values)
        {
            Console.WriteLine($"Flight Number: {flight.FlightNumber}, Origin: {flight.Origin}, Destination: {flight.Destination}");
        }

        // Prompt to select a flight number
        Console.Write("\nEnter Flight Number to view details: ");
        string flightNumber = Console.ReadLine()?.Trim();

        if (selectedAirline.Flights.TryGetValue(flightNumber, out Flight selectedFlight))
        {
            Console.WriteLine("\nFlight Details:");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Status: {selectedFlight.Status}");
            Console.WriteLine($"Special Request Code: {(specialRequests.ContainsKey(flightNumber) ? specialRequests[flightNumber] : "None")}");
            Console.WriteLine($"Boarding Gate: {(gateAssignments.ContainsKey(flightNumber) ? gateAssignments[flightNumber] : "Not assigned")}");
        }
        else
        {
            Console.WriteLine("Error: Flight not found.");
        }
    }
    else
    {
        Console.WriteLine("Error: Airline code not found.");
    }
}

// DisplayFlightDetailsFromAirline(terminal.Airlines, specialRequests, gateAssignments);

void ModifyFlightDetails(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    // Display the airlines with their codes and names
    Console.WriteLine("Airline Code    Airline Name");
    foreach (var airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code}              {airline.Name}");
    }

    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine()?.Trim().ToUpper();

    if (airlines.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        // Display the list of flights for the selected airline
        Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
        Console.WriteLine($"{"Flight Number",-20}{"Airline",-22}{"Origin",-20}{"Destination",-20}{"Expected Departure/Arrival Time",-20}");
        foreach (var flight in selectedAirline.Flights.Values)
        {
            Console.WriteLine($"{flight.FlightNumber, -20}{selectedAirline.Name, -22}{flight.Origin, -20}{flight.Destination,-20}{flight.ExpectedTime:dd/MM/yyyy h:mm tt}");
        }

        Console.Write("\nChoose an existing Flight to modify or delete: ");
        string flightNumber = Console.ReadLine()?.Trim();

        if (selectedAirline.Flights.TryGetValue(flightNumber, out Flight selectedFlight))
        {
            Console.WriteLine("\n1. Modify Flight");
            Console.WriteLine("2. Delete Flight");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                // Choose modification options
                Console.WriteLine("\n1. Modify Basic Information");
                Console.WriteLine("2. Modify Status");
                Console.WriteLine("3. Modify Special Request Code");
                Console.WriteLine("4. Modify Boarding Gate");
                Console.Write("Choose an option: ");
                string modifyChoice = Console.ReadLine()?.Trim();

                if (modifyChoice == "1")
                {
                    // Modify basic information (Origin, Destination, Departure time)
                    Console.Write("Enter new Origin: ");
                    selectedFlight.Origin = Console.ReadLine()?.Trim();

                    Console.Write("Enter new Destination: ");
                    selectedFlight.Destination = Console.ReadLine()?.Trim();

                    Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy hh:mm): ");
                    string timeInput = Console.ReadLine()?.Trim();
                    if (DateTime.TryParseExact(timeInput, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime newTime))
                    {
                        selectedFlight.ExpectedTime = newTime;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format.");
                    }

                    Console.WriteLine("Flight updated!");
                    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
                    Console.WriteLine($"Origin: {selectedFlight.Origin}");
                    Console.WriteLine($"Destination: {selectedFlight.Destination}");
                    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/MM/yyyy h:mm tt}");
                    Console.WriteLine($"Status: {selectedFlight.Status}");
                    Console.WriteLine($"Special Request Code: {specialRequests.GetValueOrDefault(selectedFlight.FlightNumber, "None")}");
                    Console.WriteLine($"Boarding Gate: {gateAssignments.GetValueOrDefault(selectedFlight.FlightNumber, "Unassigned")}");
                }
                else if (modifyChoice == "2")
                {
                    // Modify Status
                    Console.Write("Enter new Status: ");
                    selectedFlight.Status = Console.ReadLine()?.Trim();
                }
                else if (modifyChoice == "3")
                {
                    // Modify Special Request Code
                    Console.Write("Enter new Special Request Code: ");
                    specialRequests[selectedFlight.FlightNumber] = Console.ReadLine()?.Trim().ToUpper();
                }
                else if (modifyChoice == "4")
                {
                    // Modify Boarding Gate
                    Console.Write("Enter new Boarding Gate: ");
                    gateAssignments[selectedFlight.FlightNumber] = Console.ReadLine()?.Trim();
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            else if (choice == "2")
            {
                // Confirm deletion
                Console.Write("Are you sure you want to delete this flight? (Y/N): ");
                string confirmation = Console.ReadLine()?.Trim().ToUpper();
                if (confirmation == "Y")
                {
                    selectedAirline.Flights.Remove(flightNumber);
                    Console.WriteLine($"Flight {flightNumber} deleted successfully.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
        else
        {
            Console.WriteLine("Error: Flight not found.");
        }
    }
    else
    {
        Console.WriteLine("Error: Airline code not found.");
    }
}

// ModifyFlightDetails(terminal.Airlines, specialRequests, gateAssignments);

void DisplayFlightSchedule(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    List<Flight> allFlights = new List<Flight>();

    // Collect all flights from all airlines
    foreach (var airline in airlines.Values)
    {
        allFlights.AddRange(airline.Flights.Values);
    }

    // Sort the flights by ExpectedTime (chronologically)
    allFlights.Sort();

    // Display the header
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-22}{"Origin",-22}{"Destination",-22}{"Expected Departure/Arrival Time"}\n{"Status",-15}{"Special Request Code",-22}{"Boarding Gate",-15}");
    Console.WriteLine(new string('-', 160)); 
    // Display flight details
    foreach (var flight in allFlights)
    {
        string specialRequest = specialRequests.GetValueOrDefault(flight.FlightNumber, "None");
        string gateAssignment = gateAssignments.GetValueOrDefault(flight.FlightNumber, "Unassigned");

        // Find the airline that matches this flight
        string airlineName = "Unknown Airline";
        foreach (var airline in airlines.Values)
        {
            if (airline.Flights.ContainsKey(flight.FlightNumber))
            {
                airlineName = airline.Name;
                break;
            }
        }

        Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-22}{flight.Origin,-22}{flight.Destination,-22}{flight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}\n{flight.Status,-15}{specialRequest,-22}{gateAssignment,-15}");
    }
}



DisplayFlightSchedule(terminal.Airlines, specialRequests, gateAssignments);
