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

        public void AddDestinyItemDefinition(long ItemHash, long BucketHash, string name, string description, string tierTypeName, long? ItemCategoryClass)
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
                    cmd.Parameters.Add("@tierTypeName", SqlDbType.NChar, 50).Value = tierTypeName;
                    cmd.Parameters.Add("@ItemCategoryClass", SqlDbType.BigInt).Value = ItemCategoryClass;
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

        public void AddCategoryDefinition(long Hash, string name)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.addCategoryDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    //cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@Hash", SqlDbType.BigInt).Value = Hash;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
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

        public void OnLogin(long? membershipID, string name, List<long> CharacterID, List<long> DestinyMembershipID, List<int> ClassType, List<long> DestinyID, List<long> CharID,
            List<long> ItemHash, List<long> ItemInstanceID, List<long> BucketHash, 
            List<long> InventoryCharID, List<long> InventoryItemHash, List<long> InventoryItemInstanceID, List<long> InventoryBucketHash,
            List<long> VaultItemHash, List<long> VaultItemInstanceID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.OnLogin", con))
                {
                    var table = new DataTable();
                    table.Columns.Add("DestinyMembershipID", typeof(long));
                    table.Columns.Add("CharacterID", typeof(long));
                    table.Columns.Add("ClassType", typeof(int));
                    for(int i = 0; i < CharacterID.Count; i++)
                    {
                        table.Rows.Add(DestinyMembershipID[i], CharacterID[i], ClassType[i]);
                    }
                    var table2 = new DataTable();
                    table2.Columns.Add("DestinyID", typeof(long));
                    table2.Columns.Add("CharID", typeof(long));
                    table2.Columns.Add("ItemHash", typeof(long));
                    table2.Columns.Add("ItemInstanceID", typeof(long));
                    table2.Columns.Add("BucketHash", typeof(long));
                    for (int i = 0; i < ItemInstanceID.Count; i++)
                    {
                        table2.Rows.Add(DestinyID[i], CharID[i], ItemHash[i], ItemInstanceID[i], BucketHash[i]);
                    }
                    var table3 = new DataTable();
                    table3.Columns.Add("DestinyID", typeof(long));
                    table3.Columns.Add("CharID", typeof(long));
                    table3.Columns.Add("ItemHash", typeof(long));
                    table3.Columns.Add("ItemInstanceID", typeof(long));
                    table3.Columns.Add("BucketHash", typeof(long));
                    for (int i = 0; i < InventoryItemInstanceID.Count; i++)
                    {
                        table3.Rows.Add(DestinyID[0], InventoryCharID[i], InventoryItemHash[i], InventoryItemInstanceID[i], InventoryBucketHash[i]);
                    }
                    var table4 = new DataTable();

                    table4.Columns.Add("VaultID", typeof(long));
                    table4.Columns.Add("ItemHash", typeof(long));
                    table4.Columns.Add("ItemInstanceID", typeof(long));

                    for (int i = 0; i < VaultItemInstanceID.Count; i++)
                    {
                        table4.Rows.Add(0, VaultItemHash[i], VaultItemInstanceID[i]);
                    }
                    var pList = new SqlParameter("@CharacterID", SqlDbType.Structured);
                    pList.TypeName = "dbo.CharacterIDList";
                    pList.Value = table;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(pList);
                    cmd.Parameters.Add("@CharacterID", SqlDbType.Structured).Value = table;
                    cmd.Parameters.Add("@CharacterEquippedList", SqlDbType.Structured).Value = table2;
                    cmd.Parameters.Add("@CharacterInventoryList", SqlDbType.Structured).Value = table3;
                    cmd.Parameters.Add("@VaultList", SqlDbType.Structured).Value = table4;
                    cmd.Parameters.Add("@MembershipID", SqlDbType.BigInt).Value = membershipID;
                    cmd.Parameters.Add("@DestinyMembershipId", SqlDbType.BigInt).Value = DestinyMembershipID[0];
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
                    //create params
                    //cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    //cmd.Parameters.Add("@Hash", SqlDbType.BigInt).Value = Hash;
                    //cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
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
