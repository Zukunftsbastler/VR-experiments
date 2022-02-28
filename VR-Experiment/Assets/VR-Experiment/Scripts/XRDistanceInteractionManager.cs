using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDistanceInteractionManager : MonoBehaviour
{
    [SerializeField] private XRRayInteractor _rayInteractor;
    [Space]
    [SerializeField, Range(0f, 1f)] private float _activationThreshold = .5f;
    [Space]
    [SerializeField] private InputActionProperty _activateDistanceInteraction;

    private void Start()
    {

    }

    private void Update()
    {
        bool isActive = _activateDistanceInteraction.action.ReadValue<float>() > _activationThreshold;
        _rayInteractor.enabled = isActive;
    }

    private void OnEnable()
    {
        EnableInputActions();
    }

    private void OnDisable()
    {
        DisableInputActions();
    }

    private void EnableInputActions()
    {
        _activateDistanceInteraction.action.Enable();
    }

    private void DisableInputActions()
    {
        _activateDistanceInteraction.action.Disable();
    }
}
