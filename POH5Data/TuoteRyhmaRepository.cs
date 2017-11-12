using System;
using System.Collections.Generic;
using POH5Luokat;
using System.Data;
using System.Data.SqlClient;

namespace POH5Data
{
    public class TuoteRyhmaRepository : DataAccess, IRepository<TuoteRyhma>
    {
        public TuoteRyhmaRepository(string conString) : base(conString) {}

        private TuoteRyhma TeeRivistaTuoteRyhma(IDataReader reader) {
            var paluu = new TuoteRyhmaProxy(int.Parse(reader["CategoryID"].ToString()), reader["CategoryName"].ToString());

            paluu.Kuvaus = (!(reader["Description"] is DBNull) ? reader["Description"].ToString() : null);
            paluu.Kuva = (!(reader["Picture"] is DBNull) ? (byte[])reader["Picture"] : null);

            //Tuote‐olioiden myöhempää populointia varten
            ((TuoteRyhmaProxy)paluu).TuoteRepository = new TuoteRepository(ConnectionString);
            return (paluu);
        }

        private List<TuoteRyhma> TeeTuoteRyhmaLista(IDataReader reader) {
            var tuoteRyhmat = new List<TuoteRyhma>();
            while (reader.Read()) {
                tuoteRyhmat.Add(TeeRivistaTuoteRyhma(reader));
            }
            return (tuoteRyhmat);
        }

        public TuoteRyhma Hae(int id) {
            var paluu = new TuoteRyhma();

            string sql = "SELECT CategoryID, CategoryName, Description, Picture FROM dbo.Categories WHERE CategoryID = @CategoryID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", id));
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        reader.Read();
                        paluu = TeeRivistaTuoteRyhma(reader);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public List<TuoteRyhma> HaeKaikki() {
            var paluu = new List<TuoteRyhma>();

            string sql = "SELECT CategoryID, CategoryName, Description, Picture FROM dbo.Categories ORDER BY CategoryID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        paluu = TeeTuoteRyhmaLista(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Lisaa(TuoteRyhma item) {
            var paluu = false;

            string sql = "INSERT INTO dbo.Categories(CategoryName, Description, Picture) VALUES(@CategoryName, @Description, @Picture)";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CategoryName", item.Nimi));
                        cmd.Parameters.Add(new SqlParameter("@Description", item.Kuvaus ?? (object)DBNull.Value));
                        if (item.Kuva == null) {
                            SqlParameter imageParameter = new SqlParameter("@Picture", SqlDbType.Image);
                            imageParameter.Value = DBNull.Value;
                            cmd.Parameters.Add(imageParameter);
                        }
                        else {
                            cmd.Parameters.AddWithValue("@Picture", item.Kuva);
                        }
                        paluu = (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }

            return (paluu);
        }

        public bool Muuta(TuoteRyhma item) {
            var paluu = false;

            string sql = "UPDATE dbo.Categories SET CategoryName = @CategoryName, Description = @Description, Picture = @Picture WHERE CategoryID = @CategoryID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", item.Id));

                        cmd.Parameters.Add(new SqlParameter("@CategoryName", item.Nimi));
                        cmd.Parameters.Add(new SqlParameter("@Description", item.Kuvaus ?? (object)DBNull.Value));
                        if (item.Kuva == null) {
                            SqlParameter imageParameter = new SqlParameter("@Picture", SqlDbType.Image);
                            imageParameter.Value = DBNull.Value;
                            cmd.Parameters.Add(imageParameter);
                        }
                        else {
                            cmd.Parameters.AddWithValue("@Picture", item.Kuva);
                        }
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

            string sql = "DELETE FROM dbo.Categories WHERE CategoryID = @CategoryID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", id));
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
