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
        public int checkUserExist(long? membershipID)
        {
            if (membershipID == null)
            {
                return 0;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@MembershipID", SqlDbType.BigInt).Value = membershipID;
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    //set param as output
                    try
                    {
                        //start connection and execute procedure
                        con.Open();
                        cmd.ExecuteNonQuery();
                        int sqlreturn = (int)returnParameter.Value;
                        if (sqlreturn == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
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
        public string createUser(long? membershipID, long DestinyMembershipID, string name)
        {
            if(membershipID == null || name == null || DestinyMembershipID == null)
            {
                return "False";
            }
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.addUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@MembershipID", SqlDbType.BigInt).Value = membershipID;
                    cmd.Parameters.Add("@DestinyMembershipId", SqlDbType.BigInt).Value = DestinyMembershipID;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.VarChar, 20);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    //set param as output
                    try
                    {
                        //start connection and execute procedure
                        con.Open();
                        cmd.ExecuteNonQuery();
                        int sqlreturn = (int)returnParameter.Value;
                        if (sqlreturn == 1)
                        {
                            return "False";
                        }
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

        public void AddDestinyItemDefinition(long ItemHash, long BucketHash, string name, string description, string tierTypeName)
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
                    cmd.Parameters.Add("@Description", SqlDbType.Text).Value = description;
                    cmd.Parameters.Add("@tierTypeName", SqlDbType.VarChar, 50).Value = tierTypeName;
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
        public string getOnlyEquippableItems(long ItemHash)
        {
            string temp = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetOnlyEquippableItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 40).Value = temp;
                    cmd.Parameters["@Name"].Direction = ParameterDirection.Output;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        temp = Convert.ToString(cmd.Parameters["@Name"].Value);

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return temp;
                }
            }
        }

        public void AddDestinyBucketDefinition(long BucketHash, string name)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.addBucketDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    //cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@BucketHash", SqlDbType.BigInt).Value = BucketHash;
                    cmd.Parameters.Add("@Name", SqlDbType.NChar, 40).Value = name;
                    //cmd.Parameters.Add("@Description", SqlDbType.Text).Value = description;
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
        public string getItemDefName(uint ItemHash)
        {
            string name = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.getItemDefinitionName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 40).Value = name;
                    cmd.Parameters["@Name"].Direction = ParameterDirection.Output;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        name = Convert.ToString(cmd.Parameters["@Name"].Value);

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return name;
                }
            }
        }

        



    }
}
