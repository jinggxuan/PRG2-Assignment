using System;
using PRG2-Assignment;

void load_airlines(Dictionary<string, Airline> airlines)
{
    //load the airlines
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] airline = s.Split(',');
            string name = airline[0];
            string code = airline[1];
            airlines.Add(code, new Airline(name, code));
        }
    }
}
load_airlines(airlines);
void Load_boardingGate(Dictionary<string, BoardingGate> boardingGates)
{
    //load the boarding gates
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] boardingGate = s.Split(',');
            string gateName = boardingGate[0];
            bool supportsCFFT = Convert.ToBoolean((boardingGate[2]));
            bool supportsDDJB = Convert.ToBoolean((boardingGate[1]));
            bool supportsLWTT = Convert.ToBoolean((boardingGate[3]));
            BoardingGate boardinggate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT,null);
            boardingGates.Add(gateName, boardinggate);
        }
    }
}
Load_boardingGate(boardingGates);
