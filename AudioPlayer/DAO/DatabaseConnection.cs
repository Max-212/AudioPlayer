using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AudioPlayer.DAO
{
    public class DatabaseConnection
    {
        public static SqlConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}
