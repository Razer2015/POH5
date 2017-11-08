using System;
using System.IO;
using POH5Luokat;
using POH5Data;
using System.Configuration;
using System.Linq;

namespace POH5Testeri
{
    class Program
    {
        static void AsetaDataDirectory() {
            // Asetetaan muuttuja DataDirectory, jota käytetään yhteysmerkkijonossa  
            // tiedostossa App.config

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\..\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        static void Main(string[] args) {
            AsetaDataDirectory();

            var yhteysAsetukset = ConfigurationManager.ConnectionStrings["DB"];
            var tr = new TuoteRepository(yhteysAsetukset.ConnectionString);
            var trr = new TuoteRyhmaRepository(yhteysAsetukset.ConnectionString);

            Demo1(tr);
            Demo2(trr);
            Demo3(trr);
            Demo4(trr);
            Demo5(trr);

            Console.ReadLine();
        }

        static void Demo1(TuoteRepository repo) {
            var tuotteet = repo.HaeKaikki();
            Console.WriteLine($"Tuotteita {tuotteet.Count} kpl");
            while (true) {
                Console.Write("Anna tuotteen id: ");
                if (int.TryParse(Console.ReadLine(), out var id)) {
                    var haettu = tuotteet.Where(x => x.Id.Equals(id)).FirstOrDefault();
                    Console.WriteLine("{0} {1}, toimittaja: {2} {3}, tuoteryhmä: {4} {5}",
                        haettu.Id,
                        haettu.Nimi,
                        haettu.Toimittaja.Id,
                        haettu.Toimittaja.Nimi,
                        haettu.Ryhma,
                        haettu.Ryhma.Nimi);
                }
            }
        }

        static void Demo2(TuoteRyhmaRepository repo) {

        }

        static void Demo3(TuoteRyhmaRepository repo) {

        }

        static void Demo4(TuoteRyhmaRepository repo) {

        }

        static void Demo5(TuoteRyhmaRepository repo) {

        }
    }
}
