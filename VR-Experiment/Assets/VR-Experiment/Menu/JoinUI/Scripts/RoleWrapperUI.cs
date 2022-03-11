using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Enums;

public class RoleWrapperUI : MonoBehaviour, IItemListCallbackListener
{
    [SerializeField] private ScrollableItemListUI _roleSelectionUI;

    private Dictionary<string, Role> _roles;

    public ScrollableItemListUI SelectionUI => _roleSelectionUI;

    private void Awake()
    {
        _roles = new Dictionary<string, Role>();

        foreach(byte roleByte in Enum.GetValues(typeof(Role)).Cast<byte>().OrderBy(b => b).ToList())
        {
            Role role = (Role)roleByte;
            string roleName = role.ToString();

            _roles.Add(roleName, role);
        }

        _roleSelectionUI.SetCallbackListener(this);
        _roleSelectionUI.SetItems(_roles.Keys.ToList());
    }

    public void OnItemToggleInvoked(bool isActive, string itemName)
    {
        if(isActive)
        {
            Role role = _roles[itemName];
            PlayerWrapper.Instance.SetRole(role);
        }
        else
        {
            _roleSelectionUI.SetItem(Role.None.ToString(), withoutNotify: false);
        }
    }
}
