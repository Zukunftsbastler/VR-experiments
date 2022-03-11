using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR_Experiment.Expo.Booth
{
    public class BoothLogic : MonoBehaviourPun
    {
        [SerializeField] private List<RevolvingStageBehaviour> _productStages;

        private void Start()
        {
            SetStageInventories();
        }

        private void SetStageInventories()
        {
            foreach(RevolvingStageBehaviour stage in _productStages)
            {
                stage.SetInventory();
            }
        }
    }
}
