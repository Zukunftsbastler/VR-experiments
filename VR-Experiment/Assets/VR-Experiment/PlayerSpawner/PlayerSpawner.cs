using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Core;
using VR_Experiment.Networking;

namespace VR_Experiment.Core
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

        protected void Start()
        {
            StartCoroutine(SpawnPlayer());
        }

        private IEnumerator SpawnPlayer()
        {
            yield return PhotonRoomInstatiation.Instance.IsConnectedToPhoton;

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
}
