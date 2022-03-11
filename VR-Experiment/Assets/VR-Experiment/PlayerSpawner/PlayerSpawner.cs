using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : SingletonBehaviour<PlayerSpawner>
{
    [SerializeField] private PhotonRoomInstatiation _photonRoom;
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    protected override void OnAwake()
    {
        base.OnAwake();
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        yield return _photonRoom.IsConnectedToPhoton;

        int globalSlot = PlayerWrapper.Instance.GetGlobalSlot();
        Transform spawnPoint = GetSpawnPointByActorNumber(globalSlot);

        PlayerWrapper.Instance.SpawnPlayer(spawnPoint);
    }

    private Transform GetSpawnPointByActorNumber(int actorNumber)
    {
        if(actorNumber < 0)
        {
            Debug.LogWarning($"Can't spawn player at spawnpoint: {actorNumber}.");
            return _spawnPoints.FirstOrDefault();
        }
        else
        {
            int index = actorNumber % _spawnPoints.Count;
            return _spawnPoints[index];
        }
    }
}
