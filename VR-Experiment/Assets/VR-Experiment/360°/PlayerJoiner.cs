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

    private IEnumerator SpawnPlayer()
    {
        yield return PhotonRoomInstatiation.Instance.IsConnectedToPhoton;

        Seat spawnPoint = _spawnPoints.FirstOrDefault(s => s.IsOccupied == false);

        int globalSlot = PlayerWrapper.Instance.GetGlobalSlot();
        int seatIndex = _spawnPoints.IndexOf(spawnPoint);

        photonView.RPC(nameof(RPC_OccupieSeat), RpcTarget.AllBuffered, globalSlot, seatIndex);

        PlayerWrapper.Instance.SpawnPlayer(spawnPoint.transform);
    }

    [PunRPC]
    private void RPC_OccupieSeat(int playerActorNr, int seatIndex)
    {
        Player player = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == playerActorNr);
        _spawnPoints[seatIndex].AddPlayer(player);
    }

    public void OnMasterClientSwitched(Player newMasterClient) { }

    public void OnPlayerEnteredRoom(Player newPlayer) { }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        _spawnPoints.FirstOrDefault(s => s.Player.Equals(otherPlayer)).RemovePlayer();
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) { }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }
}
