using BungieSharper.Entities;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.Entities.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public partial class Inventory : Window
    {
        Manager manager;
        List<InventoryItem> Kinetic = new List<InventoryItem>();
        List<InventoryItem> Energy = new List<InventoryItem>();
        List<InventoryItem> Power = new List<InventoryItem>();
        List<InventoryItem> Helmet = new List<InventoryItem>();
        List<InventoryItem> Gauntlet = new List<InventoryItem>();
        List<InventoryItem> Chest = new List<InventoryItem>();
        List<InventoryItem> Leg = new List<InventoryItem>();
        List<InventoryItem> Class = new List<InventoryItem>();
        List<EquippedItem> EquippedList = new List<EquippedItem>();
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

        public class EquippedItem
        {
            public string SlotName { set; get; }
            public String ItemName { set; get; }
            public uint ItemHash { set; get; }
            public long? ItemInstanceId
            {
                get;
                set;
            }

        }
        public class InventoryItem
        {
            public string SlotName { set; get; }
            public String ItemName { set; get; }
            public uint ItemHash { set; get; }
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
            foreach (DestinyItemComponent item in this._Manager.getAPIManager().GetEquipped(list.ElementAt(0).CharacterId))
            {
                uint Kinetic = 1498876634;
                uint Energy = 2465295065;
                uint Power = 953998645;
                uint Helmet = 3448274439;
                uint Gauntlet = 3551918588;
                uint chest = 14239492;
                uint Leg = 20886954;
                uint classArmor = 1585787867;

                EquippedItem temp = new EquippedItem();

                //temp.ItemName = item.ItemHash.ToString();
                temp.ItemName = this._Manager.getSQLManager().getItemDefName(item.ItemHash);

                //temp.ItemName = InventoryItems[item.ItemHash].DisplayProperties.Name;
                if (item.BucketHash == Kinetic)
                {
                    temp.SlotName = "Kinetic";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == Energy)
                {
                    temp.SlotName = "Energy";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == Power)
                {
                    temp.SlotName = "Power";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == Helmet)
                {
                    temp.SlotName = "Helmet";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == Gauntlet)
                {
                    temp.SlotName = "Gauntlet";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == chest)
                {
                    temp.SlotName = "Chest";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == Leg)
                {
                    temp.SlotName = "Leg";
                    EquippedList.Add(temp);
                }
                else if (item.BucketHash == classArmor)
                {
                    temp.SlotName = "Class";
                    EquippedList.Add(temp);
                }

                temp.ItemHash = item.ItemHash;
                temp.ItemInstanceId = item.ItemInstanceId;

            }
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
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            //REMEMBER TO CHANGE INDEX LATER!!!
            foreach (DestinyItemComponent item in this._Manager.getAPIManager().GetInventory(list.ElementAt(0).CharacterId))
            {

                InventoryItem temp = new InventoryItem();

                temp.ItemName = item.ItemHash.ToString();
                temp.ItemName = this._Manager.getSQLManager().getItemDefName(item.ItemHash);
                //temp.ItemName = InventoryItems[item.ItemHash].DisplayProperties.Name;
                if (item.BucketHash == Kinetic)
                {
                    temp.SlotName = "Kinetic";
                    this.Kinetic.Add(temp);
                }
                else if (item.BucketHash == Energy)
                {
                    temp.SlotName = "Energy";
                    this.Energy.Add(temp);
                }
                else if (item.BucketHash == Power)
                {
                    temp.SlotName = "Power";
                    this.Power.Add(temp);
                }
                else if (item.BucketHash == Helmet)
                {
                    temp.SlotName = "Helmet";
                    this.Helmet.Add(temp);
                }
                else if (item.BucketHash == Gauntlet)
                {
                    temp.SlotName = "Gauntlet";
                    this.Gauntlet.Add(temp);
                }
                else if (item.BucketHash == chest)
                {
                    temp.SlotName = "Chest";
                    this.Chest.Add(temp);
                }
                else if (item.BucketHash == Leg)
                {
                    temp.SlotName = "Leg";
                    this.Leg.Add(temp);
                }
                else if (item.BucketHash == classArmor)
                {
                    temp.SlotName = "Class";
                    this.Class.Add(temp);
                }

                temp.ItemHash = item.ItemHash;
                temp.ItemInstanceId = item.ItemInstanceId;
            }
            InventoryKinetic.ItemsSource = this.Kinetic;
            InventoryEnergy.ItemsSource = this.Energy;
            InventoryPower.ItemsSource = this.Power;
            InventoryHelmet.ItemsSource = this.Helmet;
            InventoryGauntlet.ItemsSource = this.Gauntlet;
            InventoryChest.ItemsSource = this.Chest;
            InventoryLeg.ItemsSource = this.Leg;
            InventoryClass.ItemsSource = this.Class;

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
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Outputs\DestinyInventoryBucketDefinition.json");
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

                        this._Manager.getAPIManager().updateBucketManifest(long.Parse(item.Key), 
                            items.SelectToken($"{item.Key}.displayProperties.name").ToString());
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
