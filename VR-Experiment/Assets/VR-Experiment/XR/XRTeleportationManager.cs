using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR_Experiment.XR
{
    public class XRTeleportationManager : MonoBehaviour
    {
        [SerializeField] private TeleportationProvider _teleportationProvider;
        [SerializeField] private XRRayInteractor _rayInteractor;
        [Space]
        [SerializeField] private InputActionProperty _activateTeleport;
        [SerializeField] private InputActionProperty _cancleTeleport;

        private bool _isActive;

        private void Start()
        {
            _rayInteractor.enabled = _isActive = false;
        }

        private void OnEnable()
        {
            EnableInputActions();
            SubscribeToInputActions();
        }

        private void OnDisable()
        {
            UnsubscribeFromInputActions();
            DisableInputActions();
        }

        private void EnableInputActions()
        {
            _activateTeleport.action.Enable();
            _cancleTeleport.action.Enable();
        }

        private void DisableInputActions()
        {
            _activateTeleport.action.Disable();
            _cancleTeleport.action.Disable();
        }

        private void SubscribeToInputActions()
        {
            _activateTeleport.action.performed += OnTeleportActivate;
            _activateTeleport.action.canceled += OnTeleportDeactivate;
            _cancleTeleport.action.performed += OnTeleportCancel;
        }

        private void UnsubscribeFromInputActions()
        {
            _activateTeleport.action.performed -= OnTeleportActivate;
            _activateTeleport.action.canceled -= OnTeleportDeactivate;
            _cancleTeleport.action.performed -= OnTeleportCancel;
        }

        private void OnTeleportActivate(InputAction.CallbackContext context)
        {
            _rayInteractor.enabled = _isActive = true;
        }

        private void OnTeleportDeactivate(InputAction.CallbackContext context)
        {
            if(!_isActive)
                return;

            if(_rayInteractor.hasHover && _rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                TeleportRequest request = new TeleportRequest()
                {
                    destinationPosition = hit.point
                };

                _teleportationProvider.QueueTeleportRequest(request);
            }

            _rayInteractor.enabled = _isActive = false;
        }

        private void OnTeleportCancel(InputAction.CallbackContext context)
        {
            _rayInteractor.enabled = _isActive = false;
        }
    }
}
