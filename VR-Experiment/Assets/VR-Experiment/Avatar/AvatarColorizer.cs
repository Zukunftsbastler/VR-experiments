using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarColorizer : MonoBehaviour
{
    [SerializeField] private Renderer _head;
    [SerializeField] private Renderer _leftHand;
    [SerializeField] private Renderer _rightHand;

    void Start()
    {
        Photon.Pun.PhotonView pv = GetComponent<Photon.Pun.PhotonView>();

        if (pv == null)
        {
            Debug.LogError("No photonView found.");
            return;
        }

        //Color skinColor = PlayerSpawner.Instance.GetColorByActorNumber(pv.OwnerActorNr);
        Color skinColor = Resources.Load<SO_PlayerColor>("PlayerColor").GetPlayerColorByActorNumber(pv.OwnerActorNr);

        _head.material.color = skinColor;
        _leftHand.material.color = skinColor;
        _rightHand.material.color = skinColor;
    }
}
