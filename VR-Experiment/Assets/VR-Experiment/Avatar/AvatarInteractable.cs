using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarInteractable : XRBaseInteractable
{
    [Space]
    [SerializeField] private AvatarInfoUI _avatarInfoUI;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        //TODO: add condition so you cant tag yourself
        _avatarInfoUI.ToggleUI();
    }
}
