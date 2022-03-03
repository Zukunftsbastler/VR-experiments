using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DildoInteractionBehaviour : MonoBehaviour
{
    [SerializeField] private InputActionProperty _dildoInteraction;
    [SerializeField] private XRGrabInteractable _interactable;

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(SelectedEntered);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(SelectedEntered);
    }

    private void SelectedEntered(SelectEnterEventArgs args)
    {

    }
}
