using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prg2_final_assgn
{
    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Airline(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Code}: {Name}";
        }
    }
}

