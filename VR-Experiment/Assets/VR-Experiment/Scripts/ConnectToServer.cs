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
        GameObject prefab = isActive ? _avatarPrefabs[1] : null;
        PlayerWrapper.Instance.SetAvatar(prefab);
    }

    public void SetAvatarTwo(bool isActive)
    {
        GameObject prefab = isActive ? _avatarPrefabs[2] : null;
        PlayerWrapper.Instance.SetAvatar(prefab);
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        SceneManager.LoadScene("JoinRoom_Robin");
    }
}
