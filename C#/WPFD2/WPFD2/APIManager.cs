﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BungieSharper;
using BungieSharper.Entities;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.Definitions;
using BungieSharper.Entities.Destiny.Entities.Characters;
using BungieSharper.Entities.Destiny.Entities.Inventory;
using BungieSharper.Entities.Destiny.Entities.Items;
using BungieSharper.Entities.Destiny.Responses;
using BungieSharper.Entities.User;

namespace WPFD2
{
    public class APIManager
    {
        String apiKey = "7c1528dadd144643b93a7ceb2fff5685";
        BungieSharper.Client.BungieApiClient Client;
        BungieSharper.Entities.TokenResponse Token;
        DestinyLinkedProfilesResponse _UserProfile;
        DestinyProfileUserInfoCard _DestinyProfile;
        List<DestinyCharacterComponent> _CharacterList;
        UserInfoCard _BnetProfile;
        public APIManager()
        {
            BungieSharper.Client.BungieClientConfig config = new BungieSharper.Client.BungieClientConfig();
            config.OAuthClientId = 39661;
            config.OAuthClientSecret = "zGpr-mMHVnYhIYJODAlphLzvsiQQ4HfgyFjrZU2UVvE";
            config.ApiKey = apiKey;
            Client = new BungieSharper.Client.BungieApiClient(config);
        }
        public List<DestinyCharacterComponent> getCharacterList()
        {
            return _CharacterList;
        }
        public void Authenticate() 
        {
            string url = Client.OAuth.GetOAuthAuthorizationUrl();
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        public string getAccessToken(string authToken)
        {
            BungieSharper.Entities.TokenResponse token = Client.OAuth.GetOAuthToken(authToken).Result;
            if(token.AccessToken == null)
            {
                return token.ErrorDescription;
            }
            Token = token;
            _UserProfile = Client.Api.Destiny2_GetLinkedProfiles((long)Token.MembershipId, BungieSharper.Entities.BungieMembershipType.TigerSteam).Result;
            IEnumerable<DestinyProfileUserInfoCard> profileEnumerator = _UserProfile.Profiles;
            foreach (DestinyProfileUserInfoCard profile in profileEnumerator)
            {
                if (profile.MembershipType == BungieSharper.Entities.BungieMembershipType.TigerSteam)
                {
                    _DestinyProfile = profile;
                }
            }
            GetCharacter();
            return "True";
        }
        private List<DestinyCharacterComponent> GetCharacter()
        {
            _CharacterList = new List<DestinyCharacterComponent>();
            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.Characters);
            DestinyProfileResponse resp = Client.Api.Destiny2_GetProfile(_DestinyProfile.MembershipId,BungieMembershipType.TigerSteam,query, Token.AccessToken).Result;
            Dictionary<long, DestinyCharacterComponent> Data = resp.Characters.Data;
            foreach (KeyValuePair<long, DestinyCharacterComponent> entry in Data)
            {
                _CharacterList.Add(entry.Value);
            }
            return _CharacterList;
        }
        public IEnumerable<DestinyItemComponent> GetEquipped(long characterID)
        {
            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.CharacterEquipment);
            DestinyCharacterResponse resp = Client.Api.Destiny2_GetCharacter(characterID, _DestinyProfile.MembershipId, BungieMembershipType.TigerSteam, query, Token.AccessToken).Result;
            IEnumerable<DestinyItemComponent> Items = resp.Equipment.Data.Items;
            /*
            Items.GetEnumerator();
            AggregateDestinyDefinitions def = new AggregateDestinyDefinitions();
            Dictionary<uint, DestinyInventoryItemDefinition> InventoryItems = def.InventoryItems;
            List<DestinyInventoryItemDefinition> itemsList = new List<DestinyInventoryItemDefinition>();
            foreach (DestinyItemComponent item in Items)
            {
                itemsList.Add(InventoryItems[item.ItemHash]);
            }
            */

            return Items;
        }
        public void GetItem(long instanceid)
        {
            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.ItemInstances);
            DestinyItemResponse resp = Client.Api.Destiny2_GetItem(_DestinyProfile.MembershipId, instanceid, BungieMembershipType.TigerSteam, query, Token.AccessToken).Result;

        }
/*        public string profile()
        {

            BungieSharper.Entities.Destiny.Responses.DestinyLinkedProfilesResponse resp =
                
            IEnumerable<DestinyProfileUserInfoCard> profileEnumerator = resp.Profiles;
            foreach(DestinyProfileUserInfoCard profile in profileEnumerator)
            {
                if(profile.MembershipType == BungieSharper.Entities.BungieMembershipType.TigerSteam)
                {
                    d2memId = profile.MembershipId;
                    userName = profile.DisplayName;
                    return profile.MembershipId.ToString();
                }
            }
            return "No Profile found";

        }*/

        public string GetInventory() {
            List<DestinyComponentType> components = new List<DestinyComponentType>();
            components.Add(DestinyComponentType.Characters);
            DestinyProfileResponse resp = Client.Api.Destiny2_GetProfile(_DestinyProfile.MembershipId, BungieSharper.Entities.BungieMembershipType.TigerSteam, components).Result;
            DictionaryComponentResponseOfint64AndDestinyCharacterComponent a = resp.Characters;
            Dictionary<long, DestinyCharacterComponent> characterDic = a.Data;
            List<long> idList = new List<long>(characterDic.Keys);
            components = new List<DestinyComponentType>();
            components.Add(DestinyComponentType.CharacterEquipment);
            DestinyCharacterResponse resp2 = Client.Api.Destiny2_GetCharacter(idList[0], _DestinyProfile.MembershipId, BungieSharper.Entities.BungieMembershipType.TigerSteam, components).Result;
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
