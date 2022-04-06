using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VR_Experiment.Core;
using VR_Experiment.Menu.UI.Core;

public class TourInformationUI : MonoBehaviour, IItemListCallbackListener
{
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private TourManager _tourManager;
    [Space]
    [SerializeField] private ScrollableItemListUI _selectionUI;
    [SerializeField] private GameObject _previewWrapper;
    [SerializeField] private Image _preview;
    [SerializeField] private Button _choose;
    [SerializeField] private Button _confirm;
    [Space]
    [SerializeField] private InputActionProperty _closeInput;

    private Sprite _defaultSprite;

    private void Start()
    {
        _choose.onClick.AddListener(OnChooseButtonClicked);
        _confirm.onClick.AddListener(OnConfirmButtonClicked);

        _selectionUI.SetCallbackListener(this);

        _defaultSprite = _preview.sprite;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _closeInput.action.performed += OnCloseInputPerformed;
    }

    private void OnDisable()
    {
        _closeInput.action.performed -= OnCloseInputPerformed;
    }

    void IItemListCallbackListener.OnItemToggleInvoked(bool isActive, string itemName)
    {
        //TODO: Notify TourManager about changes at the direction/hotspot (point of interest).
        _tourManager.SetPoIData(isActive, itemName);
        CloseUI();
    }

    public void ShowPointOfInterestData(PointOfInterestData data, List<ScriptableListItem> selection)
    {
        //activate ui
        gameObject.SetActive(true);

        //rotate canvas towards player
        Vector3 lookAtTargetPos = _lookAtTarget.position;
        lookAtTargetPos.y = transform.position.y;
        transform.LookAt(lookAtTargetPos);

        //set selection items
        _selectionUI.SetItems(selection);

        //configure Layout
        bool hasListItem = data.ListItem != null;
        if(hasListItem)
        {
            _preview.sprite = data.ListItem.Preview;
            _selectionUI.SetItem(data.ListItem.Name);
        }
        else
        {
            _preview.sprite = _defaultSprite;
        }

        _choose.interactable = PlayerWrapper.Instance.CanManagePointsOfInterest;
        _confirm.interactable = hasListItem ?
            PlayerWrapper.Instance.CanManagePointsOfInterest : false;

        _confirm.gameObject.SetActive(data is DirectionData);
    }

    private void OnChooseButtonClicked()
    {
        _previewWrapper.SetActive(false);
        _selectionUI.gameObject.SetActive(true);
    }

    private void OnConfirmButtonClicked()
    {
        _tourManager.ChangeDisplayScene();
        CloseUI();
    }

    private void CloseUI()
    {
        _previewWrapper.SetActive(true);
        _selectionUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnCloseInputPerformed(InputAction.CallbackContext context)
    {
        CloseUI();
    }
}
