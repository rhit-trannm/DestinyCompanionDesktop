using System;
using System.Diagnostics;

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

	
}
