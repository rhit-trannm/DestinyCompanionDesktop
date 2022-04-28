using System;
using System.Diagnostics;
using System.IO;
using BungieSharper;

using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Responses;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class Manager
{

	String apiKey = "7c1528dadd144643b93a7ceb2fff5685";
	BungieSharper.Client.BungieApiClient client;
	
	public Manager()
	{
		BungieSharper.Client.BungieClientConfig config = new BungieSharper.Client.BungieClientConfig();
		config.OAuthClientId = 39661;
		config.ApiKey = apiKey;
		client = new BungieSharper.Client.BungieApiClient(config);
	}

	void Authenticate()
    {
		string url = client.OAuth.GetOAuthAuthorizationUrl();
		var psi = new ProcessStartInfo
		{
			FileName = url,
			UseShellExecute = true
		};
		Process.Start(psi);
	}
	public string LoadJson()
	{
		string path = @"..\..\..\output\DestinyClassDefinition.json";
		using (StreamReader r = new StreamReader(path))
		{
			string json = r.ReadToEnd();
			JObject j = new JObject();
			JObject items = JsonConvert.DeserializeObject<JObject>(json);
			dynamic k = items;

			foreach(var item in items)
            {
				Console.WriteLine(item.Key);
				Console.WriteLine(items.SelectToken($"{item.Key}.classType"));
            }
		
			//Console.Write(items);
			/*			foreach (BungieSharper.Entities.Destiny.Entities.Items.DestinyItemComponent element in items)
						{
							Console.Write($"{element.ItemHash} \n");
						}*/
		}
		return null;
	}

	public class Item
	{
		public string name;
	}


}
