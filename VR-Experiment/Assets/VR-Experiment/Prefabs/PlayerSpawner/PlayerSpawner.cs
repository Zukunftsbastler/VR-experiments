using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : SingletonBehaviour<PlayerSpawner>
{
    [SerializeField] private PhotonRoomInstatiation _photonRoom;
    [SerializeField] private List<PlayerSpawnPoint> _spawnPoints = new List<PlayerSpawnPoint>();

    protected override void OnAwake()
    {
        base.OnAwake();
        StartCoroutine(SpawnPlayer());
    }

    public Color GetColorByActorNumber(int actorNumber)
    {
        return GetSpawnPointByActorNumber(actorNumber).Color;
    }

    private IEnumerator SpawnPlayer()
    {
        yield return _photonRoom.IsConnectedToPhoton;

        int globalSlot = PlayerWrapper.Instance.GetGlobalSlot();
        PlayerSpawnPoint spawnPoint = GetSpawnPointByActorNumber(globalSlot);

        PlayerWrapper.Instance.SpawnPlayer(spawnPoint);
    }

    private PlayerSpawnPoint GetSpawnPointByActorNumber(int actorNumber)
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
