using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Core;
using VR_Experiment.Menu.UI.JoinRoom;

namespace VR_Experiment.Networking
{
    public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private JoinRoomUI _joinRoomUI;
        [SerializeField] private Toggle _expoToggle;
        [SerializeField] private Toggle _threesixtyToggle;
        [Space]
        [SerializeField] private string _expoSceneName;
        [SerializeField] private string _threesixtySceneName;
        [SerializeField] private string _roomName;

        private string sceneName;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _joinRoomUI.joinRoomButtonClicked += JoinRoom;
            PlayerWrapper.Instance.onPropertiesChanged += LocalPlayerPropertiesChanged;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _joinRoomUI.joinRoomButtonClicked -= JoinRoom;
            PlayerWrapper.Instance.onPropertiesChanged -= LocalPlayerPropertiesChanged;
        }

        // --- Photon Feedback ------------------------------------------------------------------------------
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            _joinRoomUI.gameObject.SetActive(true);
            PlayerWrapper.Instance.SetNetworkInfo(new PlayerNetworkInfo_Photon(PhotonNetwork.LocalPlayer));
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            PhotonNetwork.LoadLevel(sceneName);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            Debug.LogWarning($"Joining Room Failed. Creating room: {_roomName}");

            PhotonNetwork.CreateRoom(_roomName);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);

            PhotonNetwork.JoinRoom(_roomName);
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// Join photon room.
        /// </summary>
        public void JoinRoom()
        {
            if(PhotonNetwork.IsConnectedAndReady == false)
                return;

            sceneName = _expoToggle.isOn ? _expoSceneName :
                _threesixtyToggle.isOn ? _threesixtySceneName : "";

            if(string.IsNullOrWhiteSpace(sceneName))
            {
                _joinRoomUI.SetDebugText("Please ensure to select a scenario.");
            }

            if(PlayerWrapper.Instance.HasAvatar && PlayerWrapper.Instance.HasRole)
            {
                _joinRoomUI.gameObject.SetActive(false);
                PhotonNetwork.JoinRoom(_roomName);
            }
        }

        private void LocalPlayerPropertiesChanged()
        {
            string debugText = "";

            if(PlayerWrapper.Instance.HasAvatar == false)
            {
                debugText += $"You need to choose an avatar befor you can join a room. \n";
            }

            if(PlayerWrapper.Instance.HasRole == false)
            {
                debugText += $"You need to choose a role befor you can join a room. \n";
            }

            _joinRoomUI.SetDebugText(debugText);
        }
    }
}
