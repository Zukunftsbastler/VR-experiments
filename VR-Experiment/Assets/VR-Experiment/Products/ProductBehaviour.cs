using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBehaviour : MonoBehaviour
{
    [SerializeField] private SO_Product _info;

    public void GrabedByPlayer(bool isGrabbed)
    {
        string productId = isGrabbed ? _info.Id : null;
        PlayerWrapper.Instance.SetActiveProduct(productId);
    }
}
