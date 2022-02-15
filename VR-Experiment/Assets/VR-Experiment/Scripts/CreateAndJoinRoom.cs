using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VR_Experiment.Enums;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public void SetVisitor(bool isActive)
    {
        Debug.Log($"SetVisitor: {isActive}");
    }

    public void SetPresenter(bool isActive)
    {
        Debug.Log($"SetPresenter: {isActive}");
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
}
