using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Enums;

public class JoinRoomUI : MonoBehaviour
{
    [SerializeField] private SO_Avatar _standartAvatar;
    [SerializeField] private Role _standartRole;
    [Space]
    [SerializeField] private AvatarWrapperUI _avatarWrapper;
    [SerializeField] private RoleWrapperUI _roleWrapper;
    [SerializeField] private Text _debugText;
    [SerializeField] private Button _joinButton;

    public event Action joinRoomButtonClicked;

    private void Start()
    {
        _avatarWrapper.SelectionUI.SetItem(_standartAvatar.Name, withoutNotify: false);
        _roleWrapper.SelectionUI.SetItem(_standartRole.ToString(), withoutNotify: false);
    }

    private void OnEnable()
    {
        _joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private void OnDisable()
    {
        _joinButton.onClick.RemoveListener(OnJoinButtonClicked);
    }

    public void SetDebugText(string text)
    {
        _joinButton.interactable = string.IsNullOrEmpty(text);
        _debugText.text = text;
    }

    private void OnJoinButtonClicked()
    {
        joinRoomButtonClicked?.Invoke();
    }
}
