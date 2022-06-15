using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PointOfInterest : MonoBehaviour
{
    private const string NO_DATA_SELECTED = "No Data selected";

    [Serializable]
    public class Style
    {
        public Sprite sprite;
        public Color color;
        public Vector3 scale;

        public void ApplyStyle(Image image)
        {
            image.sprite = sprite;
            image.color = color;
            image.rectTransform.localScale = scale;
        }
    }

    [SerializeField] private Canvas _canvas;
    [Space]
    [SerializeField] private Image _image;
    [Header("Styles")]
    [SerializeField] private Style _defaultStyle;
    [Space]
    [SerializeField] private Style _hoverStyle;
    [SerializeField] private TextMeshProUGUI _hoverText;
    [Space]
    [SerializeField] private Style _clickStyle;
    [Space]
    [SerializeField] private Style _errorStlye;

    [Header("Interactions")]
    [SerializeField] private InputActionProperty _edit;
    [SerializeField] private InputActionProperty _delete;

    private PointOfInterestData _data;
    private bool _isHoverActive = false;

    public PointOfInterestData Data
    {
        get
        {
            return _data;
        }

        set
        {
            _data = value;

            if(Data.ListItem != null)
            {
                _hoverText.text = Data.ListItem.Name;
            }
            else
            {
                _hoverText.text = NO_DATA_SELECTED;
            }
        }
    }

    public TourManager TourManager { get; set; }

    private void Awake()
    {
        ApplyDefaultSettings();
        _canvas.worldCamera = Camera.main;
    }

    private void ApplyDefaultSettings()
    {
        _defaultStyle.ApplyStyle(_image);
        _hoverText.enabled = _isHoverActive = false;
    }

    private void OnEnable()
    {
        _edit.action.performed += OnEditActionPerformed;
        _delete.action.performed += OnDeleteActionPerformed;
    }

    private void OnDisable()
    {
        _edit.action.performed -= OnEditActionPerformed;
        _delete.action.performed -= OnDeleteActionPerformed;
    }

    public void OnEventEnter()
    {
        _hoverStyle.ApplyStyle(_image);
        _hoverText.enabled = _isHoverActive = true;
    }

    public void OnEventExit()
    {
        ApplyDefaultSettings();
    }

    public void OnEventDown()
    {
        if(Data.ListItem != null)
        {
            _clickStyle.ApplyStyle(_image);
        }
        else
        {
            _errorStlye.ApplyStyle(_image);
        }
    }

    public void OnEventUp()
    {
        if(Data.ListItem != null)
        {
            ApplyDefaultSettings();
            TourManager.ChangeDisplayScene(Data);
        }
        else
        {
            _hoverStyle.ApplyStyle(_image);
        }
    }

    private void OnDeleteActionPerformed(InputAction.CallbackContext obj)
    {
        if(_isHoverActive == false)
            return;

        TourManager.DestroyPointOfInterest(Data);
    }

    private void OnEditActionPerformed(InputAction.CallbackContext obj)
    {
        if(_isHoverActive == false)
            return;

        TourManager.DisplayPointOfInteresstData(Data);
    }
}
