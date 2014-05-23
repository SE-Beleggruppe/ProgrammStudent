using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sybase.Data.AseClient;

namespace ClickDummyStudent
{

    class Database
    {
        public string ConnectionString = "Data Source=141.56.20.2;Port=5200;" +
                                    "UID=case04;PWD=itcyisay;" +
                                    "Database=case04;";

        AseConnection conn;
        public Database()
        {
            conn = new AseConnection(ConnectionString);
        }

        public void Connect()
        {

            try
            {
                conn.Open();
            }
            catch (AseException ex)
            {
                Console.Write(
                   ex.Message,
                   "Failed to connect");
            }
        }

        public List<string[]> ExecuteQuery(string query)
        {
            Connect();
            AseCommand command = new AseCommand(query, conn);
            using (var dataReader = command.ExecuteReader())
            {
                int col = dataReader.FieldCount;
                List<string[]> strings = new List<string[]>();

                while (dataReader.Read())
                {
                    string[] array = new string[col];
                    for (int i = 0; i < col; i++)
                    {
                        array[i] = dataReader.GetString(i).Trim();
                    }
                    strings.Add(array);
                }
                conn.Close();
                return strings;
            }

        }
    }
}
