using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VR_Experiment.Enums;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _uiConsole;
    [SerializeField] private GameObject[] _avatarPrefabs = new GameObject[2];

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
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

        PlayerWrapper.Instance.SetNetworkInfo(new PlayerNetworkInfo_Photon(PhotonNetwork.LocalPlayer));
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Expo_Gil");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.LogWarning($"Joining Room Failed. Creating room: VR-Experiment");

        PhotonNetwork.CreateRoom("VR-Experiment");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        PhotonNetwork.JoinRoom("VR-Experiment");
    }

    // --- UI Feedback ----------------------------------------------------------------------------------

    public void SetAvatarOne(bool isActive)
    {
        GameObject prefab = isActive ? _avatarPrefabs[0] : null;
        PlayerWrapper.Instance.SetAvatar(prefab, true);
    }

    public void SetAvatarTwo(bool isActive)
    {
        GameObject prefab = isActive ? _avatarPrefabs[1] : null;
        PlayerWrapper.Instance.SetAvatar(prefab, true);
    }

    // --------------------------------------------------------------------

    public void SetVisitor(bool isActive)
    {
        Role role = isActive ? Role.Visitor : Role.None;
        PlayerWrapper.Instance.SetRole(role);
    }

    public void SetPresenter(bool isActive)
    {
        Role role = isActive ? Role.Presenter : Role.None;
        PlayerWrapper.Instance.SetRole(role);
    }

    // --------------------------------------------------------------------

    public void JoinRoom()
    {
        if(PhotonNetwork.IsConnected == false)
            return;

        if(PlayerWrapper.Instance.CanConnectToPhoton == false)
        {
            _uiConsole.text = $"You need to choose an avatar befor you can join a room.";
            Debug.LogWarning($"You need to choose an avatar befor you can join a room.");
            return;
        }

        if(PlayerWrapper.Instance.CanConnectToRoom == false)
        {
            _uiConsole.text = $"You need to choose a role befor you can join a room.";
            Debug.LogWarning($"You need to choose a role befor you can join a room.");
            return;
        }
        
        PhotonNetwork.JoinRoom("VR-Experiment");
    }
}
