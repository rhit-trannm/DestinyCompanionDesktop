using System;
using System.Collections;
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
using BungieSharper.Entities.Destiny.Requests;
using BungieSharper.Entities.Destiny.Requests.Actions;
using BungieSharper.Entities.Destiny.Responses;
using BungieSharper.Entities.User;

namespace WPFD2
{
    public class APIManager
    {
        String apiKey = "7c1528dadd144643b93a7ceb2fff5685";
        BungieSharper.Client.BungieApiClient Client;
        private BungieSharper.Entities.TokenResponse Token;
        private DestinyLinkedProfilesResponse _UserProfile;
        public DestinyLinkedProfilesResponse GetUserProfile()
        {
            return _UserProfile;
        }
        private DestinyProfileUserInfoCard _DestinyProfile;

        public DestinyProfileUserInfoCard GetDestinyProfile()
        {
            return _DestinyProfile;
        }

        private List<DestinyCharacterComponent> _CharacterList;

        public List<DestinyCharacterComponent> GetCharacterList()
        {
            return _CharacterList;
        }

        SQLManager SQL = new SQLManager();
        public APIManager()
        {
            BungieSharper.Client.BungieClientConfig config = new BungieSharper.Client.BungieClientConfig();
            config.OAuthClientId = 39661;
            config.OAuthClientSecret = "zGpr-mMHVnYhIYJODAlphLzvsiQQ4HfgyFjrZU2UVvE";
            config.ApiKey = apiKey;
            Client = new BungieSharper.Client.BungieApiClient(config);
            Token = new TokenResponse();
            _UserProfile = new DestinyLinkedProfilesResponse();
            _DestinyProfile = new DestinyProfileUserInfoCard();
            _CharacterList = new List<DestinyCharacterComponent>();
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
            if (token.AccessToken == null)
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
            userCheck();
            GetCharacter();
            OnLoginDriver();
            return "True";
        }
        public void userCheck()
        {
            if(SQL.checkUserExist(Token.MembershipId) == 0)
            {
                SQL.createUser(Token.MembershipId, _DestinyProfile.MembershipId, _DestinyProfile.DisplayName);
            }
            
        }
        private List<DestinyCharacterComponent> GetCharacter()
        {
            _CharacterList = new List<DestinyCharacterComponent>();
            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.Characters);
            DestinyProfileResponse resp = Client.Api.Destiny2_GetProfile(_DestinyProfile.MembershipId, BungieMembershipType.TigerSteam, query, Token.AccessToken).Result;
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
        public string EquipItem(long ItemInstanceID, long CharacterID, BungieMembershipType membershipType)
        {
            DestinyItemActionRequest request = new DestinyItemActionRequest();
            request.ItemId = ItemInstanceID;
            request.CharacterId = CharacterID;
            request.MembershipType = membershipType;
            try
            {
                int s = Client.Api.Destiny2_EquipItem(request, Token.AccessToken).Result;
            }catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }
            
