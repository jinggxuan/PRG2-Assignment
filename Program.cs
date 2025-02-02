using S10266823_PRG2Assignment;
using System.Globalization;

using System;
using System.Collections.Generic;
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
    // Check if the file exists before attempting to read it
    if (!File.Exists("airlines.csv"))
    {
        Console.WriteLine("Error: airlines.csv not found.");
        return; // Exit the function if the file is missing
    }

    // Open the file for reading
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        string? s = sr.ReadLine(); // Read and discard the header line

        // Read each subsequent line until the end of the file
        while ((s = sr.ReadLine()) != null)
        {
            // Split the line by commas to extract airline details
            string[] airlineData = s.Split(',');

            // Ensure the line contains at least the required fields (name and code)
            if (airlineData.Length >= 2)
            {
                string name = airlineData[0].Trim(); // Get airline name
                string code = airlineData[1].Trim(); // Get airline code

                // Add the airline to the dictionary with an empty flight list
                airlines[code] = new Airline(name, code, new Dictionary<string, Flight>());
            }
        }
    }
}


void LoadBoardingGates(Dictionary<string, BoardingGate> boardingGates, Dictionary<string, double> gateFees)
{
    // Check if the boardinggates.csv file exists before attempting to read it
    if (!File.Exists("boardinggates.csv"))
    {
        Console.WriteLine("Error: boardinggates.csv not found.");
        return; // Exit the function if the file is missing
    }

    // Open the file for reading
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string? s = sr.ReadLine(); // Read and discard the header line

        // Process each line in the file
        while ((s = sr.ReadLine()) != null)
        {
            // Split the line by commas to extract boarding gate details
            string[] boardingGate = s.Split(',');

            // Ensure the line contains at least the required four fields
            if (boardingGate.Length >= 4)
            {
                string gateName = boardingGate[0].Trim(); // Extract gate name

                // Convert boolean values from string, ensuring case insensitivity
                bool supportsDDJB = Convert.ToBoolean(boardingGate[1].ToLower());
                bool supportsCFFT = Convert.ToBoolean(boardingGate[2].ToLower());
                bool supportsLWTT = Convert.ToBoolean(boardingGate[3].ToLower());

                // Create a new BoardingGate object with extracted values
                BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);

                // Store the boarding gate in the dictionary
                boardingGates[gateName] = gate;

                // Calculate and store the gate fee in the gateFees dictionary
                gateFees[gateName] = gate.CalculateFees();
            }
        }
    }
}



