using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;


namespace WPFD2
{
    public class SQLManager
    {
        SqlConnection conn;
        string cs = @"Server=titan.csse.rose-hulman.edu; Encrypt=False; Database=CSSE333_S4G1_FinalProjectDB; UID=trannm; Password=Acixuw+03";
        public SQLManager()
        {

            string cs = @"Server=titan.csse.rose-hulman.edu; Encrypt=False; Database=CSSE333_S4G1_FinalProjectDB; UID=trannm; Password=Acixuw+03";
            try
            {
                conn = new SqlConnection(cs);
                //MySqlConnection con = new MySqlConnection(cs);
                conn.Open();
                Console.WriteLine($"MySQL version : {conn.ServerVersion}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }


        }
        public string starter()
        {
            return conn.ServerVersion;
        }
        public string initiateUser()
        {

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.getPlayer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@DestinyID", SqlDbType.Int).Value = 1;

                   cmd.Parameters.Add("@Return_Value", SqlDbType.VarChar, 50);

                    cmd.Parameters["@Return_Value"].Direction = ParameterDirection.Output;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        String displayName = Convert.ToString(cmd.Parameters["@Return_Value"].Value);

                        return displayName;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally {
                        conn.Close();
                    }
              
                    
                }
            }
        }



    }
}
