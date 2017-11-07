namespace POH5Data
{
    public abstract class DataAccess
    {
        public string ConnectionString { get; set; }

        public DataAccess(string conString) {
            this.ConnectionString = conString;
        }
    }
}
