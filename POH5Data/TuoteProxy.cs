using POH5Luokat;

namespace POH5Data
{
    public class TuoteProxy : Tuote
    {
        private Toimittaja _toimittaja;
        private TuoteRyhma _ryhma;
        private bool ToimittajaHaettu = false;
        private bool RyhmaHaettu = false;

        public ToimittajaRepository ToimittajaRepository { get; set; }
        public TuoteRyhmaRepository TuoteRyhmaRepository { get; set; }

        public override Toimittaja Toimittaja
        {
            get
            {
                if (base.ToimittajaId.HasValue) {
                    if (!ToimittajaHaettu) {
                        _toimittaja = ToimittajaRepository.Hae(base.ToimittajaId.Value);
                        ToimittajaHaettu = true;
                    }
                    return (_toimittaja);
                }
                return (null);
            }
            set => base.Toimittaja = value;
        }

        public override TuoteRyhma Ryhma
        {
            get
            {
                if (base.RyhmaId.HasValue) {
                    if (!RyhmaHaettu) {
                        _ryhma = TuoteRyhmaRepository.Hae(base.RyhmaId.Value);
                        RyhmaHaettu = true;
                    }
                    return (_ryhma);
                }
                return (null);
            }
            set => base.Ryhma = value;
        }

        public TuoteProxy(int id, string nimi) 
            : base(id, nimi) {

        }
    }
}
