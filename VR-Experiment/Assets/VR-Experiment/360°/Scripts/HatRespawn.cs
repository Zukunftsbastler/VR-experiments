using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRespawn : MonoBehaviour
{
    [SerializeField] private Collider _hatCollider;
    [SerializeField] private PhotonView _hatPhotonView;
    [SerializeField] private Rigidbody _hatRigidbody;
    [Space]
    [SerializeField] private Transform _respawnAnchor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.Equals(_hatCollider))
        {
            _hatRigidbody.velocity = Vector3.zero;
            _hatCollider.transform.SetPositionAndRotation(_respawnAnchor.position, _respawnAnchor.rotation);
        }
    }
}
