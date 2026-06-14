using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SpravceUkolu
{
    internal class SpravceSouboru
    {
        private static string _cestaKSouboru = "ukoly.json";


        public static void UlozitData(List<Ukol> seznamUkoly)
        {
            string jsonText = JsonSerializer.Serialize(seznamUkoly);
            File.WriteAllText(_cestaKSouboru, jsonText);
        }

        public static List<Ukol> NacistData()
        {
            if (File.Exists(_cestaKSouboru) == false)
            {
                return new List<Ukol>();
            }
            else
            {
                string jsonText = File.ReadAllText(_cestaKSouboru);
                List<Ukol> nacteneUkoly = JsonSerializer.Deserialize<List<Ukol>>(jsonText);
                return nacteneUkoly;
            }
        }
    }
}
