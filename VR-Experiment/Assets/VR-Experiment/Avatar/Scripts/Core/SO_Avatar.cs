using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;

namespace VR_Experiment.Avatar.Core
{
    [CreateAssetMenu(fileName = "SO_Avatar", menuName = "VR-Experiment/XR/Avatar", order = 0)]
    public class SO_Avatar : ScriptableListItem
    {
        [SerializeField] private GameObject _asset;

        public GameObject Asset => _asset;
    }
}
