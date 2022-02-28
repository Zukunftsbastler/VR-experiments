using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _productUIPrefab;

    private IInventoryCallbackListener _callbackListener;
    private List<ProductUI> _productUIs;

    public void Initialize(IInventoryCallbackListener callbackListener)
    {
        _callbackListener = callbackListener;
        _productUIs = new List<ProductUI>();
    }

    public void SetInventory(SO_ProductInventory inventory)
    {
        ClearProductUIs();

        if(inventory == null)
            return;

        foreach(SO_Product product in inventory.Products)
        {
            ProductUI productUI = Instantiate(_productUIPrefab, transform).GetComponent<ProductUI>();
            productUI.SetProduct(product, OnProductUIInvoked);

            _productUIs.Add(productUI);
        }
    }

    public void OnProductUIInvoked(string productId)
    {
        _callbackListener.OnInventoryProductInvoked(productId);
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
