using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Product", menuName = "VR-Experiment/Messestand/Product", order = 1)]
public class SO_Product : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _info;
    [SerializeField] private Sprite _preview;
    [SerializeField] private GameObject _asset;

    public string Id => _id;
    public string Info => _info;
    public Sprite Preview => _preview;
    public GameObject Asset => _asset;
}
