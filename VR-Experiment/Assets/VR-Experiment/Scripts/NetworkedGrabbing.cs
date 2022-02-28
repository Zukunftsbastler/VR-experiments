using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhotonTransformView))]
public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private XRGrabInteractable _interActable;
    private PhotonTransformView _photonView;
    private Rigidbody _rb;
    private bool _isBehingHeld;
    public bool IsBehingHeld
    {
        get => _isBehingHeld;
        set
        {
            _rb.isKinematic = value;
            _isBehingHeld = value;
        }
    }

    private void Awake()
    {
        _interActable = GetComponent<XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(_interActable, $"XRGrabInteractable is null, please add it to {this.gameObject}");
        Assert.IsNotNull(_rb, $"Theres no Rigidbody attached to {this.gameObject}. Make sure its added.");
        _interActable.selectEntered.AddListener(OnSelectEntered);
        _interActable.selectExited.AddListener(OnSelectExit);
        _photonView.photonView.OwnershipTransfer = OwnershipOption.Request;
    }

    public void OnSelectEntered(SelectEnterEventArgs en)
    {
        _photonView.photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
        if (_photonView.photonView.Owner != PhotonNetwork.LocalPlayer)
        {
            TransferOwnership();
        }
    }

    public void OnSelectExit(SelectExitEventArgs ex)
    {
        _photonView.photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
        Debug.Log($"OnSelectExit!");
    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        IsBehingHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        IsBehingHeld = false;
    }

    private void TransferOwnership()
    {
        _photonView.photonView.RequestOwnership();
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log($"OwnerShip requested for: {targetView.name} name {requestingPlayer.NickName}");
        if (targetView == _photonView.photonView)
        {
            _photonView.photonView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log($"OwnerShip granted to {targetView.name} from {previousOwner.NickName}");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }
}
