using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBehaviour : MonoBehaviour
{
    public TransformFollow transfromFollow;
    [Space]
    [SerializeField] private SO_Product _info;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Rigidbody _rigidbody;

    public event Action<string> productGrabbed;

    private void Start()
    {
        _meshFilter.mesh = _info.LowPoly;
    }

    public void GrabedByPlayer(bool isGrabbed)
    {
        string productId;

        if(isGrabbed)
        {
            productGrabbed?.Invoke(_info.Id);

            productId = _info.Id;
            _meshFilter.mesh = _info.HeightPoly;

            transfromFollow.enabled = false;
        }
        else
        {
            productId = null;
            _meshFilter.mesh = _info.LowPoly;

            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
        }

        PlayerWrapper.Instance.SetActiveProduct(productId);
    }
}
