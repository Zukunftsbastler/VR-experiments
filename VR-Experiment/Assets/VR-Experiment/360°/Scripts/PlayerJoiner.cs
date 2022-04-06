using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Avatar.Expo;
using VR_Experiment.Core;
using VR_Experiment.Networking;

public class PlayerJoiner : MonoBehaviourPun, IInRoomCallbacks
{
    [SerializeField] private List<Seat> _spawnPoints = new List<Seat>();

    void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private IEnumerator SpawnPlayer()
    {
        yield return PhotonRoomInstatiation.Instance.IsConnectedToPhoton;

        if(PhotonNetwork.IsMasterClient)
        {
            Seat spawnPoint = _spawnPoints.FirstOrDefault(s => s.IsOccupied == false);
            spawnPoint.AddPlayer(PlayerWrapper.Instance.Player);
            PlayerWrapper.Instance.SpawnPlayer(spawnPoint.transform);
        }
    }

    [PunRPC]
    private void RPC_RecieveSeatFromMaster(int seatIndex)
    {
        Seat spawnPoint = _spawnPoints[seatIndex];
        PlayerWrapper.Instance.SpawnPlayer(spawnPoint.transform);
        photonView.RPC(nameof(RPC_OccupieSeat), RpcTarget.MasterClient, seatIndex);
    }

    [PunRPC]
    private void RPC_OccupieSeat(int seatIndex, PhotonMessageInfo info)
    {
        _spawnPoints[seatIndex].AddPlayer(info.Sender);
    }

    public void OnMasterClientSwitched(Player newMasterClient) { }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Seat spawnPoint = _spawnPoints.FirstOrDefault(s => s.IsOccupied == false);
            int seatIndex = _spawnPoints.IndexOf(spawnPoint);
            photonView.RPC(nameof(RPC_RecieveSeatFromMaster), newPlayer, seatIndex);
        }
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _spawnPoints.FirstOrDefault(s => s.Player.Equals(otherPlayer)).RemovePlayer();
        }
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) { }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }
}
