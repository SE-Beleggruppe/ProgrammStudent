using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDummyStudent
{
    public class Thema
    {
        public int ThemenNummer { get; set; }
        public string AufgabenName { get; set; }

        public Thema(int themenNummer, string aufgabe)
        {
            AufgabenName = aufgabe;
            ThemenNummer = themenNummer;
        }
    }
}
