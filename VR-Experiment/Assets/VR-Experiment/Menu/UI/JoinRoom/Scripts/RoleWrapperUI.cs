using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Core;
using VR_Experiment.Menu.UI.Core;

namespace VR_Experiment.Menu.UI.JoinRoom
{
    public class RoleWrapperUI : MonoBehaviour, IItemListCallbackListener
    {
        [SerializeField] private ScrollableItemListUI _roleSelectionUI;
        [SerializeField] private Toggle _expoToggle;
        [SerializeField] private Toggle _threesixtyToggle;

        private Dictionary<string, Role> _roles;
        private Role _prevPlayerRole;

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

            _expoToggle.onValueChanged.AddListener(OnExpoToggleChanged);
            _threesixtyToggle.onValueChanged.AddListener(OnTthreesixtyToggleChanged);
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

        private void OnExpoToggleChanged(bool active)
        {
            if(active)
            {
                _roleSelectionUI.SetItem(_prevPlayerRole.ToString(), withoutNotify: false);
            }
            else
            {
                _prevPlayerRole = PlayerWrapper.Instance.GetLocalRole();
            }

            _roleSelectionUI.SetInteractable(true);
        }

        private void OnTthreesixtyToggleChanged(bool active)
        {
            if(active)
            {
                _prevPlayerRole = PlayerWrapper.Instance.GetLocalRole();
                _roleSelectionUI.SetItem(Role.Attendee.ToString(), withoutNotify: false);
            }

            _roleSelectionUI.SetInteractable(!active);
        }
    }
}
