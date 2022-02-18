using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "VR-Experiment/Messestand/product", order = 0)]
public class ProductSO : ScriptableObject
{
    [SerializeField] private string _productName;
    [SerializeField] private string _productString;
    [SerializeField] private Texture2D _productPreview;
    [SerializeField] private GameObject _productAsset;

    public GameObject Spawn() => Instantiate(_productAsset);
}
