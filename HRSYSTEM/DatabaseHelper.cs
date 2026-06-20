using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public class DatabaseHelper
    {
        private readonly string connectionString = "Server=localhost;Database=hrsys_db;Uid=codeshark;Pwd=sovann@1029;";
        public string connection => connectionString;
        public DataTable? ExecuteQuery(string query, MySqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                try
                {
                    conn.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Read Error: " + ex.Message);
                    return null;
                }
            }

            return dt;
        }

        public bool ExecuteNonQuery(string query, MySqlParameter[]? parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Write Error: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
