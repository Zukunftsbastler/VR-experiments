using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;
using VR_Experiment.Avatar.Expo;
using VR_Experiment.Menu.Hud;
using VR_Experiment.Networking;
using VR_Experiment.XR;

namespace VR_Experiment.Core
{
    public class PlayerWrapper : SingletonBehaviour<PlayerWrapper>
    {
        private const string AVATAR_KEY = "avatar";
        private const string ROLE_KEY = "role";
        private const string PRODUCT_KEY = "product";

        private PlayerNetworkInfo _networkInfo = null;

        private XRRig _rig = null;
        private PlayerHud _hud = null;

        private AvatarLinkBehaviour _avatarLink = null;

        public bool HasAvatar => string.IsNullOrEmpty(GetLocalAvatar()) == false;
        public bool HasRole => GetLocalRole() != Role.None;
        public bool HasActiveProduct => string.IsNullOrEmpty(GetLocalActiveProduct()) == false;
        [Obsolete]
        public bool CanOccupyBooths => GetLocalRole() > Role.Visitor;
        public bool CanManageProducts => GetLocalRole() > Role.Visitor;

        public XRRig Rig => _rig ??= FindObjectOfType<XRRig>();
        public PlayerHud Hud => _hud ??= FindObjectOfType<PlayerHud>();

        public event Action onPropertiesChanged;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // --- Set Properties --------------------------------------------------------------------------------------------------
        public void SetPlayerData(PlayerNetworkInfo networkInfo, GameObject avatarPrefab, Role role)
        {
            SetNetworkInfo(networkInfo);
            SetAvatar(avatarPrefab, false);
            SetRole(role);
        }

        public void SetNetworkInfo(PlayerNetworkInfo networkInfo)
        {
            _networkInfo = networkInfo;

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                bool propertiesHaveChanged = photonInfo.Client.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                {AVATAR_KEY, ""},
                {ROLE_KEY, Role.None},
                {PRODUCT_KEY, ""},
            });

                if(propertiesHaveChanged)
                    onPropertiesChanged?.Invoke();
            }
        }

        public void SetAvatar(GameObject avatarPrefab, bool updateAvatar)
        {
            if(updateAvatar)
            {
                if(_avatarLink != null)
                {
                    Destroy(_avatarLink.gameObject);
                }

                if(avatarPrefab != null)
                {
                    GameObject avatar = Instantiate(avatarPrefab, Rig.transform.position, Quaternion.identity);
                    avatar.transform.SetParent(this.transform);

                    LinkAvatarToRig(avatar);
                }
            }

            string avatarName = avatarPrefab != null ? avatarPrefab.name : "";

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                bool propertiesHaveChanged = photonInfo.Client.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                {
                    {AVATAR_KEY, avatarName}
                });

                if(propertiesHaveChanged)
                    onPropertiesChanged?.Invoke();
            }
        }

        public void SetRole(Role role)
        {
            if(GetLocalRole() == role)
                return;

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                bool propertiesHaveChanged = photonInfo.Client.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                {
                    { ROLE_KEY, role }
                });

                if(propertiesHaveChanged)
                    onPropertiesChanged?.Invoke();
            }
        }

        public void SetActiveProduct(string productName)
        {
            if(productName.Equals(GetLocalActiveProduct()))
                return;

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                bool propertiesHaveChanged = photonInfo.Client.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                {
                    { PRODUCT_KEY, productName }
                });

                if(propertiesHaveChanged)
                    onPropertiesChanged?.Invoke();
            }
        }

        // --- Get Properties --------------------------------------------------------------------------------------------------
        /// <summary>
        /// </summary>
        /// <returns>Actor number of this player in current network room.</returns>
        public int GetGlobalSlot()
        {
            if(_networkInfo == null)
                return -1;

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                return photonInfo.Client.ActorNumber;
            }

            return -1;
        }

        /// <summary>
        /// </summary>
        /// <returns>Avatar prefab name of this player or empty string.</returns>
        public string GetLocalAvatar()
        {
            if(_networkInfo == null)
                return "";

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                return GetAvatar(photonInfo.Client);
            }

            return "";
        }

        /// <summary>
        /// </summary>
        /// <returns>Role of this player or <see cref="Role.None"/>.</returns>
        public Role GetLocalRole()
        {
            if(_networkInfo == null)
                return Role.None;

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                return GetRole(photonInfo.Client);
            }

            return Role.None;
        }

        /// <summary>
        /// </summary>
        /// <returns>Product name of this player or empty string.</returns>
        public string GetLocalActiveProduct()
        {
            if(_networkInfo == null)
                return "";

            if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
            {
                return GetActiveProduct(photonInfo.Client);
            }

            return "";
        }

        /// <summary>
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Avatar prefab name in <see cref="Player.CustomProperties"/> or empty string.</returns>
        public static string GetAvatar(Player player)
        {
            string avatarName;
            if(player.CustomProperties.TryGetValue(AVATAR_KEY, out object avatarValue))
            {
                avatarName = (string)avatarValue;
            }
            else
            {
                Debug.LogError($"No CustomProperty found for key: {nameof(AVATAR_KEY)}, '{AVATAR_KEY}'.");
                avatarName = "";
            }

            return avatarName;
        }

        /// <summary>
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Role in <see cref="Player.CustomProperties"/> or <see cref="Role.None"/>.</returns>
        public static Role GetRole(Player player)
        {
            Role role;
            if(player.CustomProperties.TryGetValue(ROLE_KEY, out object roleValue))
            {
                role = (Role)roleValue;
            }
            else
            {
                Debug.LogError($"No CustomProperty found for key: {nameof(ROLE_KEY)}, '{ROLE_KEY}'.");
                role = Role.None;
            }

            return role;
        }

        /// <summary>
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Product name in <see cref="Player.CustomProperties"/> or empty string.</returns>
        public static string GetActiveProduct(Player player)
        {
            string productId;
            if(player.CustomProperties.TryGetValue(PRODUCT_KEY, out object productValue))
            {
                productId = (string)productValue;
            }
            else
            {
                Debug.LogError($"No CustomProperty found for key: {nameof(PRODUCT_KEY)}, '{PRODUCT_KEY}'.");
                productId = "";
            }

            return productId;
        }

        // --- Others --------------------------------------------------------------------------------------------------
        /// <summary>
        /// Spawns its networked <see cref="GetLocalAvatar">avatar</see>, <see cref="AvatarLinkBehaviour.LinkRigToAvatar(XRRig)">links</see> the rig to the avatar 
        /// and <see cref="XRRig.TeleportRig(Vector3, Vector3)">teleports</see> the rig to the spawnpoint.
        /// </summary>
        /// <param name="spawnPoint"></param>
        public void SpawnPlayer(Transform spawnPoint)
        {
            //Destroy local avatar
            if(_avatarLink != null)
                Destroy(_avatarLink.gameObject);

            //spawn avatar via network
            GameObject avatar;
            if(_networkInfo is PlayerNetworkInfo_Photon)
            {
                avatar = PhotonNetwork.Instantiate(GetLocalAvatar(), spawnPoint.position, Quaternion.identity);
                LinkAvatarToRig(avatar);
            }

            //Teleport Rig
            Rig.TeleportRig(spawnPoint.position, spawnPoint.forward);
        }

        private void LinkAvatarToRig(GameObject avatar)
        {
            _avatarLink = avatar.GetComponent<AvatarLinkBehaviour>();
            _avatarLink.LinkRigToAvatar(Rig);
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _rig = FindObjectOfType<XRRig>();

            if(_avatarLink != null)
            {
                _avatarLink.LinkRigToAvatar(Rig);
            }
        }
    }
}
