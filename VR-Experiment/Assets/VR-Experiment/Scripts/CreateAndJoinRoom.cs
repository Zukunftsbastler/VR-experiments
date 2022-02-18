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
        Role role = isActive ? Role.Visitor : Role.None;
        PlayerWrapper.Instance.SetRole(role);
    }

    public void SetPresenter(bool isActive)
    {
        Role role = isActive ? Role.Presenter : Role.None;
        PlayerWrapper.Instance.SetRole(role);
    }

    public void JoinRoom()
    {
        if(PlayerWrapper.Instance.CanConnectToRoom)
        {
            PhotonNetwork.JoinRoom("VR-Experiment");
        }
        else
        {
            Debug.LogWarning($"You need to choose a role befor you can join a room.");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Testscene_Messe");
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
