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
            this.manager = manager;

            /*blah.Text = manager.getAPIManager().profile();
            Items.Text = manager.getAPIManager().getInventory();
            string user = manager.getAPIManager().getUserName();
            long id = manager.getAPIManager().getD2MemID();
            username.Text = "Username: " + user;
            userid.Text = "UserID: " + id.ToString();
            manager.getSQLManager().initiateUser(id);*/
            //textBox.Text = sql.starter();

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
