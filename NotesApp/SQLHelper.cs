using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NotesApp
{
    internal class SQLHelper
    {
        public static void executeSavingQuery(string query)
        {
            string connectionString = "Server=DESKTOP-VN3SNKN;Database=NotesAppDB;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand();

            try
            {
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 2 * 3600;

                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }
        }

        public static List<Note> executeReadingQuery(string query)
        {
            List<Note> notes = new List<Note>();
            string connectionString = "Server=DESKTOP-VN3SNKN;Database=NotesAppDB;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = query;

                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        notes.Add(new Note() { Id = (int)dr["Id"], CreationDateTime = (DateTime)dr["CreationDateTime"], EncodedText = dr["EncodedText"].ToString() });
                    }
                    dr.Close();
                }
                sqlConnection.Close();
            }

            return notes;
        }
    }
}
