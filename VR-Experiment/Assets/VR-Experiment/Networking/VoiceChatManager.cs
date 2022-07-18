using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Avatar;
using VR_Experiment.Core;

namespace VR_Experiment.Networking
{
    public class VoiceChatManager : SingletonBehaviour<VoiceChatManager>, IInRoomCallbacks
    {
        [SerializeField] private PhotonView _photonView;
        private Dictionary<int, AvatarSoundSettings> _clients;

        protected override void OnAwake()
        {
            base.OnAwake();
            _clients = new Dictionary<int, AvatarSoundSettings>();
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void Register(Player player, AvatarSoundSettings avatarSettings)
        {
            _clients.Add(player.ActorNumber, avatarSettings);
        }

        public void Unregister(Player player)
        {
            _clients.Remove(player.ActorNumber);
        }

        public AvatarSoundSettings GetAvatarSettings(Player player)
        {
            if(_clients.TryGetValue(player.ActorNumber, out AvatarSoundSettings settings))
            {
                return settings;
            }
            else
            {
                Debug.LogWarning($"Trying to {nameof(GetAvatarSettings)} of unregistered player '{player.ActorNumber}' - NO SETTINGS FOUND!");
                return null;
            }
        }

        public void SyncVoiceSettings(Player player, VoiceChatSettings settings)
        {
            _photonView.RPC(nameof(RPC_SyncVoiceSettings), RpcTarget.OthersBuffered, player.ActorNumber, settings);
        }

        [PunRPC]
        private void RPC_SyncVoiceSettings(int actorNumber, VoiceChatSettings settings)
        {
            _clients[actorNumber].SetSettings(settings);
        }

        void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer) 
        {
            //if(PhotonNetwork.IsMasterClient)
            //{
            //    foreach(var client in _clients)
            //    {
            //        _photonView.RPC(nameof(RPC_SyncVoiceSettings), newPlayer, client.Key, client.Value.GetSettings());
            //    }
            //}
        }

        void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer) { }

        void IInRoomCallbacks.OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }

        void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) { }

        void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient) { }
    }
}
