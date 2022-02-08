using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("VR-Experiment");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("VR-Experiment");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Testscene_Robin");
    }
}
