using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }

        public bool SupportsLWTT { get; set; }
        public Flight flight { get; set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            this.flight = flight;
        }

        public double CalculateFees()
        {
            return 300;
        }

        public override string ToString()
        {
            return $"Gate Name: {GateName} \t Supports CFFT: {SupportsCFFT} \t Supports DDJB: {SupportsDDJB} \t Supports LWTT: {SupportsLWTT}";
        }
    }
}
}
