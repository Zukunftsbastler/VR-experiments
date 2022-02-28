using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Avatar", menuName = "VR-Experiment/XR/Avatar", order = 0)]
public class SO_Avatar : ScriptableObject
{
    [SerializeField] private Sprite _preview;
    [SerializeField] private GameObject _asset;

    public Sprite Preview => _preview;
    public GameObject Asset => _asset;
}
