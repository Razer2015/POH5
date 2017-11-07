using System.Collections.Generic;

namespace POH5Luokat
{
    public class TuoteRyhma : IId, INimi
    {
        public int Id { get; private set; }
        public string Nimi { get; set; }

        public string Kuvaus { get; set; }
        public byte[] Kuva { get; set; }

        public virtual List<Tuote> Tuotteet { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TuoteRyhma() {
            Tuotteet = new List<Tuote>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nimi"></param>
        public TuoteRyhma(int id, string nimi) : this() {
            this.Id = id;
            this.Nimi = nimi;
        }

        public override string ToString() {
            return ($"{Id} {Nimi} ({Tuotteet.Count})");
        }
    }
}
