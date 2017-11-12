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
        }

        static void Demo1(TuoteRepository repo) {
            var tuotteet = repo.HaeKaikki();
            Console.WriteLine($"Tuotteita {tuotteet.Count} kpl");
            while (true) {
                Console.Write("Anna tuotteen id: ");
                if (int.TryParse(Console.ReadLine(), out var id)) {
                    if (id < 0) {
                        break;
                    }
                    var haettu = tuotteet.Where(x => x.Id.Equals(id)).FirstOrDefault();
                    if (haettu != default(Tuote)) {
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
        }

        static void Demo2(TuoteRyhmaRepository repo) {
            var tuoteRyhmat = repo.HaeKaikki();
            tuoteRyhmat.ForEach(x => Console.WriteLine("{0} {1}: {2}", x.Id, x.Nimi, x.Kuvaus));
            Console.WriteLine();

            Console.Write("Valitse ryhmä: ");
            if (int.TryParse(Console.ReadLine(), out var id)) {
                var haettu = tuoteRyhmat.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (haettu != default(TuoteRyhma)) {
                    haettu.Tuotteet.ForEach(x => 
                    Console.WriteLine("{0} {1}, yksikköhinta: {2:0.00} / {3}", x.Id, x.Nimi, x.YksikkoHinta, x.YksikkoKuvaus));
                }
            }

            Console.WriteLine("Paina Enter lopettaaksesi...");
            Console.ReadLine();
        }

        static void Demo3(TuoteRyhmaRepository repo) {
            Console.Write("Lisätäänkö uusi (k/e): ");
            var result = Console.ReadLine();
            if (result.Equals("k", StringComparison.OrdinalIgnoreCase)) {
                Console.Write("Anna nimi: ");
                var nimi = Console.ReadLine();
                Console.Write("Anna kuvaus: ");
                var kuvaus = Console.ReadLine();
                var uusiRyhma = new TuoteRyhma() { Nimi = nimi, Kuvaus = (!string.IsNullOrEmpty(kuvaus) ? kuvaus : "NULL") };
                repo.Lisaa(uusiRyhma);

                var tuoteRyhmat = repo.HaeKaikki();
                tuoteRyhmat.ForEach(x => Console.WriteLine("{0} {1}: {2}", x.Id, x.Nimi, x.Kuvaus));
            }

            Console.Write("Paina Enter lopettaaksesi...");
            Console.ReadLine();
        }

        static void Demo4(TuoteRyhmaRepository repo) {
            var tuoteRyhmat = repo.HaeKaikki();
            tuoteRyhmat.ForEach(x => Console.WriteLine("{0} {1}: {2}", x.Id, x.Nimi, x.Kuvaus));
            Console.WriteLine();

            Console.Write("Valitse muutettava: ");
            if (int.TryParse(Console.ReadLine(), out var id)) {
                var haettu = tuoteRyhmat.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (haettu != default(TuoteRyhma)) {
                    Console.Write("Anna nimi: ");
                    var nimi = Console.ReadLine();
                    Console.Write("Anna kuvaus: ");
                    var kuvaus = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nimi)) {
                        haettu.Nimi = nimi;
                    }
                    if (!string.IsNullOrEmpty(kuvaus)) {
                        if (kuvaus.Equals("NULL")) {
                            haettu.Kuvaus = null;
                        }
                        else {
                            haettu.Kuvaus = kuvaus;
                        }
                    }
                    repo.Muuta(haettu);

                    tuoteRyhmat = repo.HaeKaikki();
                    tuoteRyhmat.ForEach(x => Console.WriteLine("{0} {1}: {2}", x.Id, x.Nimi, x.Kuvaus));
                }
            }

            Console.WriteLine("Paina Enter lopettaaksesi...");
            Console.ReadLine();
        }

        static void Demo5(TuoteRyhmaRepository repo) {
            var tuoteRyhmat = repo.HaeKaikki();
            tuoteRyhmat.ForEach(x => Console.WriteLine("{0} {1}: {2}", x.Id, x.Nimi, x.Kuvaus));
            Console.WriteLine();

            Console.Write("Valitse poistettava: ");
            if (int.TryParse(Console.ReadLine(), out var id)) {
                var haettu = tuoteRyhmat.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (haettu != default(TuoteRyhma)) {
                    repo.Poista(haettu.Id);

                    tuoteRyhmat = repo.HaeKaikki();
                    tuoteRyhmat.ForEach(x => Console.WriteLine("{0} {1}: {2}", x.Id, x.Nimi, x.Kuvaus));
                }
            }

            Console.WriteLine("Paina Enter lopettaaksesi...");
            Console.ReadLine();
        }
    }
}
