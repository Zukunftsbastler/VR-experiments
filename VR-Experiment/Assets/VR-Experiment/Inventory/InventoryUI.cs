using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _productUIPrefab;

    private IItemListCallbackListener _callbackListener;
    private List<ProductUI> _productUIs;

    public void Initialize(IItemListCallbackListener callbackListener)
    {
        _callbackListener = callbackListener;
        _productUIs = new List<ProductUI>();
    }

    public void SetInventory()
    {
        ClearProductUIs();

        foreach(SO_Product product in Inventory.Products)
        {
            ProductUI productUI = Instantiate(_productUIPrefab, transform).GetComponent<ProductUI>();
            productUI.SetProduct(product, OnProductUIInvoked);

            _productUIs.Add(productUI);
        }
    }

    public void OnProductUIInvoked(string productId)
    {
        _callbackListener.OnItemToggleInvoked(true, productId);
    }

    private void ClearProductUIs()
    {
        if(_productUIs.Count == 0)
            return;

        foreach(ProductUI productUI in _productUIs.ToArray())
        {
            _productUIs.Remove(productUI);
            Destroy(productUI.gameObject);
        }
    }
}
