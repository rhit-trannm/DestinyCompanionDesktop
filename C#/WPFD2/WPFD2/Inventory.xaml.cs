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

namespace WPFD2
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : AdonisWindow
    {
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
        Manager _Manager;
        public Inventory(Manager manager)
        {
            InitializeComponent();

            List<EquippedItem> persons = new List<EquippedItem>();
/*            persons.Add(new EquippedItem() { SlotName = "Kinetic", ItemName = "Ace of Spades" });*/
            EquippedItemsChart.ItemsSource = persons;
            List<DestinyClass> classList = new List<DestinyClass>();
            foreach (DestinyCharacterComponent entry in manager.getAPIManager().getCharacterList())
            {
                classList.Add(entry.ClassType);
            }
            CharacterSelection.ItemsSource = classList;
            List<InventoryItem> temp = new List<InventoryItem>();
/*            temp.Add(new InventoryItem() { SlotName = "Kinetic", ItemName = "Ace of Spades" });
            temp.Add(new InventoryItem() { SlotName = "Kinetic", ItemName = "Ace of 2" });
            temp.Add(new InventoryItem() { SlotName = "Kinetic", ItemName = "Ace of 3" });*/
            InventoryKinetic.ItemsSource = temp;
            this._Manager = manager;
            UpdateVault();

            /*blah.Text = manager.getAPIManager().profile();
            Items.Text = manager.getAPIManager().getInventory();
            string user = manager.getAPIManager().getUserName();
            long id = manager.getAPIManager().getD2MemID();
            username.Text = "Username: " + user;
            userid.Text = "UserID: " + id.ToString();
            manager.getSQLManager().initiateUser(id);*/
            //textBox.Text = sql.starter();

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

        }

        public void CharacterSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DestinyItemCategoryDefinition definition = new DestinyItemCategoryDefinition();
            DestinyInventoryItemDefinition itemDefinition = new DestinyInventoryItemDefinition();
            int index = CharacterSelection.SelectedIndex;
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            //All destiny defintion

            List<DestinyInventoryItemDefinition> itemsList = new List<DestinyInventoryItemDefinition>();
            this.EquippedList = new List<InventoryItem>();
            updateEquipped();
            EquippedItemsChart.ItemsSource = EquippedList;
            updateInventory();

        }
        private void updateInventory()
        {
            uint Kinetic = 1498876634;
            uint Energy = 2465295065;
            uint Power = 953998645;
            uint Helmet = 3448274439;
            uint Gauntlet = 3551918588;
            uint chest = 14239492;
            uint Leg = 20886954;
            uint classArmor = 1585787867;

            this.Kinetic = new List<InventoryItem>();
            this.Energy = new List<InventoryItem>();
            this.Power = new List<InventoryItem>();
            this.Helmet = new List<InventoryItem>();
            this.Gauntlet = new List<InventoryItem>();
            this.Chest = new List<InventoryItem>();
            this.Leg = new List<InventoryItem>();
            this.Class = new List<InventoryItem>();
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            //REMEMBER TO CHANGE INDEX LATER!!!
            DataTable data = this._Manager.getSQLManager().getInventory(list.ElementAt(0).CharacterId);
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
                    if(ItemName == null)
                    {
                    continue;
                    }
                    temp.SlotName = SlotName;
                    temp.ItemName = ItemName;
                    temp.ItemHash = ItemHash;
                    temp.ItemInstanceId = ItemInstanceId;
                    temp.BucketHash = BucketHash;

                    if (temp.BucketHash == Kinetic)
                    {
                        
                        this.Kinetic.Add(temp);
                    }
                    else if (temp.BucketHash == Energy)
                    {
                        
                        this.Energy.Add(temp);
                    }
                    else if (temp.BucketHash == Power)
                    {
                        
                        this.Power.Add(temp);
                    }
                    else if (temp.BucketHash == Helmet)
                    {
                        
                        this.Helmet.Add(temp);
                    }
                    else if (temp.BucketHash == Gauntlet)
                    {
                        
                        this.Gauntlet.Add(temp);
                    }
                    else if (temp.BucketHash == chest)
                    {
                        
                        this.Chest.Add(temp);
                    }
                    else if (temp.BucketHash == Leg)
                    {
                        
                        this.Leg.Add(temp);
                    }
                    else if (temp.BucketHash == classArmor)
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
        private void updateEquipped()
        {
            uint Kinetic = 1498876634;
            uint Energy = 2465295065;
            uint Power = 953998645;
            uint Helmet = 3448274439;
            uint Gauntlet = 3551918588;
            uint chest = 14239492;
            uint Leg = 20886954;
            uint classArmor = 1585787867;

            this.Kinetic = new List<InventoryItem>();
            this.Energy = new List<InventoryItem>();
            this.Power = new List<InventoryItem>();
            this.Helmet = new List<InventoryItem>();
            this.Gauntlet = new List<InventoryItem>();
            this.Chest = new List<InventoryItem>();
            this.Leg = new List<InventoryItem>();
            this.Class = new List<InventoryItem>();
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            //REMEMBER TO CHANGE INDEX LATER!!!
            this.EquippedList = new List<InventoryItem>();
            DataTable data = this._Manager.getSQLManager().GetEquipped(list.ElementAt(0).CharacterId);

            foreach (DataRow row in data.Rows)
            {
                InventoryItem temp = new InventoryItem();
                string SlotName = row["SlotName"].ToString();
                string ItemName = row["ItemName"].ToString();
                long ItemHash = long.Parse(row["ItemHash"].ToString());
                long ItemInstanceId = long.Parse(row["ItemInstanceId"].ToString());
                long BucketHash = long.Parse(row["BucketHash"].ToString());
                if (ItemName == null)
                {
                    continue;
                }
                temp.SlotName = SlotName;
                temp.ItemName = ItemName;
                temp.ItemHash = ItemHash;
                temp.ItemInstanceId = ItemInstanceId;
                temp.BucketHash = BucketHash;
                this.EquippedList.Add(temp);
            }



        }
        private void UpdateVault()
        {
            foreach (DestinyItemComponent item in this._Manager.getAPIManager().GetVault())
            {
                InventoryItem temp = new InventoryItem();
                temp.ItemName = item.ItemHash.ToString();
                temp.ItemName = this._Manager.getSQLManager().getOnlyEquippableItems(item.ItemHash);
                if(!(temp.ItemName.ToString() == ""))
                {
                    VaultList.Add(temp);
                }
                
            }
            VaultDisplay.ItemsSource = VaultList;

        }

        private void updateManifests_Click(object sender, RoutedEventArgs e)
        {

            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Outputs\DestinyItemCategoryDefinition.json");
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
/*                        this._Manager.getAPIManager().updateManifest(
                                    long.Parse(item.Key),
                                    long.Parse(items.SelectToken($"{item.Key}.inventory.bucketTypeHash").ToString()),
                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString(),
                                    items.SelectToken($"{item.Key}.displayProperties.description").ToString(),
                                    items.SelectToken($"{item.Key}.inventory.tierTypeName").ToString(),
                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[0]").ToString()));*/

                        this._Manager.getSQLManager().AddCategoryDefinition(
                                    long.Parse(item.Key),
                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString());





                            
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
        private void EquipHandler(InventoryItem inventoryitem, int CharIndex)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            long instanceID = (long)inventoryitem.ItemInstanceId;
            long characterID = list[0].CharacterId;
            BungieMembershipType member = BungieMembershipType.TigerSteam;

            this._Manager.getAPIManager().EquipItem(instanceID, characterID, member);
            System.Threading.Thread.Sleep(10000);
            this._Manager.getAPIManager().OnLoginDriver();
        }

        private void EquipKinetic_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryKinetic.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void EnergyEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryPower.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void EquipHelmet_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryHelmet.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void GauntletEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryGauntlet.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void ChestEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryChest.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void LegEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryLeg.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void PowerEquip_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryPower.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }

        private void ClassEquip_Click(object sender, RoutedEventArgs e)
        {
                        List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryClass.SelectedItem;
            EquipHandler(inventoryitem, 0);
        }
    }
}
