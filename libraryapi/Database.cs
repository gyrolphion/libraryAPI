using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace libraryapi
{
    public static class Database
    {
        private static readonly string connectionString = "Data Source=GOODDAY;Initial Catalog=librarydb;Integrated Security=True;Encrypt=False";

        /// <summary>
        /// SQL bağlantısını açar ve geri döner. Kullanımdan sonra kapatmayı unutma.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// SQL sorgusu çalıştırır ve etkilenen satır sayısını döner.
        /// </summary>
        public static int ExecuteCommand(string query, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// SQL sorgusu çalıştırır ve tek bir değer döner (örneğin COUNT, MAX, IDENTITY vb.)
        /// </summary>
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// SQL sorgusunun sonucunu DataTable olarak döner.
        /// </summary>
        public static DataTable GetDataTable(string query, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddRange(parameters);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public static List<T> ToList<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T(); // Yeni bir nesne oluştur

                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        // Property'e uygun türde değeri ata
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                    }
                }

                list.Add(obj); // Listeye ekle
            }

            return list;

        }

        public static T toData<T>(DataRow row) where T : new()
        {

            T data = new T();

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (row[prop.Name] != DBNull.Value)
                {
                    // Property'e uygun türde değeri ata
                    prop.SetValue(data, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                }
            }

            return data;

        }

    }


}
