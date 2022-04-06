using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using VR_Experiment.Core;
using VR_Experiment.Menu.UI.Core;

[Obsolete]
public class PointOfUI : MonoBehaviour, IItemListCallbackListener
{
    [SerializeField] private Transform _origin;
    [SerializeField] private float _distanceToOrigin = 1;
    [Space]
    [SerializeField] private HighlightManager _highlightManager;
    [SerializeField] private ScrollableItemListUI _pointOfInterestSelectionUI;
    [Space]
    [SerializeField] private Button _button;
    [SerializeField] private Text _buttonText;

    private Sprite _defaultSprite;
    private string _defaultText;

    public bool IsActive => gameObject.activeSelf;
    public bool IsSelectingPointOfInterest => _pointOfInterestSelectionUI.gameObject.activeSelf;

    private void Start()
    {
        Assert.IsNotNull(_highlightManager, $"{gameObject.name} - {typeof(HighlightManager)} is null. " +
            $"Please ensure this gameobject has a reference to a {typeof(HighlightManager)}.");
        Assert.IsNotNull(_pointOfInterestSelectionUI, $"{gameObject.name} - {typeof(ScrollableItemListUI)} is null. " +
            $"Please ensure this gameobject has a reference to a {typeof(ScrollableItemListUI)}.");

        _pointOfInterestSelectionUI.SetCallbackListener(this);
        _pointOfInterestSelectionUI.SetItems(_highlightManager.PointsOfInterest.Cast<ScriptableListItem>().ToList());

        _defaultSprite = _button.image.sprite;
        _defaultText = _buttonText.text;

        _button.onClick.AddListener(OpenSelectionUI);

        gameObject.SetActive(false);
    }

    void IItemListCallbackListener.OnItemToggleInvoked(bool isActive, string itemName)
    {
        _highlightManager.SetPointOfInterest(isActive, itemName);
        CloseSelectionUI();
    }

    public void ToggleUI(bool active)
    {
        gameObject.SetActive(active);

        if(IsActive)
        {
            RepositionUI();
            ResetUI();
        }
    }

    private void RepositionUI()
    {
        Vector3 position = _highlightManager.LocalActiveHighlight.transform.position;
        Vector3 direction = (position - _origin.position).normalized;

        transform.position = _origin.position + direction * _distanceToOrigin;
        transform.forward = direction;
    }

    private void CloseSelectionUI()
    {
        _pointOfInterestSelectionUI.gameObject.SetActive(false);
        _button.gameObject.SetActive(true);

        _highlightManager.OnSelectionUIChanged(false);
    }

    private void ResetUI()
    {
        _pointOfInterestSelectionUI.gameObject.SetActive(false);
        _button.gameObject.SetActive(true);
        UpdateUIButton();
    }

    public void UpdateUIButton()
    {
        if(_highlightManager.LocalActiveHighlight.HasPointOfInterest)
        {
            _button.image.sprite = _highlightManager.LocalActiveHighlight.PointOfInterest.Preview;
            _buttonText.text = "";
        }
        else
        {
            _button.image.sprite = _defaultSprite;
            _buttonText.text = _defaultText;
        }

        _button.interactable = PlayerWrapper.Instance.CanManagePointsOfInterest 
            ? _highlightManager.IsRemotelyBlocked(_highlightManager.LocalActiveHighlight) == false
            : false;
    }

    private void UpdateSelectionUI()
    {
        if(_highlightManager.LocalActiveHighlight.HasPointOfInterest)
        {
            _pointOfInterestSelectionUI.SetItem(_highlightManager.LocalActiveHighlight.PointOfInterest.Name);
        }
        else
        {
            _pointOfInterestSelectionUI.DeselectLastItem();
        }
    }

    private void OpenSelectionUI()
    {
        UpdateSelectionUI();
        _button.gameObject.SetActive(false);
        _pointOfInterestSelectionUI.gameObject.SetActive(true);

        _highlightManager.OnSelectionUIChanged(true);
    }
}
