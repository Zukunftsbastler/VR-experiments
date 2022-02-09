using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _avatarPrefab;
    [Space]
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;


    void Start()
    {
        GameObject avatar = PhotonNetwork.Instantiate(_avatarPrefab.name, Vector3.zero, Quaternion.identity);
        AvatarLinkBehaviour avatarLink = avatar.GetComponent<AvatarLinkBehaviour>();
        avatarLink.LinkRigToAvatar(_head, _leftHand, _rightHand);
    }
}
