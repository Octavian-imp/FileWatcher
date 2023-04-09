using System.Data.SqlClient;

namespace FileWatcher
{
    internal class DBConnect
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=LAPTOP-QBS5U0BR\SQLEXPRESS;Initial Catalog=practice_spring;Integrated Security=True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
