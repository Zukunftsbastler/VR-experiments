using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] _avatarPrefabs = new GameObject[2];

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

    public void ConnectToPhoton()
    {
        if(PlayerWrapper.Instance.CanConnectToPhoton)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogWarning($"You need to choose an avatar befor you can connect to photon.");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PlayerWrapper.Instance.SetNetworkInfo(new PlayerNetworkInfo_Photon(PhotonNetwork.LocalPlayer));
        SceneManager.LoadScene("JoinRoom_Robin");
    }
}
