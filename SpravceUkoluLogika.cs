using System;
using System.Collections.Generic;
using System.Text;

namespace SpravceUkolu
{
    internal class SpravceUkoluLogika
    {
        private List<Ukol> _ukoly;

        public SpravceUkoluLogika()
        {
            _ukoly = new List<Ukol>();
        }

        public void PridatUkol(Ukol ukol)
        {
            _ukoly.Add(ukol);
        }
        public List<Ukol> ZiskatVsechnyUkoly()
        {
            return _ukoly;
        }


        public bool SplnitUkol(int i)
        {
            if (i >= 0 && i < _ukoly.Count)
            {
                _ukoly[i].JeSplneno = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void VymazatSplneneUkoly()
        {
            List<Ukol> jenNesplnene = new List<Ukol>();
            for (int i = 0; i < _ukoly.Count; i++)
            {
                if (_ukoly[i].JeSplneno == false)
                {
                    jenNesplnene.Add(_ukoly[i]);
                }
            }
            _ukoly = jenNesplnene;
        }

        public void UlozitDoSouboru()
        {
            SpravceSouboru.UlozitData(_ukoly);
        }
        public void NacistZeSouboru()
        {
            _ukoly = SpravceSouboru.NacistData();
        }
    }
}
