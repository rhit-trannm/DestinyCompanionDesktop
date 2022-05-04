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
        public string initiateUser(long id)
        {

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.getPlayer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@DestinyID", SqlDbType.Int).Value = (int) id;
                    cmd.Parameters.Add("@Return_Value", SqlDbType.VarChar, 50);
                    //set param as output
                    cmd.Parameters["@Return_Value"].Direction = ParameterDirection.Output;
                    try
                    {
                        //start connection and execute procedure
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //get value of output parameter
                        String displayName = Convert.ToString(cmd.Parameters["@Return_Value"].Value);
                        if (displayName != null)
                        {
                            return displayName;
                        }
                        else
                        {
                            return "False";
                        }
                        
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }


        }
        public string createUser(long id, string name)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.addPlayer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@destinyMembershipId", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@displayName", SqlDbType.VarChar, 50).Value = name;
                    //set param as output
                    try
                    {
                        //start connection and execute procedure
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //get value of output parameter
                        return name;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void AddDestinyItemDefinition(long ItemHash, long BucketHash, string name)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.addItemDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@BucketHash", SqlDbType.BigInt).Value = BucketHash;
                    cmd.Parameters.Add("@Name", SqlDbType.NChar,40).Value = name;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }



    }
}
