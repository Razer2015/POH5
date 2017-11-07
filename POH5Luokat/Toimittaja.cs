using System.Collections.Generic;

namespace POH5Luokat
{
    class Toimittaja : IId, INimi
    {
        public int Id { get; private set; }

        public string Nimi { get; set; }
        public string YhteysHenkilo { get; set; }
        public string YhteysTitteli { get; set; }
        public string Katuosoite { get; set; }
        public string PostiKoodi { get; set; }
        public string Kaupunki { get; set; }
        public string Maa { get; set; }

        public virtual List<Tuote> Tuotteet { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Toimittaja() {
            Tuotteet = new List<Tuote>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nimi"></param>
        public Toimittaja(int id, string nimi) : this() {
            this.Id = id;
            this.Nimi = nimi;
        }

        public override string ToString() {
            return ($"{Id} {Nimi} ({Tuotteet.Count})");
        }
    }
}