            //OnLoginDriver();
            return "?";
        }
        public void TransferItem(long ItemInstanceID, long CharacterID, BungieMembershipType membershipType, long ItemHash, bool ToVault)
        {
            DestinyItemTransferRequest request = new DestinyItemTransferRequest();
            request.ItemId = ItemInstanceID;
            request.ItemReferenceHash = (uint)ItemHash;
            request.TransferToVault = ToVault;
            request.CharacterId = CharacterID;
            request.MembershipType = membershipType;
            try
            {
                int s = Client.Api.Destiny2_TransferItem(request, Token.AccessToken).Result;
            }
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.ToString(), "Error", AdonisUI.Controls.MessageBoxButton.OK);
            }

            //OnLoginDriver();
            //return "?";
        }

        /* OnLogin Will essentially act as a refresh from Destiny Database to SQL Database
         * 
         * 
         */
        public void OnLoginDriver()
        {
            List<long> CharacterID = new List<long>();
            List<long> DestinyMembershipID = new List<long>();
            List<Int32> ClassType = new List<Int32>();
            for (int i = 0; i < _CharacterList.Count; i++)
            {
                CharacterID.Add(_CharacterList[i].CharacterId);
                DestinyMembershipID.Add(_DestinyProfile.MembershipId);
                //this is scuff. Be very careful!
                if(_CharacterList[i].ClassType == DestinyClass.Titan)
                {
                    ClassType.Add(22);
                }
                else if (_CharacterList[i].ClassType == DestinyClass.Hunter)
                {
                    ClassType.Add(23);
                }
                else if (_CharacterList[i].ClassType == DestinyClass.Warlock)
                {
                    ClassType.Add(21);
                }
                
            }

            List<long> DestinyID = new List<long>();
            List<long> CharID = new List<long>();
            List<long> ItemHash = new List<long>();
            List<long> ItemInstanceID = new List<long>();
            List<long> BucketHash = new List<long>();
            for (int i = 0; i < _CharacterList.Count; i++)
            {
                IEnumerable<DestinyItemComponent>  enumers = GetEquipped(_CharacterList[i].CharacterId);
                foreach (DestinyItemComponent item in enumers)
                {
                    DestinyID.Add(_DestinyProfile.MembershipId);
                    CharID.Add(_CharacterList[i].CharacterId);
                    ItemHash.Add(item.ItemHash);
                    if(item.ItemInstanceId != null)
                    {
                        ItemInstanceID.Add((long)item.ItemInstanceId);
                    }
                    else
                    {
                        continue;
                    }
                    BucketHash.Add(item.BucketHash);

                }
            }
            //List<long> IDestinyID = new List<long>();
            List<long> ICharID = new List<long>();
            List<long> IItemHash = new List<long>();
            List<long> IItemInstanceID = new List<long>();
            List<long> IBucketHash = new List<long>();
            for (int i = 0; i < _CharacterList.Count; i++)
            {
                IEnumerable<DestinyItemComponent> enumers = GetInventory(_CharacterList[i].CharacterId);
                foreach (DestinyItemComponent item in enumers)
                {
                    //DestinyID.Add(_DestinyProfile.MembershipId);
                    ICharID.Add(_CharacterList[i].CharacterId);
                    IItemHash.Add(item.ItemHash);
                    if (item.ItemInstanceId != null)
                    {
                        IItemInstanceID.Add((long)item.ItemInstanceId);
                    }
                    else
                    {
                        continue;
                    }
                    IBucketHash.Add(item.BucketHash);

                }
            }
            List<long> VaultItemHash = new List<long>();
            List<long> VaultItemInstanceID = new List<long>();
            IEnumerable<DestinyItemComponent> vaultenum = GetVault();
            int j = 0;
               
                foreach (DestinyItemComponent item in vaultenum)
                {
                    //DestinyID.Add(_DestinyProfile.MembershipId);
                    VaultItemHash.Add(item.ItemHash);
                    if (item.ItemInstanceId != null)
                    {
                        VaultItemInstanceID.Add((long)item.ItemInstanceId);
                    }
                    else
                    {
                        continue;
                    }
                j++;
                }
            

            this.SQL.OnLogin(Token.MembershipId, _DestinyProfile.DisplayName, CharacterID, DestinyMembershipID, ClassType, 
                DestinyID, CharID, ItemHash, ItemInstanceID, BucketHash, ICharID, IItemHash, IItemInstanceID, IBucketHash,
                VaultItemHash, VaultItemInstanceID);
            
        }
        public void updateManifest(long ItemHash, long BucketHash, string name, string description, string tierTypeName, long? ItemCategoryClass, 
            string IconURL, long? ItemCategoryArmor, long? ItemCategoryWeapon)
        {
            SQL.AddDestinyItemDefinition(ItemHash, BucketHash, name, description, tierTypeName, ItemCategoryClass, IconURL, ItemCategoryArmor, ItemCategoryWeapon);
        }
        public void updateBucketManifest(long bucketHash, string name)
        {
            SQL.AddDestinyBucketDefinition(bucketHash, name);
        }
        public DestinyItemResponse ItemToolTip(long instanceid)
        {
            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.ItemInstances);
            query.Add(DestinyComponentType.ItemPerks);
            query.Add(DestinyComponentType.ItemStats);
            query.Add(DestinyComponentType.ItemSockets);
            query.Add(DestinyComponentType.ItemTalentGrids);
            query.Add(DestinyComponentType.ItemCommonData);
            query.Add(DestinyComponentType.ItemPlugObjectives);
            query.Add(DestinyComponentType.ItemPlugStates);
            query.Add(DestinyComponentType.ItemReusablePlugs);
            DestinyItemResponse resp = Client.Api.Destiny2_GetItem(_DestinyProfile.MembershipId, instanceid, BungieMembershipType.TigerSteam, query, Token.AccessToken).Result;
            return resp;
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

        public IEnumerable<DestinyItemComponent> GetInventory(long characterID)
        {
            /*            List<DestinyComponentType> components = new List<DestinyComponentType>();
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
                        return sb.ToString();*/

            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.CharacterInventories);
            DestinyCharacterResponse resp = Client.Api.Destiny2_GetCharacter(characterID, _DestinyProfile.MembershipId, BungieMembershipType.TigerSteam, query, Token.AccessToken).Result;
            IEnumerable<DestinyItemComponent> Items = resp.Inventory.Data.Items;
            return Items;
        }
        public IEnumerable<DestinyItemComponent> GetVault()
        {
            List<DestinyComponentType> query = new List<DestinyComponentType>();
            query.Add(DestinyComponentType.ProfileInventories);
            DestinyProfileResponse resp = Client.Api.Destiny2_GetProfile(_DestinyProfile.MembershipId, BungieMembershipType.TigerSteam, query, Token.AccessToken).Result;
            IEnumerable<DestinyItemComponent> Items = resp.ProfileInventory.Data.Items;
            return Items;
        }


    }
}
