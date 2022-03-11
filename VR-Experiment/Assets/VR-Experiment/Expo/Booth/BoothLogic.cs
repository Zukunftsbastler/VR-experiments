using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoothLogic : MonoBehaviourPun
{
    [SerializeField] private List<RevolvingStageBehaviour> _productStages;

    private void Start()
    {
        SetStageInventories();
    }

    public void SetStageInventories()
    {
        foreach(RevolvingStageBehaviour stage in _productStages)
        {
            stage.SetInventory();
        }
    }
}
