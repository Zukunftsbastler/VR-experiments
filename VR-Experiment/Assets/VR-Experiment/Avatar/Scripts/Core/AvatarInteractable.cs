using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR_Experiment.Menu.UI.Core;

namespace VR_Experiment.Avatar.Core
{
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

            if(IsLocalPlayer(pv))
            {
                //interactionLayers = InteractionLayerMask.GetMask("NotInteractable");
            }

            bool IsLocalPlayer(Photon.Pun.PhotonView photonView)
            {
                return pv.Owner != null && pv.Owner.IsLocal;
            }
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);

            _avatarInfoUI.ToggleUI();
        }
    }
}
