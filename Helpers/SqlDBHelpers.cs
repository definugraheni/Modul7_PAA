using Npgsql;
using System.Data;

namespace Modul_7.Helpers
{
    public class SqlDBHelpers
    {
        private NpgsqlConnection connection;
        private string __constr;

        public SqlDBHelpers(string pCOnstr)
        {
            __constr = pCOnstr;
            connection = new NpgsqlConnection();
            connection.ConnectionString = __constr;
        }

        public NpgsqlCommand getNpgsqlCommand(string query)
        {
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public void closeConnection()
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public void Dispose()
        {
            if (connection != null)
            {
                closeConnection();
                connection.Dispose();
            }
        }
    }
}

