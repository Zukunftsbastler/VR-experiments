using Photon.Pun;
using UnityEngine;
using VR_Experiment.Core;

namespace VR_Experiment.Networking
{
    public class PhotonRoomInstatiation : MonoBehaviourPunCallbacks
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

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected to Mastert");

            if(PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.CreateRoom("Offline VR-Experiment");
            }
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Debug.Log("Created Room");

            PlayerWrapper.Instance.SetPlayerData(new PlayerNetworkInfo_Photon(PhotonNetwork.LocalPlayer), _standartAvatar, _standartRole);

            _isConnected = true;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log("Failed to create a Room");
        }

        private bool IsConnected() => _isConnected;
    }
}
