using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PhotonRoomInstatiation _photonRoom;
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        yield return _photonRoom.IsConnectedToPhoton;

        int globalSlot = PlayerWrapper.Instance.GetGlobalSlot();
        Transform spawnPoint;

        if(globalSlot < 0)
        {
            Debug.LogWarning($"Can't spawn player at spawnpoint: {globalSlot}.");
            spawnPoint = _spawnPoints.FirstOrDefault();
        }
        else
        {
            int index = globalSlot % _spawnPoints.Count;
            spawnPoint = _spawnPoints[index];
        }

        PlayerWrapper.Instance.SpawnPlayer(spawnPoint);
    }
}
