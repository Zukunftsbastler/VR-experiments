using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoothLogic : MonoBehaviourPun
{
    [SerializeField] private List<RevolvingStageBehaviour> _productStages;
    [Space]
    [SerializeField] private SO_ProductInventory _standartInventory;

    private void Start()
    {
        SetStageInventories(_standartInventory);
    }

    public void SetStageInventories(SO_ProductInventory inventory)
    {
        foreach(RevolvingStageBehaviour stage in _productStages)
        {
            stage.SetInventory(inventory);
        }
    }
}
