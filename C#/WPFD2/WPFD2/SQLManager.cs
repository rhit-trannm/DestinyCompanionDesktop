using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using AdonisUI.Controls;

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
                    cmd.Parameters.Add("@DestinyID", SqlDbType.Int).Value = (int)id;
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
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return null;


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
                    cmd.Parameters.Add("@DestinyMembershipID", SqlDbType.BigInt).Value = membershipID;
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
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return 1;
        }
        public string createUser(long? membershipID, long DestinyMembershipID, string name)
        {
            if (membershipID == null || name == null || DestinyMembershipID == null)
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
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);

                    }
                    finally
                    {
                        conn.Close();

                    }
                }
            }
            return null;
        }
        public string GetItemCategoryDefinition(long hash)
        {
            string name = "";
            // DataTable tblEmployees = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetItemCategoryDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@Hash", SqlDbType.BigInt).Value = hash;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50);
                    cmd.Parameters["@Name"].Direction = ParameterDirection.Output;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        name = Convert.ToString(cmd.Parameters["@Name"].Value).Trim();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return name;
        }

        public void AddDestinyItemDefinition(long ItemHash, long BucketHash, string name, string description, string tierTypeName,
            long? ItemCategoryClass, string IconURL, long? ItemCategoryArmor, long? ItemCategoryWeapon)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.addItemDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@BucketHash", SqlDbType.BigInt).Value = BucketHash;
                    cmd.Parameters.Add("@Name", SqlDbType.NChar, 40).Value = name;
                    cmd.Parameters.Add("@Description", SqlDbType.Text).Value = description;
                    cmd.Parameters.Add("@tierTypeName", SqlDbType.NChar, 50).Value = tierTypeName;
                    cmd.Parameters.Add("@ItemCategoryClass", SqlDbType.BigInt).Value = ItemCategoryClass;
                    cmd.Parameters.Add("@IconURL", SqlDbType.VarChar, 500).Value = IconURL;
                    cmd.Parameters.Add("@ItemCategoryWeapon", SqlDbType.BigInt).Value = ItemCategoryWeapon;
                    cmd.Parameters.Add("@ItemCategoryArmor", SqlDbType.BigInt).Value = ItemCategoryArmor;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        class FilterContainer
        {
            public List<long> arr = new List<long>() { 1498876634 , 2465295065, 953998645, 5, 7, 8, 6 ,14, 11,
            10, 3954685534, 3448274439, 3551918588, 14239492, 20886954, 1585787867};
            public long Kinetic = 1498876634; // 0
            public long Energy = 2465295065; // 1
            public long Power = 953998645; // 2
            public long Auto = 5; // 3
            public long Pulse = 7;// 4
            public long Scout = 8; // 5
            public long HandCannon = 6; // 6
            public long SideArms = 14; // 7
            public long Shotguns = 11; // 8
            public long Sniper = 10; // 9
            public long SMG = 3954685534; // 10
            public long Helmet = 3448274439; // 11
            public long Gauntlet = 3551918588; // 12
            public long Chest = 14239492; // 13
            public long Leg = 20886954; // 14
            public long Class = 1585787867; // 15
            public string Rare = "Rare"; // 16
            public string Legendary = "Legendary"; // 17
            public string Exotic = "Exotic"; // 18
        }
        /*
         *
         *
         *
         *
         *
         */
        public DataTable GetVaultFiltered(List<bool> filters, string Orderby, long MembershipID)
        {
            int count = 0;
            FilterContainer fils = new FilterContainer();
            DataTable tblEmployees = new DataTable();
            string Query = "SELECT DestinyBucketDefinition.Name as SlotName, DestinyItemDefinition.Name as ItemName, Vault.ItemHash as ItemHash, Vault.ItemInstanceID as ItemInstanceID, DestinyItemDefinition.BucketHash as BucketHash, DestinyItemDefinition.tierTypeName as Rarity " + 
                "FROM Vault " +
                "JOIN[DestinyItemDefinition] ON [DestinyItemDefinition].ItemHash = Vault.ItemHash " +
                "JOIN DestinyBucketDefinition ON DestinyBucketDefinition.BucketHash = DestinyItemDefinition.BucketHash " +
                "JOIN Users ON Users.[DestinyMembershipID] = " + MembershipID.ToString() +
                " WHERE Vault.VaultID = Users.VaultID AND (";
            string Buckets = "DestinyBucketDefinition.BucketHash IN (";
            bool temp = false;
            for(int i = 0; i < 3; i++)
            {
                if (filters[i])
                {
                    if(temp == false)
                    {
                        temp = true;
                    }
                    Buckets = Buckets + fils.arr[i].ToString() + ",";

                }
            }
            if(temp == true)
            {
                count++;
                Buckets = Buckets.Substring(0, Buckets.Length - 1);
                Buckets = Buckets + ")";
                Query = Query + Buckets;
            }
            temp = false;

            string Category = "[DestinyItemDefinition].ItemCategoryWeapon IN (";
            for(int i = 3; i < 11; i++)
            {
                if (filters[i])
                {
                    if (temp == false)
                    {
                        temp = true;
                    }
                    Category = Category + fils.arr[i].ToString() + ",";
                }
            }
            
            
            if (temp == true)
            {
                count++;
                Category = Category.Substring(0, Category.Length - 1);
                Category = Category + ")";
                if(count > 0)
                {
                    Query = Query + " AND " + Category;
                }
                else
                {
                    Query = Query + Category;
                }
                
            }
            temp = false;


            string armorType = "[DestinyItemDefinition].ItemCategoryArmor IN (";
            for (int i = 11; i < 16; i++)
            {
                if (filters[i])
                {
                    if (temp == false)
                    {
                        temp = true;
                    }
                    armorType = armorType + fils.arr[i].ToString() + ",";
                }
                
            }
            
            if (temp == true)
            {
                count++;
                armorType = armorType.Substring(0, armorType.Length - 1);
                armorType = armorType + ")";
                if (count > 0)
                {
                    Query = Query + " AND " + armorType;
                }
                else
                {
                    Query = Query + armorType;
                }
            }
            temp = false;

            string rarity = "[DestinyItemDefinition].tierTypeName IN (";
            for (int i = 16; i < 19; i++)
            {
                if (filters[i])
                {
                    if (temp == false)
                    {
                        temp = true;
                    }
                    if(i == 16)
                    {
                        rarity = rarity + "'" + fils.Rare + "'" + ",";
                    }
                    else if( i == 17)
                    {
                        rarity = rarity + "'" + fils.Legendary + "'" + ",";
                    }
                    else if( i == 18)
                    {
                        rarity = rarity + "'" +fils.Exotic + "'" + ",";
                    }
                    
                }
                
            }
            if (temp == true)
            {
                rarity = rarity.Substring(0, rarity.Length - 1);
                rarity = rarity + ")";
                if (count > 0)
                {
                    Query = Query + " AND " + rarity;
                }
                else
                {
                    Query = Query + rarity;
                }
            }
            Query = Query + ")";
            AdonisUI.Controls.MessageBox.Show(Query.ToString() + "\n" + count.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            //23 bools

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {

                    cmd.CommandType = CommandType.Text;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter _dap = new SqlDataAdapter(cmd);

                        _dap.Fill(tblEmployees);
                        return tblEmployees;

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return tblEmployees;

        }

        public DataTable GetVault(long DestinyMembershipID)
        {
            string name = "";
            DataTable tblEmployees = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetVault", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@DestinyMembershipID", SqlDbType.BigInt).Value = DestinyMembershipID;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter _dap = new SqlDataAdapter(cmd);

                        _dap.Fill(tblEmployees);
                        return tblEmployees;

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return null;

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
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
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
                    for (int i = 0; i < CharacterID.Count; i++)
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
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
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
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public DataTable getItemDefName(long ItemHash)
        {
            string name = "";
            DataTable tblEmployees = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.getItemDefinitionName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter _dap = new SqlDataAdapter(cmd);

                        _dap.Fill(tblEmployees);

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return tblEmployees;
                }
            }
        }
        public void UpdateStatManifest(long StatHash, string StatName)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.AddStatDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    //cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@StatHash", SqlDbType.BigInt).Value = StatHash;
                    cmd.Parameters.Add("@StatName", SqlDbType.NChar, 40).Value = StatName;
                    //cmd.Parameters.Add("@Description", SqlDbType.Text).Value = description;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

        }

        public DataTable getInventory(long CharID)
        {
            string name = "";
            DataTable tblEmployees = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetInventory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@CharID", SqlDbType.BigInt).Value = CharID;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter _dap = new SqlDataAdapter(cmd);

                        _dap.Fill(tblEmployees);


                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return tblEmployees;
        }
        public string GetStatDefinition(long StatHash)
        {
            string name = "";
            // DataTable tblEmployees = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetStatDefinition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@StatHash", SqlDbType.BigInt).Value = StatHash;
                    cmd.Parameters.Add("@StatName", SqlDbType.VarChar, 100);
                    cmd.Parameters["@StatName"].Direction = ParameterDirection.Output;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        name = Convert.ToString(cmd.Parameters["@StatName"].Value).Trim();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return name;
        }
        public DataTable GetEquipped(long CharID)
        {
            string name = "";
            DataTable tblEmployees = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetEquipped", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@CharID", SqlDbType.BigInt).Value = CharID;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter _dap = new SqlDataAdapter(cmd);

                        _dap.Fill(tblEmployees);


                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return tblEmployees;
        }
        public void EquipItem(long DestinyID, long ItemHash, long BucketHash, long ItemInstanceID, long CharacterID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.EquipItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@DestinyID", SqlDbType.BigInt).Value = DestinyID;
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@BucketHash", SqlDbType.BigInt).Value = BucketHash;
                    cmd.Parameters.Add("@ItemInstanceID", SqlDbType.BigInt).Value = ItemInstanceID;
                    cmd.Parameters.Add("@CharacterID", SqlDbType.BigInt).Value = CharacterID;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

        }

        public void TransferVaultToInventory(long DestinyID, long CharID, long ItemInstanceID, long ItemHash, long BucketHash)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.TransferVaultToInventory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@DestinyMembershipID", SqlDbType.BigInt).Value = DestinyID;
                    cmd.Parameters.Add("@CharID", SqlDbType.BigInt).Value = CharID;
                    cmd.Parameters.Add("@ItemInstanceID", SqlDbType.BigInt).Value = ItemInstanceID;
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@BucketHash", SqlDbType.BigInt).Value = BucketHash;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

        }

        public void TransferInventoryToVault(long DestinyID, long CharID, long ItemInstanceID, long ItemHash, long BucketHash)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.TransferInventoryToVault", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //create params
                    cmd.Parameters.Add("@DestinyMembershipID", SqlDbType.BigInt).Value = DestinyID;
                    cmd.Parameters.Add("@CharID", SqlDbType.BigInt).Value = CharID;
                    cmd.Parameters.Add("@ItemInstanceID", SqlDbType.BigInt).Value = ItemInstanceID;
                    cmd.Parameters.Add("@ItemHash", SqlDbType.BigInt).Value = ItemHash;
                    cmd.Parameters.Add("@BucketHash", SqlDbType.BigInt).Value = BucketHash;
                    //set param as output
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        AdonisUI.Controls.MessageBox.Show(e.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
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
