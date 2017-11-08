using POH5Luokat;
using System.Collections.Generic;

namespace POH5Data
{
    public class ToimittajaProxy : Toimittaja
    {
        List<Tuote> _tuotteet;
        bool TuotteetHaettu = false;

        public TuoteRepository TuoteRepository { get; set; }

        public override List<Tuote> Tuotteet
        {
            get
            {
                if (!TuotteetHaettu) {
                    _tuotteet = TuoteRepository.HaeToimittajanKaikki(Id);
                    TuotteetHaettu = true;
                }
                return (_tuotteet);
            }
            set => base.Tuotteet = value;
        }

        public ToimittajaProxy(int id, string nimi) 
            : base(id, nimi) {

        }
    }
}
