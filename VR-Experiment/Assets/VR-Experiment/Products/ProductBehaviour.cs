using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBehaviour : NetworkedGrabInteractable
{
    public TransformFollow transfromFollow;
    public SO_Product info;
    [Space]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private Rigidbody _rigidbody;

    public event Action<string> productGrabbed;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _meshFilter.mesh = info.LowPoly;
        _meshRenderer.materials = info.Materials;
        _meshCollider.sharedMesh = _meshFilter.mesh;
    }

    [PunRPC]
    protected override void RPC_StartNetworkGrabbing()
    {
        base.RPC_StartNetworkGrabbing();

        productGrabbed?.Invoke(info.Name);
        _meshFilter.mesh = info.HighPoly;
        transfromFollow.enabled = false;
        PlayerWrapper.Instance.SetActiveProduct(info.Name);
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

        _meshFilter.mesh = info.LowPoly;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        //todo: start destroy delay
    }
}
