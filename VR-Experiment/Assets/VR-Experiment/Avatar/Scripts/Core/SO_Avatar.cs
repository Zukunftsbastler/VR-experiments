using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Avatar", menuName = "VR-Experiment/XR/Avatar", order = 0)]
public class SO_Avatar : ScriptableListItem
{
    [SerializeField] private GameObject _asset;

    public GameObject Asset => _asset;
}
