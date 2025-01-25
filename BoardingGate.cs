using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    public class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = null; 
        }

        public override string ToString()
        {
            var status = Flight == null ? "No flight assigned" : $"Assigned to flight {Flight.FlightNumber}";
            return $"Gate {GateName} - Supports: " +
                   $"CFFT({SupportsCFFT}), DDJB({SupportsDDJB}), LWTT({SupportsLWTT}) - {status}";
        }
    }
}
