using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "VR-Experiment/Messestand/Inventory", order = 0)]
public class ProductInventory : MonoBehaviour
{
    [SerializeField] private string _inventoryName;
    [SerializeField] private List<ProductSO> _products;

    public List<ProductSO> GetProducts => _products;
}
