using POH5Luokat;
using System.Collections.Generic;

namespace POH5Data
{
    public class TuoteRyhmaProxy : TuoteRyhma
    {
        List<Tuote> _tuotteet;
        bool TuotteetHaettu = false;

        public TuoteRepository TuoteRepository { get; set; }

        public override List<Tuote> Tuotteet
        {
            get
            {
                if (!TuotteetHaettu) {
                    _tuotteet = TuoteRepository.HaeRyhmanKaikki(Id);
                    TuotteetHaettu = true;
                }
                return (_tuotteet);
            }
            set => base.Tuotteet = value;
        }

        public TuoteRyhmaProxy(int id, string nimi) 
            : base(id, nimi) {

        }
    }
}
