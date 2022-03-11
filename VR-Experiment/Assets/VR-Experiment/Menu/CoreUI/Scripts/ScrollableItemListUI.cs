using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableItemListUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.Transform _container;
    [SerializeField] private GameObject _groupItemToggleUIPrefab;

    private IItemListCallbackListener _callbackListener;
    private List<GroupItemToggleUI> _groupItemToggleUIs = new List<GroupItemToggleUI>();

    private GroupItemToggleUI _lastActiveItem;

    private bool HasLastActivItem => _lastActiveItem != null;

    public void SetCallbackListener(IItemListCallbackListener callbackListener)
    {
        _callbackListener = callbackListener;
    }

    public void SetItems(List<ScriptableListItem> items)
    {
        ClearProductUIs();

        foreach(ScriptableListItem item in items)
        {
            GroupItemToggleUI toggleUI = Instantiate(_groupItemToggleUIPrefab, _container).GetComponent<GroupItemToggleUI>();
            toggleUI.SetItem(item.Name, OnItemToggleUIInvoked, item.Preview);

            _groupItemToggleUIs.Add(toggleUI);
        }
    }

    public void SetItems(List<string> items)
    {
        ClearProductUIs();

        foreach(string item in items)
        {
            GroupItemToggleUI toggleUI = Instantiate(_groupItemToggleUIPrefab, _container).GetComponent<GroupItemToggleUI>();
            toggleUI.SetItem(item, OnItemToggleUIInvoked);

            _groupItemToggleUIs.Add(toggleUI);
        }
    }

    public void SetItem(string itemName, bool isActive = true, bool withoutNotify = true)
    {
        GroupItemToggleUI activeItem = _groupItemToggleUIs.FirstOrDefault(i => i.ItemName.Equals(itemName));
        SetItem(activeItem, isActive, withoutNotify);
    }

    public void SetItem(GroupItemToggleUI activeItem, bool isActive = true, bool withoutNotify = true)
    {
        UpdateLastActiveItem(activeItem, isActive);

        if(withoutNotify)
        {
            activeItem.Toggle.SetIsOnWithoutNotify(isActive);
        }
        else
        {
            activeItem.Toggle.isOn = isActive;
        }
    }

    private void OnItemToggleUIInvoked(bool isActive, GroupItemToggleUI activeItem)
    {
        UpdateLastActiveItem(activeItem, isActive);
        _callbackListener?.OnItemToggleInvoked(isActive, activeItem.ItemName);
    }

    private void UpdateLastActiveItem(GroupItemToggleUI newActiveItem, bool isActive)
    {
        if(HasLastActivItem && _lastActiveItem.Equals(newActiveItem) == false)
            SetItem(_lastActiveItem, false);

        _lastActiveItem = isActive ? newActiveItem : null;
    }

    private void ClearProductUIs()
    {
        if(_groupItemToggleUIs.Count == 0)
            return;

        foreach(GroupItemToggleUI toggleUI in _groupItemToggleUIs.ToArray())
        {
            _groupItemToggleUIs.Remove(toggleUI);
            Destroy(toggleUI.gameObject);
        }
    }
}
