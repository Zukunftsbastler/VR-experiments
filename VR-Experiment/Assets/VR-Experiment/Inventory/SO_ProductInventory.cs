using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Inventory", menuName = "VR-Experiment/Messestand/Inventory", order = 0)]
public class SO_ProductInventory : ScriptableObject
{
    [SerializeField] private string _inventoryName;
    [SerializeField] private List<SO_Product> _products;

    public List<SO_Product> Products => _products;
}
