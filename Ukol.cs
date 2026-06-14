using System;
using System.Collections.Generic;
using System.Text;

namespace SpravceUkolu
{
    internal class Ukol
    {
        public string Nazev { get; set; }
        public string Priorita { get; set; }
        public bool JeSplneno { get; set; }

        public Ukol(string nazev, string priorita)
        {
            Nazev = nazev;
            Priorita = priorita;
            JeSplneno = false;
        }
    }
}
