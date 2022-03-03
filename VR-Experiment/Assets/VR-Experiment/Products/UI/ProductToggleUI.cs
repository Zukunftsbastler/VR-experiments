using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductToggleUI : MonoBehaviour
{
    [SerializeField] private Toggle _productToggle;
    [SerializeField] private Text _lable;

    private string _productId;
    private Action<bool, string> _onToggleValueChanged;

    private void OnEnable()
    {
        _productToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnDisable()
    {
        _productToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    public void SetProduct(SO_Product product, Action<bool, string> onProductToggleUIInvoked)
    {
        _productToggle.image.sprite = product.Preview;
        _lable.text = product.Id;
        _productToggle.group = GetComponentInParent<ToggleGroup>();

        _productId = product.Id;
        _onToggleValueChanged = onProductToggleUIInvoked;
    }

    private void OnToggleValueChanged(bool isActive)
    {
        _onToggleValueChanged.Invoke(isActive, _productId);
    }
}
