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


namespace CDestiny2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            String apiKey = "7c1528dadd144643b93a7ceb2fff5685";
            BungieSharper.Client.BungieApiClient client;

            
            BungieSharper.Client.BungieClientConfig config = new BungieSharper.Client.BungieClientConfig();
            config.OAuthClientSecret = "zGpr-mMHVnYhIYJODAlphLzvsiQQ4HfgyFjrZU2UVvE";
            config.OAuthClientId = 39661;
            config.ApiKey = apiKey;
            client = new BungieSharper.Client.BungieApiClient(config);
            BungieSharper.Entities.Destiny.Config.DestinyManifest manifest = client.Api.Destiny2_GetDestinyManifest().Result;
            Manager man = new Manager();
            man.LoadJson();
            //Console.WriteLine(manifest.MobileAssetContentPath);

            /*            string url = client.OAuth.GetOAuthAuthorizationUrl();

                        var psi = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };
                        Process.Start(psi);*/

            //BungieSharper.Entities.TokenResponse token = client.OAuth.GetOAuthToken("595ecf909d5863264b55d0ac7bab668a").Result;

            //Console.WriteLine(token.ErrorDescription);
            /*            string cs = @"Server=titan.csse.rose-hulman.edu; Encrypt=False; Database=CSSE333_S4G1_FinalProjectDB; UID=trannm; Password=Acixuw+03";
                        try
                        {
                            SqlConnection conn = new SqlConnection(cs);
                            //MySqlConnection con = new MySqlConnection(cs);
                            conn.Open();
                            Console.WriteLine($"MySQL version : {conn.ServerVersion}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: {0}", e.ToString());
                        }*/





            /*string s = "COWJBBKGAgAgMtxOtnwuNkNK9vWNnh2Si4jzT3005d95WeoKBImUvYfgAAAAbvmtXoHzWjTOLnqtyN/HYtWPINh4eYIh57Hn9lQ777QkyBIQx6LUqCqhc8RV0XJWFyv4EHPF7iUrIN/uulB7xda9b5oXvTt9HwrSuIePrpJLyBhhmrNTj+AXtS2K7wyrsHabAFwD1oCJ2RNDMGlWnY1N8KFDrHtsKZmvVAwrSrKO9ro3GJehDXoXs3TRwkp1OhXg9RDJT1gHXFMqF/Xf77mEn81vU8SPyULNLj3+k8HFD5XKXenTawtJ6+BABWaOQZMUKdPrhV9sw3ti2gGGAEwH4uXlRu0ATM6sowOU8rk=";

            BungieSharper.Entities.User.GeneralUser user = client.Api.User_GetBungieNetUserById(21139663).Result;
            long membershipid = 4611686018472301094;
            List<DestinyComponentType> components = new List<DestinyComponentType>();
            components.Add(DestinyComponentType.Profiles);
            components.Add(DestinyComponentType.Characters);

            IEnumerable<DestinyComponentType> search = components;

            Console.WriteLine(string.Join(",", components.Select((DestinyComponentType x) => x.ToString())));
            
            DestinyProfileResponse response = client.Api.Destiny2_GetProfile(membershipid, BungieSharper.Entities.BungieMembershipType.TigerSteam, components, s).Result;


            Console.WriteLine(user.DisplayName);*/
            //Console.WriteLine(response.Profile.Data.);

            //BungieSharper.Entities.Destiny.Responses.DestinyCharacterResponse character;




        }

        Boolean DownloadToFile(String URL, String fileName, BungieSharper.Client.BungieApiClient client)
        {
            string path = @"C:\CSSE333\CSSE333Project\C#\CDestiny2\Output\"+fileName+".json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            String s = client.DownloadString(URL).Result;
            File.WriteAllTextAsync(path, s);
            return true;
        }



    }
}
