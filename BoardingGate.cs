using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
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
            double baseFee = 300;
            if (Flight is CFFTFlight) baseFee += 150;
            if (Flight is DDJBFlight) baseFee += 300;
            if (Flight is LWTTFlight) baseFee += 500;
            return baseFee;
        }

        public override string ToString()
        {
            string flightInfo = Flight != null ? Flight.FlightNumber : "No Flight Assigned";
            return $"Gate Name: {GateName} \t Supports CFFT: {SupportsCFFT} \t Supports DDJB: {SupportsDDJB} \t Supports LWTT: {SupportsLWTT} \t Flight: {flightInfo}";
        }
    }
}
