using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VR_Experiment.Networking;

namespace VR_Experiment.Menu.UI.Core
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Transform _xrHead;
        [SerializeField] private float _offset = 3f;
        [SerializeField] private InputActionProperty _openCloseInput;
        [Space]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _openCloseInput.action.Enable();
            _openCloseInput.action.performed += OnOpenClosePerformed;

            _canvas.SetActive(false);
        }

        private void Update()
        {
            if(_canvas.activeSelf)
            {
                float distance = Vector3.SqrMagnitude(_canvas.transform.position - _xrHead.position);
                if(distance > MaxDistSqr())
                {
                    ToggleUI();
                }

                float MaxDistSqr()
                {
                    return (_offset + _offset) * (_offset + _offset);
                }
            }
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnOpenClosePerformed(InputAction.CallbackContext obj)
        {
            if(obj.performed)
            {
                ToggleUI();
            }
        }

        private void OnCloseButtonClicked()
        {
            ToggleUI();
        }

        private void OnExitButtonClicked()
        {
            PhotonRoomInstatiation.Instance.ExitExperiment();
        }

        private void ToggleUI()
        {
            _canvas.SetActive(!_canvas.activeSelf);
            if(_canvas.activeSelf)
            {
                _canvas.transform.position = _xrHead.position + _xrHead.forward * _offset;
                _canvas.transform.forward = _xrHead.forward;
            }
        }
    }
}
