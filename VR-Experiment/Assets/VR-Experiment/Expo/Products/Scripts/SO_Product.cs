using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;

namespace VR_Experiment.Expo.Product
{
    [CreateAssetMenu(fileName = "SO_Product", menuName = "VR-Experiment/Expo/Product", order = 1)]
    public class SO_Product : ScriptableListItem
    {
        [SerializeField, TextArea(2, 5)] private List<string> _info;
        [SerializeField] private GameObject _asset;
        [SerializeField] private Mesh _lowPoly;
        [SerializeField] private Mesh _highPoly;
        [SerializeField] private Material[] _materials;

        public string Id => _asset.name;
        public List<string> Info => _info;
        public GameObject Asset => _asset;
        public Mesh LowPoly => _lowPoly;
        public Mesh HighPoly => _highPoly;
        public Material[] Materials => _materials;
    }
}