using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoothColumnUI : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _productToggleUIPrefab;

    private IInventoryCallbackListener _callbackListener;
    private List<ProductToggleUI> _productToggleUIs;

    private void Awake()
    {
        _productToggleUIs = new List<ProductToggleUI>();
    }

    public void SetCallbackListener(IInventoryCallbackListener callbackListener)
    {
        _callbackListener = callbackListener;
    }

    public void SetInventory(SO_ProductInventory inventory)
    {
        ClearProductUIs();

        if(inventory == null)
            return;

        foreach(SO_Product product in inventory.Products)
        {
            ProductToggleUI productUI = Instantiate(_productToggleUIPrefab, _container).GetComponent<ProductToggleUI>();
            productUI.SetProduct(product, OnProductToggleUIInvoked);

            _productToggleUIs.Add(productUI);
        }
    }

    public void OnProductToggleUIInvoked(bool isActive, string productId)
    {
        _callbackListener.OnInventoryProductInvoked(isActive, productId);
    }

    private void ClearProductUIs()
    {
        if(_productToggleUIs.Count == 0)
            return;

        foreach(ProductToggleUI productUI in _productToggleUIs.ToArray())
        {
            _productToggleUIs.Remove(productUI);
            Destroy(productUI.gameObject);
        }
    }
}