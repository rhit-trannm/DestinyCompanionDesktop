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
        Manager manager = new Manager();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void getURL(object sender, RoutedEventArgs e)
        {
            manager.Authenticate();
        }

        private void Authentication_Button(object sender, RoutedEventArgs e)
        {
            string s = manager.getAccessToken(textBox.Text);
            if(s == "True")
            {
                Inventory inventoryWindow = new Inventory();
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
