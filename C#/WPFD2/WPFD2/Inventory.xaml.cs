using AdonisUI.Controls;
using BungieSharper.Entities;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.Entities.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AdonisUI.Controls;
using System.Collections;

namespace WPFD2
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : AdonisWindow
    {
        uint _BucketHashKinetic = 1498876634;
        uint _BucketHashEnergy = 2465295065;
        uint _BucketHashPower = 953998645;
        uint _BucketHashHelmet = 3448274439;
        uint _BucketHashGauntlet = 3551918588;
        uint _BucketHashchest = 14239492;
        uint _BucketHashLeg = 20886954;
        uint _BucketHashclassArmor = 1585787867;



        List<InventoryItem> Kinetic = new List<InventoryItem>();
        List<InventoryItem> Energy = new List<InventoryItem>();
        List<InventoryItem> Power = new List<InventoryItem>();
        List<InventoryItem> Helmet = new List<InventoryItem>();
        List<InventoryItem> Gauntlet = new List<InventoryItem>();
        List<InventoryItem> Chest = new List<InventoryItem>();
        List<InventoryItem> Leg = new List<InventoryItem>();
        List<InventoryItem> Class = new List<InventoryItem>();
        List<InventoryItem> EquippedList = new List<InventoryItem>();
        List<InventoryItem> VaultList = new List<InventoryItem>();
        List<CharacterInfo> characterInfos = new List<CharacterInfo>();
        Manager _Manager;


        public class EquippedItem
        {
            public string? SlotName { set; get; }
            public String? ItemName { set; get; }
            public long? ItemHash { set; get; }
            public long? ItemInstanceId
            {
                get;
                set;
            }

        }
        public class InventoryItem
        {
            public string? SlotName { set; get; }
            public String? ItemName { set; get; }
            public long? ItemHash { set; get; }
            public long? BucketHash { set; get; }
            public long? ItemInstanceId
            {
                get;
                set;
            }
            public string? Rarity { set; get; }
            public string? Color { set; get; }

        }
        public class CharacterInfo {
            public long? CharacterID { set; get; }
            public DestinyClass? DestinyClass { set; get; }

        }
        public Inventory(Manager manager)
        {
            InitializeComponent();
            //AdonisUI.Controls.MessageBox.Show("Hello world!", "Info", AdonisUI.Controls.MessageBoxButton.OK);
            this._Manager = manager;
            this._Manager.getAPIManager().OnLoginDriver();
            UpdateCharacterDropDown();
            int index = CharacterSelection.SelectedIndex;
            long charID = (long)characterInfos[index].CharacterID;
            updateEquipped(charID);
            updateInventory(charID);
            UpdateVault();

        }
        private Timer timer;
        public void InitTimer()
        {
            timer = new Timer(_ => timer1_Tick(), null, 0, 3000 * 10); //every 30 seconds
        }

        private void timer1_Tick()
        {
            //Refresh;
        }
        public void UpdateCharacterDropDown()
        {
            List<DestinyClass> classList = new List<DestinyClass>();
            characterInfos = new List<CharacterInfo>();
            foreach (DestinyCharacterComponent entry in this._Manager.getAPIManager().getCharacterList())
            {
                CharacterInfo info = new CharacterInfo();
                info.CharacterID = entry.CharacterId;
                info.DestinyClass = entry.ClassType;
                classList.Add(entry.ClassType);
                characterInfos.Add(info);
            }
            CharacterSelection.ItemsSource = classList;
        }


        public void CharacterSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DestinyItemCategoryDefinition definition = new DestinyItemCategoryDefinition();
            DestinyInventoryItemDefinition itemDefinition = new DestinyInventoryItemDefinition();
            int index = CharacterSelection.SelectedIndex;
            long charID = (long) characterInfos[index].CharacterID;
            //All destiny defintion
            List<DestinyInventoryItemDefinition> itemsList = new List<DestinyInventoryItemDefinition>();
            this.EquippedList = new List<InventoryItem>();
            updateEquipped(charID);
            updateInventory(charID);
            UpdateVault();

        }
        private void updateInventory(long CharacterID)
        {
            this.Kinetic = new List<InventoryItem>();
            this.Energy = new List<InventoryItem>();
            this.Power = new List<InventoryItem>();
            this.Helmet = new List<InventoryItem>();
            this.Gauntlet = new List<InventoryItem>();
            this.Chest = new List<InventoryItem>();
            this.Leg = new List<InventoryItem>();
            this.Class = new List<InventoryItem>();
            //REMEMBER TO CHANGE INDEX LATER!!!
            DataTable data = this._Manager.getSQLManager().getInventory(CharacterID);
            //foreach (DestinyItemComponent item in this._Manager.getAPIManager().GetInventory(list.ElementAt(0).CharacterId))
            //{



            foreach (DataRow row in data.Rows)
            {
                InventoryItem temp = new InventoryItem();
                string SlotName = row["SlotName"].ToString();
                string ItemName = row["ItemName"].ToString();
                long ItemHash = long.Parse(row["ItemHash"].ToString());
                long ItemInstanceId = long.Parse(row["ItemInstanceId"].ToString());
                long BucketHash = long.Parse(row["BucketHash"].ToString());
                string rarity = row["Rarity"].ToString();
                if (ItemName == null)
                {
                    continue;
                }
                temp.SlotName = SlotName;
                temp.ItemName = ItemName;
                temp.ItemHash = ItemHash;
                temp.ItemInstanceId = ItemInstanceId;
                temp.BucketHash = BucketHash;
                temp.Rarity = rarity;
                if (temp.Rarity.Trim().Equals("Common"))
                {
                    temp.Color = "White";
                }
                else if (temp.Rarity.Trim().Equals("Uncommon"))
                {
                    temp.Color = "Blue";
                }
                else if (temp.Rarity.Trim().Equals("Rare"))
                {
                    temp.Color = "LightBlue";
                }
                else if (temp.Rarity.Trim().Equals("Legendary"))
                {
                    temp.Color = "Purple";
                }
                else if (temp.Rarity.Trim().Equals("Exotic"))
                {
                    temp.Color = "Yellow";
                }
                else
                {
                    temp.Color = temp.Rarity.Length.ToString();
                }
                
                if (temp.BucketHash == _BucketHashKinetic)
                {
                    this.Kinetic.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashEnergy)
                {

                    this.Energy.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashPower)
                {

                    this.Power.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashHelmet)
                {

                    this.Helmet.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashGauntlet)
                {

                    this.Gauntlet.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashchest)
                {

                    this.Chest.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashLeg)
                {

                    this.Leg.Add(temp);
                }
                else if (temp.BucketHash == _BucketHashclassArmor)
                {

                    this.Class.Add(temp);
                }
            }
            //}
            InventoryKinetic.ItemsSource = this.Kinetic;
            InventoryEnergy.ItemsSource = this.Energy;
            InventoryPower.ItemsSource = this.Power;
            InventoryHelmet.ItemsSource = this.Helmet;
            InventoryGauntlet.ItemsSource = this.Gauntlet;
            InventoryChest.ItemsSource = this.Chest;
            InventoryLeg.ItemsSource = this.Leg;
            InventoryClass.ItemsSource = this.Class;


        }
        private void updateEquipped(long CharacterID)
        {

            this.Kinetic = new List<InventoryItem>();
            this.Energy = new List<InventoryItem>();
            this.Power = new List<InventoryItem>();
            this.Helmet = new List<InventoryItem>();
            this.Gauntlet = new List<InventoryItem>();
            this.Chest = new List<InventoryItem>();
            this.Leg = new List<InventoryItem>();
            this.Class = new List<InventoryItem>();
            //REMEMBER TO CHANGE INDEX LATER!!!
            this.EquippedList = new List<InventoryItem>();
            ArrayList arlist = new ArrayList();
            DataTable data = this._Manager.getSQLManager().GetEquipped(CharacterID);
            for(int i = 0; i < 8; i++)
            {
                arlist.Add(null);
            }
            foreach (DataRow row in data.Rows)
            {
                InventoryItem temp = new InventoryItem();
                string SlotName = row["SlotName"].ToString();
                string ItemName = row["ItemName"].ToString();
                long ItemHash = long.Parse(row["ItemHash"].ToString());
                long ItemInstanceId = long.Parse(row["ItemInstanceId"].ToString());
                long BucketHash = long.Parse(row["BucketHash"].ToString());
                string rarity = row["Rarity"].ToString();
                if (ItemName == null)
                {
                    continue;
                }

                temp.SlotName = SlotName;
                temp.ItemName = ItemName;
                temp.ItemHash = ItemHash;
                temp.ItemInstanceId = ItemInstanceId;
                temp.BucketHash = BucketHash;
                temp.Rarity = rarity;
                if (temp.Rarity.Trim().Equals("Common"))
                {
                    temp.Color = "White";
                }
                else if (temp.Rarity.Trim().Equals("Uncommon"))
                {
                    temp.Color = "Blue";
                }
                else if (temp.Rarity.Trim().Equals("Rare"))
                {
                    temp.Color = "LightBlue";
                }
                else if (temp.Rarity.Trim().Equals("Legendary"))
                {
                    temp.Color = "Purple";
                }
                else if (temp.Rarity.Trim().Equals("Exotic"))
                {
                    temp.Color = "Yellow";
                }
                else
                {
                    temp.Color = temp.Rarity.Length.ToString();
                }
                if (BucketHash == _BucketHashKinetic)
                {
                    this.EquippedList.Insert(0, temp);
                    arlist[0] = temp;
                }
                else if (BucketHash == _BucketHashEnergy)
                {
                    this.EquippedList.Insert(1, temp);
                    arlist[1] = temp;
                }
                else if (BucketHash == _BucketHashPower)
                {
                    this.EquippedList.Insert(2, temp);
                    arlist[2] = temp;
                }
                else if (BucketHash == _BucketHashHelmet)
                {
                    this.EquippedList.Insert(2, temp);
                    arlist[3] = temp;
                }
                else if (BucketHash == _BucketHashGauntlet)
                {
                    this.EquippedList.Insert(2, temp);
                    arlist[4] = temp;
                }
                else if (BucketHash == _BucketHashchest)
                {
                    this.EquippedList.Insert(2, temp);
                    arlist[5] = temp;
                }
                else if (BucketHash == _BucketHashLeg)
                {
                    this.EquippedList.Insert(2, temp);
                    arlist[6] = temp;
                }
                else if (BucketHash == _BucketHashclassArmor)
                {
                    this.EquippedList.Insert(2, temp);
                    arlist[7] = temp;
                }
                else
                {
                    this.EquippedList.Add(temp);
                    arlist.Add(temp);
                }
                
            }
            EquippedItemsChart.ItemsSource = arlist;




        }

        private void UpdateVault()
        {
            VaultList = new List<InventoryItem>();
            long memshipdId = this._Manager.getAPIManager().GetDestinyProfile().MembershipId;
            DataTable data = this._Manager.getSQLManager().GetVault(memshipdId);
            foreach (DataRow row in data.Rows)
            {
                InventoryItem temp = new InventoryItem();
                string SlotName = row["SlotName"].ToString();
                string ItemName = row["ItemName"].ToString();
                long ItemHash = long.Parse(row["ItemHash"].ToString());
                long ItemInstanceId = long.Parse(row["ItemInstanceId"].ToString());
                long BucketHash = long.Parse(row["BucketHash"].ToString());
                string rarity = row["Rarity"].ToString();

                temp.SlotName = SlotName;
                temp.ItemName = ItemName;
                temp.ItemHash = ItemHash;
                temp.ItemInstanceId = ItemInstanceId;
                temp.BucketHash = BucketHash;
                temp.Rarity = rarity;
                if (temp.Rarity.Trim().Equals("Common"))
                {
                    temp.Color = "White";
                }
                else if (temp.Rarity.Trim().Equals("Uncommon"))
                {
                    temp.Color = "Blue";
                }
                else if (temp.Rarity.Trim().Equals("Rare"))
                {
                    temp.Color = "LightBlue";
                }
                else if (temp.Rarity.Trim().Equals("Legendary"))
                {
                    temp.Color = "Purple";
                }
                else if (temp.Rarity.Trim().Equals("Exotic"))
                {
                    temp.Color = "Yellow";
                }
                else
                {
                    temp.Color = temp.Rarity.Length.ToString();
                }

                if (!(temp.ItemName.ToString() == ""))
                {
                    VaultList.Add(temp);
                }
                else
                {
                    continue;
                }

            }
            VaultDisplay.ItemsSource = VaultList;

        }


        private void EquipHandler(InventoryItem inventoryitem)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            long instanceID = (long)inventoryitem.ItemInstanceId;
            long characterID = list[0].CharacterId;
            BungieMembershipType member = BungieMembershipType.TigerSteam;

            int index = CharacterSelection.SelectedIndex;
            long charID = (long)characterInfos[index].CharacterID;

            APIManager api = this._Manager.getAPIManager();
            long DestinyID = api.GetDestinyProfile().MembershipId; 
            long ItemHash = (long) inventoryitem.ItemHash; 
            long BucketHash = (long)inventoryitem.BucketHash; 
            long ItemInstanceID = (long)inventoryitem.ItemInstanceId;
            long CharacterID = charID;
            this._Manager.getSQLManager().EquipItem(DestinyID, ItemHash, BucketHash, ItemInstanceID, CharacterID);
            this._Manager.getAPIManager().EquipItem(ItemInstanceID, CharacterID, BungieMembershipType.TigerSteam);
            updateInventory(CharacterID);
            updateEquipped(CharacterID);
            //System.Threading.Thread.Sleep(10000);
            //this._Manager.getAPIManager().OnLoginDriver();
        }

        private void EquipKinetic_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryKinetic.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void EnergyEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryPower.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void EquipHelmet_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryHelmet.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void GauntletEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryGauntlet.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void ChestEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryChest.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void LegEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryLeg.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void PowerEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryPower.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void ClassEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryClass.SelectedItem;
            EquipHandler(inventoryitem);
        }

        private void updateManifests_Click(object sender, RoutedEventArgs e)
        {

            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Outputs\DestinyInventoryItemDefinition.json");
            string path = System.IO.Path.GetFullPath(sFile);
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                JObject j = new JObject();
                JObject items = JsonConvert.DeserializeObject<JObject>(json);
                dynamic k = items;

                foreach (var item in items)
                {
                    if (items.SelectToken($"{item.Key}.displayProperties.name") != null)
                    {
                        /*                        //BucketManifest
                                                this._Manager.getAPIManager().updateBucketManifest(long.Parse(item.Key), 
                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString());*/
                        try
                        {
                            //Item Definition Manifest
                            this._Manager.getAPIManager().updateManifest(
                                        long.Parse(item.Key),
                                        long.Parse(items.SelectToken($"{item.Key}.inventory.bucketTypeHash").ToString()),
                                        items.SelectToken($"{item.Key}.displayProperties.name").ToString(),
                                        items.SelectToken($"{item.Key}.displayProperties.description").ToString(),
                                        items.SelectToken($"{item.Key}.inventory.tierTypeName").ToString(),
                                        long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[0]").ToString()),
                                        items.SelectToken($"{item.Key}.displayProperties.icon").ToString(),
                                        long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[1]").ToString()),
                                        long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[2]").ToString()));

                            /*                            this._Manager.getSQLManager().AddCategoryDefinition(
                                                                    long.Parse(item.Key),
                                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString());
                            */





                        }
                        catch (Exception ex)
                        {

                        }



                    }

                }

                //Console.Write(items);
                /*			foreach (BungieSharper.Entities.Destiny.Entities.Items.DestinyItemComponent element in items)
                            {
                                Console.Write($"{element.ItemHash} \n");
                            }*/
            }

        }
    }
}
