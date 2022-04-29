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
    /// Interaction logic for DevPage.xaml
    /// </summary>
    public partial class DevPage : Window
    {
        Manager manager;
        public DevPage(Manager manager)
        {
            this.manager = manager;
            InitializeComponent();

            textBlock.Text = this.manager.getSQLManager().initiateUser(1);
           
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(userid.Text);
            this.manager.getSQLManager().createUser(id, name.Text);

        }

        private void read(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(id_read.Text);
            textBlock.Text = this.manager.getSQLManager().initiateUser(id);



        }

        private void id_read_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
