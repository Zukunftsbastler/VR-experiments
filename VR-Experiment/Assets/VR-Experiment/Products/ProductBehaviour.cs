using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBehaviour : NetworkedGrabInteractable, IPunInstantiateMagicCallback
{
    public TransformFollow transformFollow;
    [Space]
    [SerializeField] private float _destroyDelay = 5f;
    [Space]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private MeshCollider _meshCollider;

    public event Action<string> productGrabbed;

    private SO_Product _info;
    private Coroutine _destroyRoutine;

    private Coroutine _setProductDelayed;
    private string _productID;

    protected override void Awake()
    {
        base.Awake();
    }

    [PunRPC]
    protected override void RPC_StartNetworkGrabbing()
    {
        base.RPC_StartNetworkGrabbing();

        //All clients
        transformFollow.enabled = false;
        //_rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        productGrabbed?.Invoke(_info.Name);
        //Only for owner
        if (photonView.Owner.Equals(PhotonNetwork.LocalPlayer))
        {
            if (_destroyRoutine != null)
                StopCoroutine(_destroyRoutine);

            _meshFilter.mesh = _info.HighPoly;

            //setting PlayerWrappers ActiveProduct should only be called once a frame.
            _productID = _info.Name;
            if (_setProductDelayed == null)
                _setProductDelayed = StartCoroutine(SetProductDelayed());
        }
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

        //All clients
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        //Only for owner
        if (photonView.Owner.Equals(PhotonNetwork.LocalPlayer))
        {
            _destroyRoutine = StartCoroutine(DestroyDelayed());

            _meshFilter.mesh = _info.LowPoly;

            //setting the PlayerWrappers ActiveProduct should only be called once a frame.
            _productID = "";
            if (_setProductDelayed == null)
                _setProductDelayed = StartCoroutine(SetProductDelayed());
        }
    }

    private IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(_destroyDelay);

        PhotonNetwork.Destroy(photonView);
    }

    private IEnumerator SetProductDelayed()
    {
        yield return new WaitForEndOfFrame();

        PlayerWrapper.Instance.SetActiveProduct(_productID);
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        string productName = (string)info.photonView.InstantiationData[0];
        _info = Inventory.GetProductByName(productName);

        _meshFilter.mesh = _info.LowPoly;
        _meshRenderer.materials = _info.Materials;
        //_meshCollider.sharedMesh = _meshFilter.mesh;
    }
}
