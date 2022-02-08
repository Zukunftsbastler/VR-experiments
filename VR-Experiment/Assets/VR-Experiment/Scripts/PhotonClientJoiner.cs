using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotonClientJoiner : MonoBehaviourPun
{
    [SerializeField] private XROrigin _xrOrigin;

    [SerializeField] private List<Behaviour> _xrComponents = new List<Behaviour>();

    void Start()
    {
        if(photonView.IsMine)
        {
            LocomotionSystem locomotionSystem = FindObjectOfType<LocomotionSystem>();

            if(locomotionSystem != null)
                locomotionSystem.xrOrigin = _xrOrigin;

            foreach(Behaviour xrComponent in _xrComponents)
            {
                xrComponent.enabled = true;
            }
        }
        else
        {
            foreach(Behaviour xrComponent in _xrComponents)
            {
                xrComponent.enabled = false;
            }
        }
    }
}
