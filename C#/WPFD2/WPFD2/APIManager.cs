using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BungieSharper;
using BungieSharper.Entities;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.Entities.Inventory;
using BungieSharper.Entities.Destiny.Entities.Items;
using BungieSharper.Entities.Destiny.Responses;

namespace WPFD2
{
    public class APIManager
    {
        String apiKey = "7c1528dadd144643b93a7ceb2fff5685";
        BungieSharper.Client.BungieApiClient client;
        BungieSharper.Entities.TokenResponse Token;
        long d2memId;
        public APIManager()
        {
            BungieSharper.Client.BungieClientConfig config = new BungieSharper.Client.BungieClientConfig();
            config.OAuthClientId = 39661;
            config.OAuthClientSecret = "zGpr-mMHVnYhIYJODAlphLzvsiQQ4HfgyFjrZU2UVvE";
            config.ApiKey = apiKey;
            client = new BungieSharper.Client.BungieApiClient(config);
        }

        public void Authenticate() 
        {
            string url = client.OAuth.GetOAuthAuthorizationUrl();
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        public string getAccessToken(string authToken)
        {
            BungieSharper.Entities.TokenResponse token = client.OAuth.GetOAuthToken(authToken).Result;
            if(token.AccessToken == null)
            {
                //kekw
                return token.ErrorDescription;
            }
            else
            {
                Token = token;
           
                return "True";
            }
        }
        public string profile()
        {

            BungieSharper.Entities.Destiny.Responses.DestinyLinkedProfilesResponse resp =
                client.Api.Destiny2_GetLinkedProfiles((long)Token.MembershipId, BungieSharper.Entities.BungieMembershipType.TigerSteam).Result;
            IEnumerable<DestinyProfileUserInfoCard> profileEnumerator = resp.Profiles;
            foreach(DestinyProfileUserInfoCard profile in profileEnumerator)
            {
                if(profile.MembershipType == BungieSharper.Entities.BungieMembershipType.TigerSteam)
                {
                    d2memId = profile.MembershipId;
                    return profile.MembershipId.ToString();
                }
            }
            return "No Profile found";






        }

        public string getInventory() {
            List<DestinyComponentType> components = new List<DestinyComponentType>();
            components.Add(DestinyComponentType.Characters);
      
            DestinyProfileResponse resp = client.Api.Destiny2_GetProfile(d2memId, BungieSharper.Entities.BungieMembershipType.TigerSteam, components).Result;
            DictionaryComponentResponseOfint64AndDestinyCharacterComponent a = resp.Characters;
            Dictionary<long, DestinyCharacterComponent> characterDic = a.Data;

            List<long> idList = new List<long>(characterDic.Keys);
            components = new List<DestinyComponentType>();
            components.Add(DestinyComponentType.CharacterEquipment);
            DestinyCharacterResponse resp2 = client.Api.Destiny2_GetCharacter(idList[0], d2memId, BungieSharper.Entities.BungieMembershipType.TigerSteam, components).Result;
            SingleComponentResponseOfDestinyInventoryComponent d = resp2.Equipment;
            DestinyInventoryComponent f = d.Data;
            IEnumerable<DestinyItemComponent> g = f.Items;
            StringBuilder sb = new StringBuilder();
            foreach(var item in g)
            {
                sb.Append(item.ItemHash + "\n");
            }
            return sb.ToString();
        }

        
    }
}
