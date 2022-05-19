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


        private void id_read_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void updateBucketManifest()
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
                        try
                        {
                            this.manager.getAPIManager().updateBucketManifest(long.Parse(item.Key),
                                                        items.SelectToken($"{item.Key}.displayProperties.name").ToString());
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

        }

        private void updateItemManifest()
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
                        try
                        {
                            this.manager.getAPIManager().updateManifest(
                                                                    long.Parse(item.Key),
                                                                    long.Parse(items.SelectToken($"{item.Key}.inventory.bucketTypeHash").ToString()),
                                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString(),
                                                                    items.SelectToken($"{item.Key}.displayProperties.description").ToString(),
                                                                    items.SelectToken($"{item.Key}.inventory.tierTypeName").ToString(),
                                                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[0]").ToString()),
                                                                    items.SelectToken($"{item.Key}.displayProperties.icon").ToString(),
                                                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[1]").ToString()),
                                                                    long.Parse(items.SelectToken($"{item.Key}.itemCategoryHashes[2]").ToString()));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

        }

        private void updateCategoryManifest()
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
                        try
                        {
                            this.manager.getSQLManager().AddCategoryDefinition(
                                                                    long.Parse(item.Key),
                                                                    items.SelectToken($"{item.Key}.displayProperties.name").ToString());
                        }
                        catch (Exception)
                        {

                        }

                    }

                }
            }
        }

        private void updateManifest_Click(object sender, RoutedEventArgs e)
        {
            updateBucketManifest();
            updateItemManifest();
            updateCategoryManifest();
            //Console.Write(items);
            /*			foreach (BungieSharper.Entities.Destiny.Entities.Items.DestinyItemComponent element in items)
                        {
                            Console.Write($"{element.ItemHash} \n");
                        }*/
        }
    }
}
