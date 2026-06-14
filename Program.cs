
namespace SpravceUkolu
{
    internal class Program
    {
        static SpravceUkoluLogika spravce = new SpravceUkoluLogika();
        static void Main(string[] args)
        {
            spravce.NacistZeSouboru();

            bool menu = true;
            while (menu)
            {
                Console.Clear();
                ZobrazHlavicku();

                Console.WriteLine("1 - Zobrazit všechny úkoly");
                Console.WriteLine("2 - Přidat nový úkol");
                Console.WriteLine("3 - Označit úkol za splněný");
                Console.WriteLine("4 - Ukončit program");
                Console.WriteLine("=======================================");
                Console.Write("Vyberte možnost (1-4): ");

                string volba = Console.ReadLine();

                switch (volba)
                {
                    case "1":
                        ZobrazitUkoly();
                        CekejNaStisk();
                        break;

                    case "2":
                        PridatUkol();
                        CekejNaStisk();
                        break;

                    case "3":
                        SplnitUkol();
                        CekejNaStisk();
                        break;

                    case "4":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Čistím splněné úkoly, ukládám data...");

                        spravce.VymazatSplneneUkoly();
                        spravce.UlozitDoSouboru();

                        Console.WriteLine("Data úspěšně uložena.");
                        Console.ResetColor();
                        menu = false;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Chyba: Neplatná volba!");
                        Console.ResetColor();
                        CekejNaStisk();
                        break;
                }
            }
        }

        static void ZobrazHlavicku()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================");
            Console.WriteLine("             Správce úkolů             ");
            Console.WriteLine("=======================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void PridatUkol()
        {
            Console.Clear();
            ZobrazHlavicku();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("PŘIDÁNÍ NOVÉHO ÚKOLU:");
            Console.ResetColor();
            Console.WriteLine();

            string nazev = "";
            while (nazev.Length == 0)
            {
                Console.Write("Zadejte název úkolu: ");
                nazev = Console.ReadLine();

                if (nazev.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chyba: Název nesmí být prázdný!");
                    Console.ResetColor();
                }
            }

            string priorita = NactiPrioritu();
            Ukol novyUkol = new Ukol(nazev, priorita);
            spravce.PridatUkol(novyUkol);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Úkol byl úspěšně přidán!");
            Console.ResetColor();
        }
        static string NactiPrioritu()
        {
            while (true)
            {
                Console.WriteLine("Vyberte prioritu:");
                Console.WriteLine("1 - Vysoká");
                Console.WriteLine("2 - Střední");
                Console.WriteLine("3 - Nízká");
                Console.Write("Vaše volba (1-3): ");

                string volba = Console.ReadLine();

                if (volba == "1")
                {
                    return "Vysoká";
                }

                if (volba == "2")
                {
                    return "Střední";
                }
                if (volba == "3")
                {
                    return "Nízká";
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chyba: Neplatná volba.");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        static void ZobrazitUkoly()
        {
            Console.Clear();
            ZobrazHlavicku();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("VŠECHNY ÚKOLY:");
            Console.ResetColor();
            Console.WriteLine();

            List<Ukol> vsechnyUkoly = spravce.ZiskatVsechnyUkoly();

            if (vsechnyUkoly.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Seznam úkolů je prázdný. Přidejte nějaký úkol!");
                Console.ResetColor();
                return;
            }

            for (int i = 0; i < vsechnyUkoly.Count; i++)
            {
                Ukol ukol = vsechnyUkoly[i];

                string ikonaStatusu = "";
                if (ukol.JeSplneno == true)
                {
                    ikonaStatusu = "[X]";
                }
                else
                {
                    ikonaStatusu = "[ ]";
                }

                int cisloUkoly = i + 1;
                Console.Write(cisloUkoly + ". ");

                if (ukol.JeSplneno == true)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(ikonaStatusu + " [" + ukol.Priorita + "] priorita " + ukol.Nazev + " (Splněno)");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(ikonaStatusu + " [");
                    if (ukol.Priorita == "Vysoká")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (ukol.Priorita == "Střední")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (ukol.Priorita == "Nízká")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.Write(ukol.Priorita);
                    Console.ResetColor();

                    Console.WriteLine("] " + "priorita pro úkol " + ukol.Nazev);
                }
            }
        }

        static void SplnitUkol()
        {
            Console.Clear();
            ZobrazHlavicku();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("OZNAČIT ÚKOL ZA SPLNĚNÝ: ");
            Console.ResetColor();
            Console.WriteLine();

            List<Ukol> vsechnyUkoly = spravce.ZiskatVsechnyUkoly();

            if (vsechnyUkoly.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Seznam úkolů je prázdný. Nemáte co splnit.");
                Console.ResetColor();
                return;
            }

            for (int i = 0; i < vsechnyUkoly.Count; i++)
            {
                Ukol ukol = vsechnyUkoly[i];
                string status = "";
                if (ukol.JeSplneno == true)
                {
                    status = "[X]";
                }
                else
                {
                    status = "[ ]";
                }
                Console.WriteLine((i + 1) + ". " + status + " " + ukol.Nazev);
            }

            Console.WriteLine("=======================================");
            Console.Write("Zadejte číslo úkolu, který jste splnil: ");
            string vstup = Console.ReadLine();

            int cisloUkoly;
            bool jeToCislo = int.TryParse(vstup, out cisloUkoly);

            if (jeToCislo == true)
            {
                int index = cisloUkoly - 1;

                bool uspech = spravce.SplnitUkol(index);

                if (uspech == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Úkol byl úspěšně splněn!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chyba: Úkol s tímto číslem neexistuje!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chyba: Musíte zadat platné číslo!");
                Console.ResetColor();
            }
        }

        static void CekejNaStisk()
        {
            Console.WriteLine("Stiskněte libovolnou klávesu pro návrat do menu...");
            Console.ReadKey(true);
        }
    }
}
