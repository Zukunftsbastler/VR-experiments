using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VR_Experiment.Core;

namespace VR_Experiment.Networking
{
    public class PhotonRoomInstatiation : SingletonBehaviour<PhotonRoomInstatiation>, IConnectionCallbacks, IMatchmakingCallbacks
    {
        [SerializeField] private GameObject _standartAvatar;
        [SerializeField] private Role _standartRole;

        private bool _isConnected = false;

        public WaitUntil IsConnectedToPhoton => new WaitUntil(IsConnected);

        void Start()
        {
            if(PhotonNetwork.IsConnectedAndReady)
            {
                _isConnected = true;
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings, true);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void ExitExperiment()
        {
            if(_isConnected)
            { 
                PhotonNetwork.Disconnect();
            }
            else
            {
                SceneManager.LoadScene("Join Experiment", LoadSceneMode.Single);
            }
        }

        void IConnectionCallbacks.OnConnected() { }

        void IConnectionCallbacks.OnConnectedToMaster()
        {
            Debug.Log("Connected to Mastert");

            if(PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.CreateRoom("Offline VR-Experiment");
            }
        }

        void IConnectionCallbacks.OnDisconnected(DisconnectCause cause) 
        {
            SceneManager.LoadScene("Join Experiment", LoadSceneMode.Single);
        }

        void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler) { }

        void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

        void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage) { }

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList) { }

        void IMatchmakingCallbacks.OnCreatedRoom()
        {
            Debug.Log("Created Room");

            PlayerWrapper.Instance.SetPlayerData(new PlayerNetworkInfo_Photon(PhotonNetwork.LocalPlayer), _standartAvatar, _standartRole);

            _isConnected = true;
        }

        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create a Room");
        }

        void IMatchmakingCallbacks.OnJoinedRoom() { }

        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message) { }

        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) { }

        void IMatchmakingCallbacks.OnLeftRoom() { }

        private bool IsConnected() => _isConnected;
    }
}
