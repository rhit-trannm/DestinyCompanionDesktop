using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Entities.Characters;
using System;
using System.Collections.Generic;
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
        public Inventory(Manager manager)
        {
            InitializeComponent();
            List<EquippedItem> persons = new List<EquippedItem>();
            persons.Add(new EquippedItem() { SlotName = "Kinetic", ItemName = "Ace of Spades" });
            EquippedItemsChart.ItemsSource = persons;
            List<DestinyClass> classList = new List<DestinyClass>();
            foreach (DestinyCharacterComponent entry in manager.getAPIManager().getCharacterList())
            {
                classList.Add(entry.ClassType);
            }
            CharacterSelection.ItemsSource = classList;
            List<String> temp = new List<String>();
            temp.Add("Hell1");
            temp.Add("Hell2");
            temp.Add("Hell3");
            temp.Add("Hell4");
            //InventoryKinetic.ItemsSource = temp;


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
            public string ItemName { set; get; }
        }

        private void CharacterSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = CharacterSelection.SelectedIndex;
            List<DestinyCharacterComponent> list = manager.getAPIManager().getCharacterList();
            manager.getAPIManager().GetEquipped(list.ElementAt(index).CharacterId);
        }
    }
    }
