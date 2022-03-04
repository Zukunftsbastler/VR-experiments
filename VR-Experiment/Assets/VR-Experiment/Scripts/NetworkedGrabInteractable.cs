using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhotonRigidbodyView), typeof(XRGrabInteractable))]
public class NetworkedGrabInteractable : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private bool _isBehingHeld;
    protected XRGrabInteractable _interActable;

    public bool IsBehingHeld
    {
        get => _isBehingHeld;
        set
        {
            _isBehingHeld = value;
        }
    }

    protected virtual void Awake()
    {
        _interActable = GetComponent<XRGrabInteractable>();

        Assert.IsNotNull(_interActable, $"XRGrabInteractable is null, please add it to {this.gameObject}");
        Assert.IsNotNull(photonView, $"Photonview is null!");

        _interActable.selectEntered.AddListener(OnSelectEntered);
        IsBehingHeld = false;
        _interActable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
        _interActable.selectExited.AddListener(OnSelectExit);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (IsBehingHeld)
        {
            Debug.Log($"This object is already held, skipping");
            return;
        }
        photonView.RPC(nameof(RPC_StartNetworkGrabbing), RpcTarget.AllBuffered);
        if (photonView.Owner != PhotonNetwork.LocalPlayer)
        {
            TransferOwnership();
        }
    }

    public void OnSelectExit(SelectExitEventArgs ex)
    {
        photonView.RPC(nameof(RPC_StopNetworkGrabbing), RpcTarget.AllBuffered);
        Debug.Log($"OnSelectExit!");
    }

    [PunRPC]
    protected virtual void RPC_StartNetworkGrabbing()
    {
        IsBehingHeld = true;
        _interActable.interactionLayers = photonView.IsMine ? InteractionLayerMask.GetMask("Interactable") : InteractionLayerMask.GetMask($"NotInteractable");
        Debug.Log($"Starting Network Grabbing!");
    }

    [PunRPC]
    protected virtual void RPC_OnNetworkGrabFailed()
    {
        Debug.Log($"{photonView.Owner.NickName} Grab Failed! Is already Grabbed");
    }

    [PunRPC]
    protected virtual void RPC_StopNetworkGrabbing()
    {
        IsBehingHeld = false;
        _interActable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
        Debug.Log($"Stopping Network Grabbing");
    }

    private void TransferOwnership()
    {
        photonView.RequestOwnership();
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView == photonView)
        {
            Debug.Log($"OwnerShip requested for: {targetView.name} name {requestingPlayer.NickName}");
            photonView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log($"OwnerShip granted to {targetView.name} from {previousOwner.NickName}");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    { }
}
