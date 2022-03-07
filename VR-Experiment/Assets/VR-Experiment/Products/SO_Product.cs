using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Product", menuName = "VR-Experiment/Messestand/Product", order = 1)]
public class SO_Product : ScriptableListItem
{
    [SerializeField] private string _info;
    [SerializeField] private GameObject _asset;
    [SerializeField] private Mesh _lowPoly;
    [SerializeField] private Mesh _highPoly;
    [SerializeField] private Material[] _materials;

    public string Id => _asset.name;
    public string Info => _info;
    public GameObject Asset => _asset;
    public Mesh LowPoly => _lowPoly;
    public Mesh HighPoly => _highPoly;
    public Material[] Materials => _materials;
}
