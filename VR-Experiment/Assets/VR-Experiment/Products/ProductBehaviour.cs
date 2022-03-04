using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBehaviour : NetworkedGrabInteractable, IPunInstantiateMagicCallback
{
    public TransformFollow transfromFollow;
    [Space]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private Rigidbody _rigidbody;

    public event Action<string> productGrabbed;

    private SO_Product _info;

    protected override void Awake()
    {
        base.Awake();
    }

    [PunRPC]
    protected override void RPC_StartNetworkGrabbing()
    {
        base.RPC_StartNetworkGrabbing();

        productGrabbed?.Invoke(_info.Name);
        _meshFilter.mesh = _info.HighPoly;
        transfromFollow.enabled = false;
        PlayerWrapper.Instance.SetActiveProduct(_info.Name);
    }

    [PunRPC]
    protected override void RPC_OnNetworkGrabFailed()
    {
        base.RPC_OnNetworkGrabFailed();
    }

    [PunRPC]
    protected override void RPC_StopNetworkGrabbing()
    {
        base.RPC_StopNetworkGrabbing();

        _meshFilter.mesh = _info.LowPoly;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        //todo: start destroy delay
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        string productName = (string) info.photonView.InstantiationData[0];
        _info = Inventory.GetProductByName(productName);

        _meshFilter.mesh = _info.LowPoly;
        _meshRenderer.materials = _info.Materials;
        //_meshCollider.sharedMesh = _meshFilter.mesh;
    }
}
