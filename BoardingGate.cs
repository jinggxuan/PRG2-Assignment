using S10266823_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number	: S10266823
// Student Name	: Maniar Naisha Keyur
// Partner Name	: Wong Jing Xuan
//==========================================================

namespace S10266823_PRG2Assignment
{
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight? Flight { get; set; }  // Nullable flight

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight? flight = null)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }

        public double CalculateFees()
        {
            if (Flight == null) return 0; // No flight assigned to this gate

            double gateFee = 300; // Base gate fee

            return gateFee;
        }


        public override string ToString()
        {
            string flightInfo = Flight != null ? Flight.FlightNumber : "No Flight Assigned";
            return $"Gate Name: {GateName} \t Supports CFFT: {SupportsCFFT} \t Supports DDJB: {SupportsDDJB} \t Supports LWTT: {SupportsLWTT} \t Flight: {flightInfo}";
        }
    }
}
