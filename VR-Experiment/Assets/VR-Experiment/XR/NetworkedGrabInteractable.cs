using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR_Experiment.XR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class NetworkedGrabInteractable : MonoBehaviourPun, IPunOwnershipCallbacks
    {
        private bool _isBehingHeld;
        protected XRGrabInteractable _interactable;
        protected Rigidbody _rigidbody;
        public bool IsBehingHeld
        {
            get => _isBehingHeld;
            set
            {
                _isBehingHeld = value;
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        protected virtual void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(_interactable, $"XRGrabInteractable is null, please add it to {this.gameObject}");
            Assert.IsNotNull(photonView, $"Photonview is null!");

            photonView.OwnershipTransfer = OwnershipOption.Request;

            IsBehingHeld = false;
            _interactable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
            _interactable.selectEntered.AddListener(OnSelectEntered);
            _interactable.selectExited.AddListener(OnSelectExit);
        }

        protected virtual void OnSelectEntered(SelectEnterEventArgs args)
        {
            Debug.Log($"OnSelectEntered!");

            if(IsBehingHeld)
            {
                Debug.Log($"This object is already held, skipping");
                return;
            }

            if(photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_StartNetworkGrabbing), RpcTarget.AllBuffered);
            }
            else
            {
                photonView.RequestOwnership();
            }
        }

        protected virtual void OnSelectExit(SelectExitEventArgs args)
        {
            photonView.RPC(nameof(RPC_StopNetworkGrabbing), RpcTarget.AllBuffered);
            Debug.Log($"OnSelectExit!");
        }

        [PunRPC]
        protected virtual void RPC_StartNetworkGrabbing()
        {
            IsBehingHeld = true;
            _rigidbody.useGravity = false;
            _interactable.interactionLayers = photonView.IsMine ? InteractionLayerMask.GetMask("Interactable") : InteractionLayerMask.GetMask($"NotInteractable");
            Debug.Log($"Starting Network Grabbing!");
        }

        [PunRPC]
        protected virtual void RPC_OnNetworkGrabFailed()
        {
            Debug.Log($"{photonView.Owner.NickName} Grab Failed! Is already Grabbed!");
        }

        [PunRPC]
        protected virtual void RPC_StopNetworkGrabbing()
        {
            IsBehingHeld = false;
            _rigidbody.useGravity = true;
            _interactable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
            Debug.Log($"Stopping Network Grabbing!");
        }

        void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            if(targetView == this.photonView && targetView.IsMine)
            {
                Debug.Log($"OwnerShip requested for: {targetView.name} name {requestingPlayer.NickName}");
                if(IsBehingHeld)
                {
                    // No need to buffer the call unless important data is synced
                    photonView.RPC(nameof(RPC_OnNetworkGrabFailed), RpcTarget.All);
                }
                else
                {
                    photonView.TransferOwnership(requestingPlayer);
                }
            }
        }

        void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            Debug.Log($"OwnerShip granted to {targetView.name} from {previousOwner.NickName}");
            if(targetView == this.photonView && targetView.IsMine)
            {
                photonView.RPC(nameof(RPC_StartNetworkGrabbing), RpcTarget.AllBuffered);
            }
        }

        void IPunOwnershipCallbacks.OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        { }
    }
}
