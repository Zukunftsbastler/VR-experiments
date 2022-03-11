using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class ProductUI : MonoBehaviour
{
    [SerializeField] private Button _productButton;

    private string _productId;
    private Action<string> onButtonClicked;

    private void OnEnable()
    {
        _productButton.onClick.AddListener(OnProductButtonPressed);
    }

    private void OnDisable()
    {
        _productButton.onClick.RemoveListener(OnProductButtonPressed);
    }

    public void SetProduct(SO_Product product, Action<string> onProductUIInvoked)
    {
        _productButton.image.sprite = product.Preview;

        _productId = product.Id;
        onButtonClicked = onProductUIInvoked;
    }

    private void OnProductButtonPressed()
    {
        onButtonClicked.Invoke(_productId);
    }
}
