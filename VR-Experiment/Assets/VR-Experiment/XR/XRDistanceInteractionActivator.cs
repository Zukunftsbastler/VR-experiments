using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using VR_Experiment.Core;

namespace VR_Experiment.XR
{
    [RequireComponent(typeof(XRRayInteractor))]
    public class XRDistanceInteractionActivator : MonoBehaviour
    {
        [SerializeField] private Hand _hand;
        [SerializeField] private XRRayInteractor _rayInteractor;
        [Space]
        [SerializeField, Range(0f, 1f)] private float _activationThreshold = .5f;
        [SerializeField] private InputActionProperty _activateDistanceInteraction;

        public event Action<XRDistanceInteractionActivator, bool> activeChanged;

        private bool _lastActive;

        public Hand Hand => _hand;
        public bool IsActive => _rayInteractor.enabled;
        public XRRayInteractor RayInteractor => _rayInteractor;

        private void Start()
        {
            if(_rayInteractor == null)
                _rayInteractor = GetComponent<XRRayInteractor>();

            Assert.IsNotNull(_rayInteractor, $"{gameObject.name} - {typeof(XRRayInteractor)} is null. " +
            $"Please ensure this gameobject has a '{typeof(XRRayInteractor)}' component.");
        }

        private void Update()
        {
            bool isActive = _activateDistanceInteraction.action.ReadValue<float>() > _activationThreshold;

            if(_lastActive != isActive)
            {
                SetActiveDirty(isActive);
                activeChanged?.Invoke(this, isActive);
            }
        }

        private void OnEnable()
        {
            EnableInputActions();
            activeChanged += OnActiveChanged;
        }

        private void OnDisable()
        {
            DisableInputActions();
            activeChanged -= OnActiveChanged;
        }

        public void SetActiveDirty(bool isActive)
        {
            _rayInteractor.enabled = isActive;
        } 

        private void EnableInputActions()
        {
            _activateDistanceInteraction.action.Enable();
        }

        private void DisableInputActions()
        {
            _activateDistanceInteraction.action.Disable();
        }

        private void OnActiveChanged(XRDistanceInteractionActivator distanceActivator, bool isActive)
        {
            if(distanceActivator != this)
                return;

            _lastActive = isActive;
        }
    }
}
