using System;
using System.Collections.Generic;
using POH5Luokat;
using System.Data;
using System.Data.SqlClient;

namespace POH5Data
{
    public class TuoteRepository : DataAccess, IRepository<Tuote>
    {
        public TuoteRepository(string conString) : base(conString) {}

        /// <summary>
        /// Parsii Tuote-olion IDataReaderistä
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Tuote TeeRivistaTuote(IDataReader reader) {
            var paluu = new TuoteProxy(int.Parse(reader["ProductID"].ToString()), reader["ProductName"].ToString()) {
                ToimittajaId = (!(reader["SupplierID"] is DBNull) ? int.Parse(reader["SupplierID"].ToString()) : (int?)null),
                RyhmaId = (!(reader["CategoryID"] is DBNull) ? int.Parse(reader["CategoryID"].ToString()) : (int?)null),
                YksikkoKuvaus = (!(reader["QuantityPerUnit"] is DBNull) ? reader["QuantityPerUnit"].ToString() : null),
                YksikkoHinta = (!(reader["UnitPrice"] is DBNull) ? double.Parse(reader["UnitPrice"].ToString().Replace('.', ',')) : (double?)null),
                VarastoSaldo = (!(reader["UnitsInStock"] is DBNull) ? int.Parse(reader["UnitsInStock"].ToString()) : (int?)null),
                TilausSaldo = (!(reader["UnitsOnOrder"] is DBNull) ? int.Parse(reader["UnitsOnOrder"].ToString()) : (int?)null),
                HalytysRaja = (!(reader["ReorderLevel"] is DBNull) ? int.Parse(reader["ReorderLevel"].ToString()) : (int?)null),

                EiKaytossa = bool.Parse(reader["Discontinued"].ToString())
            };

            //Toimittaja ja TuoteRyhma‐olioiden myöhempää populointia varten
            ((TuoteProxy)paluu).ToimittajaRepository = new ToimittajaRepository(ConnectionString);
            ((TuoteProxy)paluu).TuoteRyhmaRepository = new TuoteRyhmaRepository(ConnectionString);
            return (paluu);
        }

        /// <summary>
        /// Tekee tuotelistan
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<Tuote> TeeTuoteLista(IDataReader reader) {
            var tuotteet = new List<Tuote>();
            while (reader.Read()) {
                tuotteet.Add(TeeRivistaTuote(reader));
            }
            return (tuotteet);
        }

        public Tuote Hae(int id) {
            var paluu = new Tuote();

            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM dbo.Products WHERE ProductID = @ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", id));
                        paluu = TeeRivistaTuote(cmd.ExecuteReader(CommandBehavior.SingleRow));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public List<Tuote> HaeKaikki() {
            var paluu = new List<Tuote>();

            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM dbo.Products ORDER BY ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        paluu = TeeTuoteLista(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public List<Tuote> HaeRyhmanKaikki(int id) {
            var paluu = new List<Tuote>();

            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM dbo.Products WHERE CategoryID = @CategoryID ORDER BY ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", id));
                        paluu = TeeTuoteLista(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public List<Tuote> HaeToimittajanKaikki(int id) {
            var paluu = new List<Tuote>();

            string sql = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM dbo.Products WHERE SupplierID = @SupplierID ORDER BY ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@SupplierID", id));
                        paluu = TeeTuoteLista(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Lisaa(Tuote item) {
            var paluu = false;

            string sql = "INSERT INTO dbo.Products(ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued) VALUES(@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductName", item.Nimi));
                        cmd.Parameters.Add(new SqlParameter("@SupplierID", item.ToimittajaId ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", item.RyhmaId ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@QuantityPerUnit", item.YksikkoKuvaus));
                        cmd.Parameters.Add(new SqlParameter("@UnitPrice", item.YksikkoHinta.HasValue ? item.YksikkoHinta.ToString().Replace(',', '.') : (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@UnitsInStock", item.VarastoSaldo ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@UnitsOnOrder", item.TilausSaldo ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ReorderLevel", item.HalytysRaja ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Discontinued", item.EiKaytossa));
                        paluu = (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Muuta(Tuote item) {
            var paluu = false;

            string sql = "UPDATE dbo.Products SET ProductName = @ProductName, SupplierID = @SupplierID, CategoryID = @CategoryID, QuantityPerUnit = @QuantityPerUnit, UnitPrice = @UnitPrice, UnitsInStock = @UnitsInStock, UnitsOnOrder = @UnitsOnOrder, ReorderLevel = @ReorderLevel, Discontinued = @Discontinued WHERE ProductID = @ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", item.Id));

                        cmd.Parameters.Add(new SqlParameter("@ProductName", item.Nimi));
                        cmd.Parameters.Add(new SqlParameter("@SupplierID", item.ToimittajaId ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", item.RyhmaId ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@QuantityPerUnit", item.YksikkoKuvaus));
                        cmd.Parameters.Add(new SqlParameter("@UnitPrice", item.YksikkoHinta.HasValue ? item.YksikkoHinta.ToString().Replace(',', '.') : (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@UnitsInStock", item.VarastoSaldo ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@UnitsOnOrder", item.TilausSaldo ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ReorderLevel", item.HalytysRaja ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Discontinued", item.EiKaytossa));
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

            string sql = "DELETE FROM dbo.Products WHERE ProductID = @ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", id));
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
