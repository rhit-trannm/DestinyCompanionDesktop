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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFD2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool DEVELOPER_MODE = false;
        Manager manager = new Manager();
        public MainWindow()
        {
            InitializeComponent();
            Inventory inventoryWindow = new Inventory(manager);
            inventoryWindow.Show();
            if (DEVELOPER_MODE)
            {
                DevPage devPage = new DevPage(manager);
                devPage.Show();


                this.Hide();
            }
            
        }

        private void getURL(object sender, RoutedEventArgs e)
        {
            manager.getAPIManager().Authenticate();
        }

        private void Authentication_Button(object sender, RoutedEventArgs e)
        {
            string s = manager.getAPIManager().getAccessToken(textBox.Text);
            if(s == "True")
            {
                Inventory inventoryWindow = new Inventory(manager);
                inventoryWindow.Show();
                this.Hide();
            }
            else
            {
                textBlock.Text = s;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
