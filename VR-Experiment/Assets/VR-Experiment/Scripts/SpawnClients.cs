using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnClients : MonoBehaviour
{
    [SerializeField] private GameObject _clientPrefab;


    void Start()
    {
        PhotonNetwork.Instantiate(_clientPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
