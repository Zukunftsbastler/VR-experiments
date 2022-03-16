using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class SnapTurnObjects : MonoBehaviour
{
    [SerializeField, Range(0, 90)] private int _turnAmount = 45;
    [SerializeField] private InputActionProperty _snapTurnInput;

    private void OnEnable()
    {
        _snapTurnInput.action.Enable();
        _snapTurnInput.action.performed += OnSnapTurnPerformed;
    }

    private void OnDisable()
    {
        _snapTurnInput.action.Disable();
        _snapTurnInput.action.performed -= OnSnapTurnPerformed;
    }

    private void OnSnapTurnPerformed(InputAction.CallbackContext obj)
    {
        Vector2 value = _snapTurnInput.action.ReadValue<Vector2>();

        int turnAmount = 0;
        Cardinal cardinal = CardinalUtility.GetNearestCardinal(value);
        switch(cardinal)
        {
            case Cardinal.North:
                break;
            case Cardinal.South:
                turnAmount = 180;
                break;
            case Cardinal.East:
                turnAmount = _turnAmount * (-1);
                break;
            case Cardinal.West:
                turnAmount = _turnAmount;
                break;
            default:
                break;
        }

        transform.Rotate(Vector3.up, turnAmount);
    }
}