void LoadFlights(Dictionary<string, Flight> flights, Dictionary<string, Airline> airlines)
{
    // Check if the flights.csv file exists before attempting to read it
    if (!File.Exists("flights.csv"))
    {
        Console.WriteLine("Error: flights.csv not found.");
        return; // Exit the function if the file is missing
    }

    // Open the file for reading
    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        string? line = sr.ReadLine(); // Read and discard the header line

        // Process each line in the file
        while ((line = sr.ReadLine()) != null)
        {
            // Split the line by commas to extract flight details
            string[] flightData = line.Split(',');

            // Ensure the line contains at least Origin, Destination, Expected Time, and Flight Number
            if (flightData.Length >= 4)
            {
                string flightNumber = flightData[0].Trim(); // Extract flight number
                string origin = flightData[1].Trim(); // Extract origin city
                string destination = flightData[2].Trim(); // Extract destination city
                string dateString = flightData[3].Trim(); // Extract expected time as a string

                // Define possible DateTime formats that may appear in flights.csv
                string[] validFormats = {
                    "dd/MM/yyyy HH:mm",    // Example: 25/01/2025 14:30
                    "MM/dd/yyyy HH:mm",    // Example: 01/25/2025 14:30
                    "yyyy-MM-dd HH:mm:ss", // Example: 2025-01-25 14:30:00
                    "yyyy/MM/dd HH:mm",    // Example: 2025/01/25 14:30
                    "h:mm tt",             // Example: 9:30 PM 
                    "hh:mm tt"             // Example: 11:45 AM 
                };

                // If the expected time is missing, skip this flight entry
                if (string.IsNullOrEmpty(dateString))
                {
                    Console.WriteLine($"Warning: Missing expected time for flight '{flightNumber}'. Skipping entry.");
                    continue;
                }

                // Try to parse the expected time using the predefined formats
                if (!DateTime.TryParseExact(dateString, validFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expectedTime))
                {
                    Console.WriteLine($"Warning: Invalid date format '{dateString}' for flight '{flightNumber}'. Skipping entry.");
                    continue; // Skip invalid entries
                }

                // Extract the special request field if present, otherwise set to an empty string
                string specialRequest = flightData.Length >= 5 ? flightData[4].Trim() : "";

                // Extract the airline code from the first two characters of the flight number
                string airlineCode = flightNumber.Substring(0, 2);

                // Create the appropriate flight object based on the special request type
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

                // Store the flight object in the flights dictionary
                flights[flightNumber] = flight;

                // Check if the airline exists in the dictionary and add the flight to it
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
}


void DisplayFlights(Dictionary<string, Flight> flights, Dictionary<string, Airline> airlines)
{
    // Display header
    Console.WriteLine("\n=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================\n");

    // Print column headers
    Console.WriteLine("Flight Number   Airline Name         Origin                    Destination               Expected Departure/Arrival Time");

    // Iterate through all flights and display details
    foreach (var flight in flights.Values)
    {
        string airlineName = "Unknown Airline";

        // Extract the airline code from the first two characters of the flight number
        string airlineCode = flight.FlightNumber.Substring(0, 2);

        // Check if the airline exists in the dictionary
        if (airlines.ContainsKey(airlineCode))
        {
            airlineName = airlines[airlineCode].Name;
        }

        // Format and print flight details
        Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} {flight.Origin,-25} {flight.Destination,-25} {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
    }
}


// List all boarding gates
void DisplayBoardingGates(Dictionary<string, BoardingGate> boardingGates)
{
    // Display the header for the boarding gates list
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================\n");

    // Print column headers with appropriate spacing
    Console.WriteLine($"{"Gate Name",-15} {"DDJB",-10} {"CFFT",-10} {"LWTT",-10}");

    // Iterate through each boarding gate in the dictionary and display its details
    foreach (KeyValuePair<string, BoardingGate> kvp in boardingGates)
    {
        BoardingGate boardingGate = kvp.Value;

        // Format and print boarding gate details
        Console.WriteLine($"{boardingGate.GateName,-15} {boardingGate.SupportsDDJB,-10} {boardingGate.SupportsCFFT,-10} {boardingGate.SupportsLWTT,-10}");
    }
}



// Load the special request codes from flights.csv
void LoadSpecialRequests(Dictionary<string, string> specialRequests)
{
    // Check if the flights.csv file exists before attempting to read it
    if (!File.Exists("flights.csv"))
    {
        Console.WriteLine("Error: flights.csv not found.");
        return; // Exit the function if the file is missing
    }

    // Open the file for reading
    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        string? line = sr.ReadLine(); // Read and discard the header line

        // Process each line in the file
        while ((line = sr.ReadLine()) != null)
        {
            // Split the line by commas to extract flight details
            string[] flightData = line.Split(',');

            // Ensure the line contains at least Flight Number and Special Request Code
            if (flightData.Length >= 5)
            {
                string flightNumber = flightData[0].Trim(); // Extract flight number
                string specialRequest = flightData[4].Trim(); // Extract special request code

                // Store the special request code in the dictionary with the flight number as key
                specialRequests[flightNumber] = specialRequest;
            }
        }
    }
}


// The function to assign a boarding gate
void AssignBoardingGateToFlight(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, string> gateAssignments, Dictionary<string, string> specialRequests)
{
    string flightNumber;
    Flight selectedFlight;

    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");
    Console.WriteLine();

    // Loop until the user enters a valid Flight Number
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        flightNumber = Console.ReadLine()?.Trim();

        // Check if the flight exists in the dictionary
        if (!string.IsNullOrEmpty(flightNumber) && flights.TryGetValue(flightNumber, out selectedFlight))
        {
            break; // Valid flight found, exit loop
        }
        else
        {
            // Inform user if flight is not found and prompt again
            Console.WriteLine("Error: Flight not found. Please enter a valid flight number.");
        }
    }

    // Retrieve Special Request Code (if available)
    string specialRequest = specialRequests.ContainsKey(flightNumber) ? specialRequests[flightNumber] : "None";

    // Display selected flight details
    Console.WriteLine($"\nFlight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {specialRequest}");

    string gateName;
    BoardingGate selectedGate;

    // Loop until a valid and unassigned Boarding Gate is selected
    while (true)
    {
        Console.Write("\nEnter Boarding Gate: ");
        gateName = Console.ReadLine()?.Trim();

        // Check if the gate exists and is not already assigned to another flight
        if (!string.IsNullOrEmpty(gateName) && boardingGates.TryGetValue(gateName, out selectedGate))
        {
            if (!gateAssignments.ContainsKey(gateName))
            {
                gateAssignments[gateName] = flightNumber; // Assign gate to flight
                break;
            }
            else
            {
                // Notify the user if the gate is already assigned to another flight
                Console.WriteLine($"Error: Boarding Gate '{gateName}' is already assigned to flight {gateAssignments[gateName]}. Choose another gate.");
            }
        }
        else
        {
            // Inform user if gate is not found and prompt again
            Console.WriteLine("Error: Boarding Gate not found. Please enter a valid gate name.");
        }
    }

    // Display the selected boarding gate's details
    Console.WriteLine($"\nBoarding Gate Name: {gateName}");
    Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");

    string updateStatus;
    // Ask user whether to update the flight status
    while (true)
    {
        Console.Write("\nWould you like to update the flight status? (Y/N): ");
        updateStatus = Console.ReadLine()?.Trim().ToUpper();

        // Validate the input to only allow 'Y' or 'N'
        if (updateStatus == "Y" || updateStatus == "N")
        {
            break; // Exit loop on valid input
        }
        else
        {
            // Inform user about invalid input
            Console.WriteLine("Error: Invalid input. Please enter 'Y' or 'N'.");
        }
    }

    // If user chooses to update status, present options
    if (updateStatus == "Y")
    {
        int statusOption;

        // Loop until user selects a valid status option
        while (true)
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Please select the new status of the flight: ");

            // Validate user input for status selection
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
                        // Notify invalid selection and prompt again
                        Console.WriteLine("Error: Invalid selection. Please enter 1, 2, or 3.");
                        continue;
                }
                break; // Exit loop once a valid status is selected
            }
            else
            {
                // Inform user about invalid input and prompt again
                Console.WriteLine("Error: Invalid selection. Please enter a number between 1 and 3.");
            }
        }
    }
    else
    {
        // Default flight status if no update is made
        selectedFlight.Status = "On Time";
    }

    // Confirm the assignment of the flight to the boarding gate
    Console.WriteLine($"\nFlight '{selectedFlight.FlightNumber}' has been assigned to Boarding Gate {gateName}!");
}



