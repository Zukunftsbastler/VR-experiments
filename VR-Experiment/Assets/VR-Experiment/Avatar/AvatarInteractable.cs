using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarInteractable : XRBaseInteractable
{
    [Space]
    [SerializeField] private AvatarInfoUI _avatarInfoUI;

    private void Start()
    {
        Photon.Pun.PhotonView pv = GetComponent<Photon.Pun.PhotonView>();

        if(pv == null)
        {
            Debug.LogError("No photonView found.");
            return;
        }

        if(pv.Owner != null && pv.Owner.IsLocal)
        {
            interactionLayers = InteractionLayerMask.GetMask("NotInteractable");
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        //TODO: add condition so you can't tag yourself
        _avatarInfoUI.ToggleUI();
    }
}
