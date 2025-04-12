using AdoNetLib.Configurations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNetLib
{
    public class MainConnector
    {
        SqlConnection? connection;

        public async Task<bool> ConnectAsync()
        {
            bool result;
            try
            {
                var connection = new SqlConnection(ConnectionString.MsSqlConnection);
                await connection.OpenAsync();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public async void DisconnectAsync()
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }
}
