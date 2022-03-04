using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TalkingpointsMessage : MonoBehaviour
{
    [SerializeField] private Text _textfield;
    [SerializeField] private InputActionProperty _deleteMessage;

    private void OnEnable()
    {
        _deleteMessage.action.Enable();
        _deleteMessage.action.performed += OnDeleteMessage;
    }

    private void OnDisable()
    {
        _deleteMessage.action.performed -= OnDeleteMessage;
        _deleteMessage.action.Disable();
    }

    public void DisplayMessage(string message)
    {
        _textfield.text = message;
    }

    private void OnDeleteMessage(InputAction.CallbackContext callback)
    {
        Destroy(gameObject);
    }
}