/// Creates a new flight entry, adding it to the flights dictionary and appending to a CSV file.
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

/// Displays detailed flight information for a selected airline, including details like flight number, origin, destination, etc.
void DisplayFlightDetailsFromAirline(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    // List all available airlines
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine();

    foreach (var airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code} - {airline.Name}");
    }

    // Prompt the user to select an airline by code
    Console.Write("\nEnter 2-letter Airline Code (e.g., SQ or MH): ");
    string airlineCode = Console.ReadLine()?.Trim().ToUpper();

    if (airlines.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine("=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name} ({selectedAirline.Code}):");
        Console.WriteLine("=============================================");

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

/// Allows modifying or deleting existing flight details, including basic information, status, special request code, or gate assignment.
void ModifyFlightDetails(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine();

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
            Console.WriteLine($"{flight.FlightNumber,-20}{selectedAirline.Name,-22}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime:dd/MM/yyyy h:mm tt}");
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

/// Displays a sorted schedule of all flights across airlines at the terminal, including information like flight number, origin, destination, time, etc.
void DisplayFlightSchedule(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine();

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

/// Processes flights without assigned gates and attempts to assign them to available boarding gates, including applying specific requirements for special request codes.
void ProcessUnassignedFlights(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, Airline> airlines)
{
    Queue<Flight> flightQueue = new Queue<Flight>();
    int unassignedFlightsCount = 0;
    int unassignedGatesCount = 0;

    // Collect unassigned flights
    foreach (var flight in flights.Values)
    {
        bool isAssigned = boardingGates.Values.Any(gate => gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber);
        if (!isAssigned)
        {
            flightQueue.Enqueue(flight);
            unassignedFlightsCount++;
        }
    }
    Console.WriteLine($"Total number of Flights that do not have any Boarding Gate assigned yet: {unassignedFlightsCount}");

    // Collect unassigned gates
    foreach (var gate in boardingGates.Values)
    {
        if (gate.Flight == null)
        {
            unassignedGatesCount++;
        }
    }
    Console.WriteLine($"Total number of Boarding Gates that do not have a Flight Number assigned yet: {unassignedGatesCount}");

    int totalProcessedFlights = 0;
    int totalProcessedGates = 0;

    // Assign flights to gates
    while (flightQueue.Count > 0)
    {
        Flight currentFlight = flightQueue.Dequeue();
        BoardingGate? assignedGate = boardingGates.Values.FirstOrDefault(gate => gate.Flight == null);

        // Check for special request
        if (currentFlight is CFFTFlight || currentFlight is DDJBFlight || currentFlight is LWTTFlight)
        {
            assignedGate = boardingGates.Values.FirstOrDefault(gate =>
                gate.Flight == null && (
                    (currentFlight is CFFTFlight && gate.SupportsCFFT) ||
                    (currentFlight is DDJBFlight && gate.SupportsDDJB) ||
                    (currentFlight is LWTTFlight && gate.SupportsLWTT))
            ) ?? assignedGate;
        }

        if (assignedGate != null)
        {
            assignedGate.Flight = currentFlight;
            totalProcessedFlights++;
            totalProcessedGates++;
            Console.WriteLine("\nFlight Details: ");
            Console.WriteLine($"Flight Number: {currentFlight.FlightNumber}");
            if (airlines.TryGetValue(currentFlight.FlightNumber.Substring(0, 2), out Airline airline))
            {
                Console.WriteLine($"Airline Name: {airline.Name}");
            }
            Console.WriteLine($"Origin: {currentFlight.Origin}");
            Console.WriteLine($"Destination: {currentFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {currentFlight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
            Console.WriteLine($"Special Request Code: {(currentFlight is CFFTFlight ? "CFFT" : (currentFlight is DDJBFlight ? "DDJB" : (currentFlight is LWTTFlight ? "LWTT" : "None")))}");
            Console.WriteLine($"Boarding Gate: {assignedGate.GateName}");
        }
    }

    Console.WriteLine($"Total number of Flights processed and assigned: {totalProcessedFlights}");
    Console.WriteLine($"Total number of Boarding Gates processed and assigned: {totalProcessedGates}");

    double percentage = (double)(totalProcessedFlights + totalProcessedGates) / (flights.Count + boardingGates.Count) * 100;
    Console.WriteLine($"Percentage of Flights and Boarding Gates processed automatically: {percentage:F2}%");
}

/// Displays the total fees collected per airline, including any applicable discounts. It calculates the subtotal fees, discounts, and final fees.
void DisplayTotalFeePerAirline(Dictionary<string, Airline> airlines, Dictionary<string, string> specialRequests, Dictionary<string, string> gateAssignments)
{
    bool hasUnassignedGates = gateAssignments.Values.Contains("Unassigned") || gateAssignments.Count == 0;

    if (hasUnassignedGates)
    {
        Console.WriteLine("\nSome flights do not have assigned boarding gates. The Boarding Gate Fee of $300 will only be added for flights with assigned Boarding Gates.\n");
    }

    double totalFeesCollected = 0;
    double totalDiscountsApplied = 0;

    Console.WriteLine("\nTotal Fees Collected Per Airline:\n");
    Console.WriteLine($"{"Airline",-20}{"Subtotal Fees",-18}{"Discounts",-15}{"Final Fees",-15}");
    Console.WriteLine(new string('-', 65));

    foreach (var airline in airlines.Values)
    {
        double subtotalFees = airline.CalculateFees(); // Calculate fees for each airline
        double discounts = 0;

        // Apply promotions and discounts here (the discount logic you provided before)
        int arrivalDepartureCount = 0;
        int flightsBefore11AMOrAfter9PM = 0;
        int flightsFromDXB_BKK_NRT = 0;

        foreach (var flight in airline.Flights.Values)
        {
            // Conditions for promotions and discounts
            if (flight.Destination == "Singapore (SIN)" || flight.Origin == "Singapore (SIN)")
                arrivalDepartureCount++;
            if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                flightsBefore11AMOrAfter9PM++;
            if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                flightsFromDXB_BKK_NRT++;
        }

        // Apply promotion-based discounts
        discounts += 350 * (arrivalDepartureCount / 3);  // Discount for every 3 arriving/departing flights
        discounts += 110 * flightsBefore11AMOrAfter9PM;  // Discount for flights before 11 AM or after 9 PM
        discounts += 25 * flightsFromDXB_BKK_NRT;  // Discount for flights from DXB, BKK, or NRT

        // Additional discount for airlines with more than 5 flights
        if (airline.Flights.Count > 5)
        {
            discounts += subtotalFees * 0.03;  // 3% off total fees
        }

        double finalFee = subtotalFees - discounts;

        totalFeesCollected += finalFee;
        totalDiscountsApplied += discounts;

        Console.WriteLine($"{airline.Name,-20}${subtotalFees,-17:F2}${discounts,-14:F2}${finalFee,-15:F2}");
    }

    Console.WriteLine(new string('-', 65));
    Console.WriteLine($"\n{"Subtotal of all Airline Fees Charged: ",-20}${totalFeesCollected:F2}");
    Console.WriteLine($"{"Subtotal of all Discounts to be deducted: ",-20}${totalDiscountsApplied:F2}");
    Console.WriteLine($"{"Final Total of Airline Fees to Collect: ",-20}${totalFeesCollected - totalDiscountsApplied:F2}");

    double discountPercentage = (totalDiscountsApplied / totalFeesCollected) * 100;
    Console.WriteLine($"{"Discount Percentage: ",-20}{discountPercentage:F2}%\n");
}


void Main()
{
    // Initialize necessary dictionaries
    Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
    Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
    Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
    Dictionary<string, string> specialRequests = new Dictionary<string, string>();
    Dictionary<string, string> gateAssignments = new Dictionary<string, string>();
    Dictionary<string, double> gateFees = new Dictionary<string, double>();

    // Initialize the Terminal object
    Terminal terminal = new Terminal("T5", airlines, flights, boardingGates, gateFees);

    // Load data
    LoadAirlines(terminal.Airlines);
    LoadBoardingGates(terminal.BoardingGates, terminal.GateFees);
    LoadFlights(terminal.Flights, terminal.Airlines);
    LoadSpecialRequests(specialRequests);

    // Print loaded data
    Console.WriteLine("Loading Airlines...");
    Console.WriteLine($"{terminal.Airlines.Count} Airlines Loaded!");
    Console.WriteLine("Loading Boarding Gates...");
    Console.WriteLine($"{terminal.BoardingGates.Count} Boarding Gates Loaded!");
    Console.WriteLine("Loading Flights...");
    Console.WriteLine($"{terminal.Flights.Count} Flights Loaded!");

    // Infinite Loop
    bool exit = false;
    while (!exit)
    {
        Console.WriteLine("\n=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("4. Create Flight");
        Console.WriteLine("5. Display Airline Flights");
        Console.WriteLine("6. Modify Flight Details");
        Console.WriteLine("7. Display Flight Schedule");
        Console.WriteLine("8. Process Unassigned Gates");
        Console.WriteLine("9. Display Total Fee Per Airline");
        Console.WriteLine("0. Exit");
        Console.Write("\nPlease select your option: ");

        string input = Console.ReadLine();
        int option;

        if (int.TryParse(input, out option))
        {
            switch (option)
            {
                case 1:
                    // List All Flights
                    DisplayFlights(terminal.Flights, terminal.Airlines);
                    break;

                case 2:
                    // List Boarding Gates
                    DisplayBoardingGates(terminal.BoardingGates);
                    break;

                case 3:
                    // Assign a Boarding Gate to a Flight
                    AssignBoardingGateToFlight(terminal.Flights, terminal.BoardingGates, gateAssignments, specialRequests);
                    break;

                case 4:
                    // Create Flight
                    CreateNewFlight(terminal.Flights, "flights.csv");
                    break;

                case 5:
                    // Display Airline Flights
                    DisplayFlightDetailsFromAirline(terminal.Airlines, specialRequests, gateAssignments);
                    break;

                case 6:
                    // Modify Flight Details
                    ModifyFlightDetails(terminal.Airlines, specialRequests, gateAssignments);
                    break;

                case 7:
                    // Display Flight Schedule
                    DisplayFlightSchedule(terminal.Airlines, specialRequests, gateAssignments);
                    break;

                case 8:
                    // Process unassigned gates
                    ProcessUnassignedFlights(terminal.Flights, terminal.BoardingGates, terminal.Airlines);
                    break;

                case 9:
                    // Display the total fee per airline for the day
                    DisplayTotalFeePerAirline(terminal.Airlines, specialRequests, gateAssignments);
                    break;

                case 0:
                    // Exit
                    exit = true;
                    Console.WriteLine("Exiting the program...");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
}


Main();
