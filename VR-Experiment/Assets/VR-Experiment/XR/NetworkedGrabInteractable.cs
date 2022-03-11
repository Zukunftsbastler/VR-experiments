using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR_Experiment.XR
{
    [RequireComponent(typeof(PhotonRigidbodyView), typeof(XRGrabInteractable))]
    public class NetworkedGrabInteractable : MonoBehaviourPun, IPunOwnershipCallbacks
    {
        private bool _isBehingHeld;
        protected XRGrabInteractable _interActable;
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
            _interActable = GetComponent<XRGrabInteractable>();
            _rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(_interActable, $"XRGrabInteractable is null, please add it to {this.gameObject}");
            Assert.IsNotNull(photonView, $"Photonview is null!");

            photonView.OwnershipTransfer = OwnershipOption.Request;

            IsBehingHeld = false;
            _interActable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
            _interActable.selectEntered.AddListener(OnSelectEntered);
            _interActable.selectExited.AddListener(OnSelectExit);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
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

        private void OnSelectExit(SelectExitEventArgs args)
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
            Debug.Log($"{photonView.Owner.NickName} Grab Failed! Is already Grabbed!");
        }

        [PunRPC]
        protected virtual void RPC_StopNetworkGrabbing()
        {
            IsBehingHeld = false;
            _interActable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
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
