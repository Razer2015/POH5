using System;
using System.Collections.Generic;
using POH5Luokat;
using System.Data;
using System.Data.SqlClient;

namespace POH5Data
{
    public  class ToimittajaRepository : DataAccess, IRepository<Toimittaja>
    {
        public ToimittajaRepository(string conString) : base(conString) {}

        private Toimittaja TeeRivistaToimittaja(IDataReader reader) {
            var paluu = new ToimittajaProxy(int.Parse(reader["SupplierID"].ToString()), reader["CompanyName"].ToString());

            paluu.YhteysHenkilo = (!(reader["ContactName"] is DBNull) ? reader["ContactName"].ToString() : null);
            paluu.YhteysTitteli = (!(reader["ContactTitle"] is DBNull) ? reader["ContactTitle"].ToString() : null);
            paluu.Katuosoite = (!(reader["Address"] is DBNull) ? reader["Address"].ToString() : null);
            paluu.Kaupunki = (!(reader["City"] is DBNull) ? reader["City"].ToString() : null);
            //paluu. = (!(reader["Region"] is DBNull) ? reader["Region"].ToString() : null);
            paluu.PostiKoodi = (!(reader["PostalCode"] is DBNull) ? reader["PostalCode"].ToString() : null);
            paluu.Maa = (!(reader["Country"] is DBNull) ? reader["Country"].ToString() : null);
            //paluu. = (!(reader["Phone"] is DBNull) ? reader["Phone"].ToString() : null);
            //paluu. = (!(reader["Fax"] is DBNull) ? reader["Fax"].ToString() : null);
            //paluu. = (!(reader["HomePage"] is DBNull) ? reader["HomePage"].ToString() : null);

            //Tuote‐olioiden myöhempää populointia varten
            ((ToimittajaProxy)paluu).TuoteRepository = new TuoteRepository(ConnectionString);
            return (paluu);
        }

        private List<Toimittaja> TeeToimittajaLista(IDataReader reader) {
            var toimittajat = new List<Toimittaja>();
            while (reader.Read()) {
                toimittajat.Add(TeeRivistaToimittaja(reader));
            }
            return (toimittajat);
        }

        public Toimittaja Hae(int id) {
            var paluu = new Toimittaja();

            string sql = "SELECT SupplierID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage FROM dbo.Suppliers WHERE SupplierID = @SupplierID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@SupplierID", id));
                        paluu = TeeRivistaToimittaja(cmd.ExecuteReader(CommandBehavior.SingleRow));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public List<Toimittaja> HaeKaikki() {
            var paluu = new List<Toimittaja>();

            string sql = "SELECT SupplierID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage FROM dbo.Suppliers ORDER BY SupplierID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        paluu = TeeToimittajaLista(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Lisaa(Toimittaja item) {
            var paluu = false;

            string sql = "INSERT INTO dbo.Suppliers(CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country) VALUES(@CompanyName, @ContactName, @ContactTitle, @Address, @City, @PostalCode, @Country)";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", item.Nimi));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", item.YhteysHenkilo ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", item.YhteysTitteli ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Address", item.Katuosoite ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@City", item.Kaupunki ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", item.PostiKoodi ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Country", item.Maa ?? (object)DBNull.Value));
                        paluu = (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Muuta(Toimittaja item) {
            var paluu = false;

            string sql = "UPDATE dbo.Suppliers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, PostalCode = @PostalCode, Country = @Country WHERE SupplierID = @SupplierID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@SupplierID", item.Id));

                        cmd.Parameters.Add(new SqlParameter("@CompanyName", item.Nimi));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", item.YhteysHenkilo ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", item.YhteysTitteli ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Address", item.Katuosoite ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@City", item.Kaupunki ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", item.PostiKoodi ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Country", item.Maa ?? (object)DBNull.Value));
                        paluu = (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Poista(int id) {
            var paluu = false;

            string sql = "DELETE FROM dbo.Suppliers WHERE SupplierID = @SupplierID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@SupplierID", id));
                        paluu = (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }
    }
}
