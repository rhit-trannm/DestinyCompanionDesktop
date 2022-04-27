using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WPFD2
{
    class SQLManager
    {
        public SQLManager()
        {
            string cs = @"server=localhost;userid=dbuser;password=s$cret;database=testdb";
            using var con = new MySqlConnection(cs);
            con.Open();
            Trace.WriteLine($"MySQL version : {con.ServerVersion}");
        }



    }
}
