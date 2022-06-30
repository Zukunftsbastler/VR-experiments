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

        public event Func<XRDistanceInteractionActivator, bool> AllowChange;
        public event Action<XRDistanceInteractionActivator, bool> ActiveChanged;

        private float _lastInputValue = 0;

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

        private void OnEnable()
        {
            EnableInputActions();
            _activateDistanceInteraction.action.performed += OnInteractionPerformed;
        }

        private void OnDisable()
        {
            DisableInputActions();
            _activateDistanceInteraction.action.performed -= OnInteractionPerformed;
        }

        /// <summary>
        /// Sets the <see cref="XRRayInteractor"/> of a spesific <see cref="Core.Hand"/>
        /// </summary>
        /// <param name="isActive"> Defines whether the <see cref="XRRayInteractor"/> gets activated or not.</param>
        /// <param name="setSilent"> Defines whether subscriber to <see cref="ActiveChanged"/> should be notified or not.</param>
        public void SetActiveDirty(bool isActive, bool setSilent = false)
        {
            if(IsActive == isActive)
                return;

            bool allowChange = true;
            if(AllowChange != null)
            {
                allowChange = AllowChange.Invoke(this);
            }

            if(!allowChange)
                return;

            _rayInteractor.enabled = isActive;

            if(setSilent == false)
            {
                ActiveChanged?.Invoke(this, isActive);
            }
        }

        private void EnableInputActions()
        {
            _activateDistanceInteraction.action.Enable();
        }

        private void DisableInputActions()
        {
            _activateDistanceInteraction.action.Disable();
        }

        private void OnInteractionPerformed(InputAction.CallbackContext input)
        {
            float activeInputValue = input.action.ReadValue<float>();

            if(_lastInputValue < activeInputValue && activeInputValue > _activationThreshold)
            {
                SetActiveDirty(true);
            }
            else
            {
                SetActiveDirty(false);
            }

            _lastInputValue = activeInputValue;
        }
    }
}
