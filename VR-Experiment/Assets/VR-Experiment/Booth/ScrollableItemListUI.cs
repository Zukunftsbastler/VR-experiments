using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollableItemListUI : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _groupItemToggleUIPrefab;

    private IItemListCallbackListener _callbackListener;
    private List<GroupItemToggleUI> _groupItemToggleUIs = new List<GroupItemToggleUI>();

    private GroupItemToggleUI _lastActiveItem;

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
        GroupItemToggleUI toggleUI = _groupItemToggleUIs.FirstOrDefault(i => i.ItemName.Equals(itemName));
        SetItem(toggleUI, isActive, withoutNotify);
    }

    public void SetItem(GroupItemToggleUI item, bool isActive = true, bool withoutNotify = true)
    {
        if(withoutNotify)
        {
            item.Toggle.SetIsOnWithoutNotify(isActive);
        }
        else
        {
            item.Toggle.isOn = isActive;
        }
    }

    private void OnItemToggleUIInvoked(bool isActive, GroupItemToggleUI activeItem)
    {
        if(_lastActiveItem != null)
            SetItem(_lastActiveItem, false);

        _lastActiveItem = isActive ? activeItem : null;

        _callbackListener.OnItemToggleInvoked(isActive, activeItem.ItemName);
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
