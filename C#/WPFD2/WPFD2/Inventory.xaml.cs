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
using BungieSharper.Entities.Destiny.Responses;

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

        long SolarHash = 591714140;
        long ArcHash = 728351493;
        long VoidHash = 4069572561;
        long StasisHash = 1819698290;

        List<bool> filterCheckBox = new List<bool>(new bool[19]);

        bool UseFilters = false;

        List<InventoryItem> Kinetic = new List<InventoryItem>();
        List<InventoryItem> Energy = new List<InventoryItem>();
        List<InventoryItem> Power = new List<InventoryItem>();
        List<InventoryItem> Helmet = new List<InventoryItem>();
        List<InventoryItem> Gauntlet = new List<InventoryItem>();
        List<InventoryItem> Chest = new List<InventoryItem>();
        List<InventoryItem> Leg = new List<InventoryItem>();
        List<InventoryItem> Class = new List<InventoryItem>();
        List<InventoryItem> EquippedList = new List<InventoryItem>( new InventoryItem[9]);
        List<InventoryItem> VaultList = new List<InventoryItem>();
        List<CharacterInfo> characterInfos = new List<CharacterInfo>();
        Manager _Manager;
        List<ProgressBar> ProgressBarList = new List<ProgressBar>();
        List<TextBlock> TextBlockList = new List<TextBlock>();
        List<TextBlock> StatList = new List<TextBlock>();

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

            


            TextBlockList.Add(ProgresTxt0);
            TextBlockList.Add(ProgresTxt1);
            TextBlockList.Add(ProgresTxt2);
            TextBlockList.Add(ProgresTxt3);
            TextBlockList.Add(ProgresTxt4);
            TextBlockList.Add(ProgresTxt5);
            TextBlockList.Add(ProgresTxt6);
            TextBlockList.Add(ProgresTxt7);
            StatList.Add(StatValue0);
            StatList.Add(StatValue1);
            StatList.Add(StatValue2);
            StatList.Add(StatValue3);
            StatList.Add(StatValue4);
            StatList.Add(StatValue5);
            StatList.Add(StatValue6);
            StatList.Add(StatValue7);


            ProgressBarList.Add(Progress0);
            ProgressBarList.Add(Progress1);
            ProgressBarList.Add(Progress2);
            ProgressBarList.Add(Progress3);
            ProgressBarList.Add(Progress4);
            ProgressBarList.Add(Progress5);
            ProgressBarList.Add(Progress6);
            ProgressBarList.Add(Progress7);
            for (int j = 0; j < ProgressBarList.Count; j++)
            {
                ProgressBarList[j].Visibility = Visibility.Collapsed;
            }


            //AdonisUI.Controls.MessageBox.Show("Hello world!", "Info", AdonisUI.Controls.MessageBoxButton.OK);
            this._Manager = manager;
            this._Manager.getAPIManager().OnLoginDriver();
            UpdateCharacterDropDown();
            int index = CharacterSelection.SelectedIndex;
            long charID = (long)characterInfos[index].CharacterID;
            updateEquipped(charID);
            updateInventory(charID);
            UpdateVault();
            //InitTimer();

        }
        private Timer timer;
        public void InitTimer()
        {
            timer = new Timer(_ => timer1_Tick(), null, 0, 3000 * 10); //every 30 seconds
        }
        private async void timer1_Tick_Refresh(object sender, RoutedEventArgs e)
        {
                this._Manager.getAPIManager().OnLoginDriver();
                int index = CharacterSelection.SelectedIndex;
                long charID = (long)characterInfos[index].CharacterID;
                updateEquipped(charID);
                updateInventory(charID);
                UpdateVault();
        }
            private async void timer1_Tick()
        {

            Dispatcher.Invoke(() => {
                this._Manager.getAPIManager().OnLoginDriver();
                int index = CharacterSelection.SelectedIndex;
                long charID = (long)characterInfos[index].CharacterID;
                updateEquipped(charID);
                updateInventory(charID);
                UpdateVault();
            });
            
            //Refresh;
        }
        public void UpdateCharacterDropDown()
        {
            try
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
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }

        }


        public void CharacterSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DestinyItemCategoryDefinition definition = new DestinyItemCategoryDefinition();
                DestinyInventoryItemDefinition itemDefinition = new DestinyInventoryItemDefinition();
                int index = CharacterSelection.SelectedIndex;
                long charID = (long)characterInfos[index].CharacterID;
                //All destiny defintion
                List<DestinyInventoryItemDefinition> itemsList = new List<DestinyInventoryItemDefinition>();
                this.EquippedList = new List<InventoryItem>();
                updateEquipped(charID);
                updateInventory(charID);
                UpdateVault();
            }
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }


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
            try
            {
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
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }

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
                string SlotName = row["SlotName"].ToString().Trim();
                string ItemName = row["ItemName"].ToString().Trim();
                long ItemHash = long.Parse(row["ItemHash"].ToString());
                long ItemInstanceId = long.Parse(row["ItemInstanceId"].ToString());
                long BucketHash = long.Parse(row["BucketHash"].ToString());
                string rarity = row["Rarity"].ToString();
                if (ItemName == null)
                {
                    continue;
                }
                if (SlotName == "Subclass")
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
                    //this.EquippedList.Insert(0, temp);
                    arlist[0] = temp;
                }
                else if (BucketHash == _BucketHashEnergy)
                {
                   // this.EquippedList.Insert(1, temp);
                    arlist[1] = temp;
                }
                else if (BucketHash == _BucketHashPower)
                {
                    //this.EquippedList.Insert(2, temp);
                    arlist[2] = temp;
                }
                else if (BucketHash == _BucketHashHelmet)
                {
                    //this.EquippedList.Insert(2, temp);
                    arlist[3] = temp;
                }
                else if (BucketHash == _BucketHashGauntlet)
                {
                  //  this.EquippedList.Insert(2, temp);
                    arlist[4] = temp;
                }
                else if (BucketHash == _BucketHashchest)
                {
                   // this.EquippedList.Insert(2, temp);
                    arlist[5] = temp;
                }
                else if (BucketHash == _BucketHashLeg)
                {
                   // this.EquippedList.Insert(2, temp);
                    arlist[6] = temp;
                }
                else if (BucketHash == _BucketHashclassArmor)
                {
                   // this.EquippedList.Insert(2, temp);
                    arlist[7] = temp;
                }
                else
                {
                   // this.EquippedList.Add(temp);
                    arlist.Add(temp);
                }
                
            }
            EquippedItemsChart.ItemsSource = arlist;




        }

        private void UpdateVault()
        {
            VaultList = new List<InventoryItem>();
            long memshipdId = this._Manager.getAPIManager().GetDestinyProfile().MembershipId;

            if (!UseFilters)
            {
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
            }
            else
            {
                DataTable data = this._Manager.getSQLManager().GetVaultFiltered(filterCheckBox,"null", memshipdId);
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

            }

            VaultDisplay.ItemsSource = VaultList;

        }

        private void StoreVaultHandler(InventoryItem inventoryitem)
        {
            try
            {
                APIManager api = this._Manager.getAPIManager();
                int index = CharacterSelection.SelectedIndex;
                long charID = (long)characterInfos[index].CharacterID;
                long DestinyID = api.GetDestinyProfile().MembershipId;
                long ItemHash = (long)inventoryitem.ItemHash;
                long BucketHash = (long)inventoryitem.BucketHash;
                long ItemInstanceID = (long)inventoryitem.ItemInstanceId;
                this._Manager.getSQLManager().TransferInventoryToVault(DestinyID, charID, ItemInstanceID, ItemHash, BucketHash);
                this._Manager.getAPIManager().TransferItem(ItemInstanceID, charID, BungieMembershipType.TigerSteam, ItemHash, true);
                UpdateVault();
                updateInventory(charID);
            }catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }

        }
        private void TransferFromVault(InventoryItem inventoryitem)
        {
            //InventoryItem inventoryitem = (InventoryItem)VaultDisplay.SelectedItem;
            APIManager api = this._Manager.getAPIManager();
            int index = CharacterSelection.SelectedIndex;
            long charID = (long)characterInfos[index].CharacterID;
            long DestinyID = api.GetDestinyProfile().MembershipId;
            long ItemHash = (long)inventoryitem.ItemHash;
            long BucketHash = (long)inventoryitem.BucketHash;
            long ItemInstanceID = (long)inventoryitem.ItemInstanceId;
            this._Manager.getSQLManager().TransferVaultToInventory(DestinyID, charID, ItemInstanceID, ItemHash, BucketHash);
            this._Manager.getAPIManager().TransferItem(ItemInstanceID, charID, BungieMembershipType.TigerSteam, ItemHash, false);

            UpdateVault();
            updateInventory(charID);
        }

        private void EquipHandler(InventoryItem inventoryitem)
        {
            try
            {
                List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
                long instanceID = (long)inventoryitem.ItemInstanceId;
                //long characterID = list[0].CharacterId;
                BungieMembershipType member = BungieMembershipType.TigerSteam;

                int index = CharacterSelection.SelectedIndex;
                long charID = (long)characterInfos[index].CharacterID;

                APIManager api = this._Manager.getAPIManager();
                long DestinyID = api.GetDestinyProfile().MembershipId;
                long ItemHash = (long)inventoryitem.ItemHash;
                long BucketHash = (long)inventoryitem.BucketHash;
                long ItemInstanceID = (long)inventoryitem.ItemInstanceId;
                long CharacterID = charID;
                this._Manager.getSQLManager().EquipItem(DestinyID, ItemHash, BucketHash, ItemInstanceID, charID);
                this._Manager.getAPIManager().EquipItem(ItemInstanceID, charID, BungieMembershipType.TigerSteam);
                updateInventory(CharacterID);
                updateEquipped(CharacterID);
            }catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }

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

        private void VaultKinetic_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem inventoryitem = (InventoryItem)InventoryKinetic.SelectedItem;
            StoreVaultHandler(inventoryitem);
           // EquipHandler(inventoryitem);
        }
        private void VaultEnergy_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryEnergy.SelectedItem;
            StoreVaultHandler(inventoryitem);
            //EquipHandler(inventoryitem);
        }
        private void VaultPower_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryPower.SelectedItem;
            StoreVaultHandler(inventoryitem);
           // EquipHandler(inventoryitem);
        }
        private void VaultHelmet_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryHelmet.SelectedItem;
            StoreVaultHandler(inventoryitem);
            //EquipHandler(inventoryitem);

        }
        private void VaultGauntlet_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryGauntlet.SelectedItem;
            StoreVaultHandler(inventoryitem);
           // EquipHandler(inventoryitem);

        }
        private void VaultChest_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryChest.SelectedItem;
            StoreVaultHandler(inventoryitem);
           // EquipHandler(inventoryitem);

        }
        private void VaultLeg_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryLeg.SelectedItem;
            StoreVaultHandler(inventoryitem);
           // EquipHandler(inventoryitem);

        }
        private void VaultClass_Click(object sender, RoutedEventArgs e)
        {
            List<DestinyCharacterComponent> list = this._Manager.getAPIManager().getCharacterList();
            InventoryItem inventoryitem = (InventoryItem)InventoryClass.SelectedItem;
            StoreVaultHandler(inventoryitem);
          //  EquipHandler(inventoryitem);

        }
        private void VaultEquip_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem inventoryitem = (InventoryItem)VaultDisplay.SelectedItem;
            TransferFromVault(inventoryitem);
            EquipHandler(inventoryitem);
        }
        private void VaultTransfer_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem inventoryitem = (InventoryItem)VaultDisplay.SelectedItem;
            TransferFromVault(inventoryitem);
            //EquipHandler(inventoryitem);
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxItem = (CheckBox)sender;
            if(CheckBoxItem.IsChecked == true)
            {
                switch (CheckBoxItem.Content)
                {
                    case "Kinetic":
                        filterCheckBox[0] = true;
                        break;
                    case "Energy":
                        filterCheckBox[1] = true;
                        break;
                    case "Power":
                        filterCheckBox[2] = true;
                        break;
                    case "Auto Rifle":
                        filterCheckBox[3] = true;
                        break;
                    case "Pulse Rifle":
                        filterCheckBox[4] = true;
                        break;
                    case "Scout Rifle":
                        filterCheckBox[5] = true;
                        break;
                    case "Hand Cannon":
                        filterCheckBox[6] = true;
                        break;
                    case "Side Arms":
                        filterCheckBox[7] = true;
                        break;
                    case "Shotguns":
                        filterCheckBox[8] = true;
                        break;
                    case "Sniper Rifle":
                        filterCheckBox[9] = true;
                        break;
                    case "SMG":
                        filterCheckBox[10] = true;
                        break;
                    case "Helmet":
                        filterCheckBox[11] = true;
                        break;
                    case "Gauntlet":
                        filterCheckBox[12] = true;
                        break;
                    case "Chest":
                        filterCheckBox[13] = true;
                        break;
                    case "Leg":
                        filterCheckBox[14] = true;
                        break;
                    case "Class":
                        filterCheckBox[15] = true;
                        break;
                    case "Rare":
                        filterCheckBox[16] = true;
                        break;
                    case "Legendary":
                        filterCheckBox[17] = true;
                        break;
                    case "Exotic":
                        filterCheckBox[18] = true;
                        break;
                }
            }
            else
            {
                switch (CheckBoxItem.Content)
                {
                    case "Kinetic":
                        filterCheckBox[0] = false;
                        break;
                    case "Energy":
                        filterCheckBox[1] = false;
                        break;
                    case "Power":
                        filterCheckBox[2] = false;
                        break;
                    case "Auto Rifle":
                        filterCheckBox[3] = false;
                        break;
                    case "Pulse Rifle":
                        filterCheckBox[4] = false;
                        break;
                    case "Scout Rifle":
                        filterCheckBox[5] = false;
                        break;
                    case "Hand Cannon":
                        filterCheckBox[6] = false;
                        break;
                    case "Side Arms":
                        filterCheckBox[7] = false;
                        break;
                    case "Shotguns":
                        filterCheckBox[8] = false;
                        break;
                    case "Sniper Rifle":
                        filterCheckBox[9] = false;
                        break;
                    case "SMG":
                        filterCheckBox[10] = false;
                        break;
                    case "Helmet":
                        filterCheckBox[11] = false;
                        break;
                    case "Gauntlet":
                        filterCheckBox[12] = false;
                        break;
                    case "Chest":
                        filterCheckBox[13] = false;
                        break;
                    case "Leg":
                        filterCheckBox[14] = false;
                        break;
                    case "Class":
                        filterCheckBox[15] = false;
                        break;
                    case "Rare":
                        filterCheckBox[16] = false;
                        break;
                    case "Legendary":
                        filterCheckBox[17] = false;
                        break;
                    case "Exotic":
                        filterCheckBox[18] = false;
                        break;
                }
            }
            bool temp = false;
            for (int i = 0; i < filterCheckBox.Count; i++) {
                if (filterCheckBox[i])
                {
                    temp = true;
                }
            }
            UseFilters = temp;
            UpdateVault();
            //InventoryItem inventoryitem = (InventoryItem)VaultDisplay.SelectedItem;
            //TransferFromVault(inventoryitem);
            //EquipHandler(inventoryitem);
        }

        public void GetItemDefinition(long ItemHash)
        {

        }
        public void UpdateItemInfo(object sender, RoutedEventArgs e)
        {
            ListViewItem listViewItem = (ListViewItem)sender;

            for (int j = 0; j < StatList.Count; j++)
            {
                StatList[j].Visibility = Visibility.Collapsed;
            }
            for (int j = 0; j < ProgressBarList.Count; j++)
            {
                ProgressBarList[j].Visibility = Visibility.Collapsed;
            }
            for (int j = 0; j < TextBlockList.Count; j++)
            {
                TextBlockList[j].Visibility = Visibility.Collapsed;
            }
            InventoryItem selectedItem = (InventoryItem)listViewItem.Content;
            DataTable data = this._Manager.getSQLManager().getItemDefName((long)selectedItem.ItemHash);
            foreach (DataRow row in data.Rows)
            {
                var URL = @"https://www.bungie.net/" + row["IconURL"].ToString().Trim();
                long InstanceItemID = (long) selectedItem.ItemInstanceId;
                //AdonisUI.Controls.MessageBox.Show(URL, "Error", AdonisUI.Controls.MessageBoxButton.OK);
                var fullFilePath = @"http://www.americanlayout.com/wp/wp-content/uploads/2012/08/C-To-Go-300x300.png";
                image2.Source = new BitmapImage(new Uri(URL));
                DestinyItemResponse resp = this._Manager.getAPIManager().ItemToolTip(InstanceItemID);
                DestinyItemStatsComponent stats = resp.Stats.Data;
                LightLevel.Text = resp.Instance.Data.PrimaryStat.Value.ToString();
                DataTable defitem = this._Manager.getSQLManager().getItemDefName((long)selectedItem.ItemHash);
                
                ItemType.Text = this._Manager.getSQLManager().GetItemCategoryDefinition(long.Parse(defitem.Rows[0]["ItemCategoryWeapon"].ToString()));
                ItemName.Text = selectedItem.ItemName;
                if (selectedItem.Rarity.Trim().Equals("Common"))
                {
                    ItemName.Foreground = Brushes.White;
                }
                else if (selectedItem.Rarity.Trim().Equals("Uncommon"))
                {
                    ItemName.Foreground = Brushes.Blue;
                }
                else if (selectedItem.Rarity.Trim().Equals("Rare"))
                {
                    ItemName.Foreground = Brushes.LightBlue;
                }
                else if (selectedItem.Rarity.Trim().Equals("Legendary"))
                {
                    ItemName.Foreground = Brushes.Purple;
                }
                else if (selectedItem.Rarity.Trim().Equals("Exotic"))
                {
                    ItemName.Foreground = Brushes.Yellow;
                }
                if (resp.Instance.Data.DamageType == 0)
                {
                    long? energyHash;
                    try
                    {
                        energyHash = resp.Instance.Data.Energy.EnergyTypeHash;
                    }
                    catch (Exception ex)
                    {
                        energyHash = null;
                    }

                    if (energyHash == VoidHash)
                    {
                        ItemElementType.Text = "Void";
                        ItemElementType.Foreground = Brushes.Purple;
                    }
                    else if (energyHash == ArcHash)
                    {
                        ItemElementType.Text = "Arc";
                        ItemElementType.Foreground = Brushes.LightBlue;
                    }
                    else if (energyHash == StasisHash)
                    {
                        ItemElementType.Text = "Stasis";
                        ItemElementType.Foreground = Brushes.Blue;
                    }
                    else if (energyHash == SolarHash)
                    {
                        ItemElementType.Text = "Solar";
                        ItemElementType.Foreground = Brushes.Red;
                    }
                    else
                    {
                        ItemElementType.Text = "Kinetic";
                        ItemElementType.Foreground = Brushes.White;
                    }
                }
                else
                {
                    ItemElementType.Text = resp.Instance.Data.DamageType.ToString();
                    if(ItemElementType.Text == "Stasis")
                    {
                        ItemElementType.Foreground = Brushes.Blue;
                    }
                    else if(ItemElementType.Text == "Arc")
                    {
                        ItemElementType.Foreground = Brushes.LightBlue;
                    }
                    else if (ItemElementType.Text == "Thermal")
                    {
                        ItemElementType.Foreground = Brushes.Red;
                    }
                    else if (ItemElementType.Text == "Kinetic")
                    {
                        ItemElementType.Foreground = Brushes.White;
                    }
                    else if (ItemElementType.Text == "Void")
                    {
                        ItemElementType.Foreground = Brushes.Purple;
                    }
                    ProgressBarList[0].Visibility = Visibility.Visible;
                }
                
                
                
                for (int j = 0; j < StatList.Count; j++)
                {

                        StatList[j].Visibility = Visibility.Visible;
                        ProgressBarList[j].Visibility = Visibility.Visible;
                        TextBlockList[j].Visibility = Visibility.Visible;

                }
                int i = 0;
                foreach (KeyValuePair<uint, DestinyStat> entry in stats.Stats)
                {
                    String Name = this._Manager.getSQLManager().GetStatDefinition(entry.Key);
                    int StatValue = entry.Value.Value;
                    DataTable table = this._Manager.getSQLManager().getItemDefName((long)selectedItem.ItemHash);
                    long WeaponCategory = long.Parse(table.Rows[0]["ItemCategoryWeapon"].ToString());
                    
                    if(WeaponCategory == 1)
                    {
                        if (StatValue > 0 || Name == "")
                        {
                            TextBlockList[i].Text = Name;
                            ProgressBarList[i].Value = StatValue;
                            ProgressBarList[i].Maximum = 50;
                            StatList[i].Text = StatValue.ToString();
                            i++;
                        }

                    }
                    else
                    {
                        if (StatValue > 0 || Name == "")
                        {
                            TextBlockList[i].Text = Name;
                            ProgressBarList[i].Value = StatValue;
                            ProgressBarList[i].Maximum = 150;
                            StatList[i].Text = StatValue.ToString();
                            i++;
                        }
                    }
                    

                }




            }
            

        }


        private void updateManifests_Click(object sender, RoutedEventArgs e)
        {

            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Outputs\DestinyStatDefinition.json");
            string path = System.IO.Path.GetFullPath(sFile);
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                JObject j = new JObject();
                JObject items = JsonConvert.DeserializeObject<JObject>(json);
                dynamic k = items;

                foreach (var item in items)
                {

                    this._Manager.getSQLManager().UpdateStatManifest(long.Parse(item.Key),
                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString());

                    if (items.SelectToken($"{item.Key}.displayProperties.name") != null)
                    {
                        /*                        //BucketManifest
                                                this._Manager.getAPIManager().updateBucketManifest(long.Parse(item.Key), 
                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString());*/


                        try
                        {

                            

                            //Item Definition Manifest
                            /*                            this._Manager.getAPIManager().updateManifest(
                                                                    long.Parse(item.Key),
                                                                    long.Parse(items.SelectToken($"{item.Key}.inventory.bucketTypeHash").ToString()),
                                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString(),
                                                                    items.SelectToken($"{item.Key}.displayProperties.description").ToString(),
                                                                    items.SelectToken($"{item.Key}.inventory.tierTypeName").ToString(),
                                                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[0]").ToString()),
                                                                    items.SelectToken($"{item.Key}.displayProperties.icon").ToString(),
                                                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[1]").ToString()),
                                                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[2]").ToString()));*/

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
