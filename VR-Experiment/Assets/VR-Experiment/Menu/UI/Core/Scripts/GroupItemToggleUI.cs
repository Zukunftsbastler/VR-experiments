using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace VR_Experiment.Menu.UI.Core
{
    [RequireComponent(typeof(Toggle))]
    public class GroupItemToggleUI : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Text _lable;

        private string _itemName;
        private Action<bool, GroupItemToggleUI> _onToggleValueChanged;

        public Toggle Toggle => _toggle;
        public string ItemName => _itemName;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();

            Assert.IsNotNull(_toggle, $"Toggle is null, please add it to {this.gameObject}");
            Assert.IsNotNull(_lable, $"Lable is null, please add reference to {this.name}");
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        public void SetItem(string itemName, Action<bool, GroupItemToggleUI> onProductToggleUIInvoked, Sprite image = null)
        {
            _itemName = itemName;
            _onToggleValueChanged = onProductToggleUIInvoked;

            _lable.text = itemName;
            if(image != null)
            {
                _toggle.image.sprite = image;
            }
        }

        private void OnToggleValueChanged(bool isActive)
        {
            _onToggleValueChanged?.Invoke(isActive, this);
        }
    }
}
