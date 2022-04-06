using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR_Experiment.Expo.Product
{
    public class VibrationFeedback : ProductBehaviour
    {
        [Header("Vibration Feedback")]
        [SerializeField, Range(0f, 1f)] private float _vibrationPower;

        private ActionBasedController _activeController = null;
        private bool _vibrationIsActive;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if(args.interactorObject.transform.TryGetComponent(out ActionBasedController controller) && _activeController == null)
            {
                _activeController = controller;
                _activeController.activateAction.action.performed += OnActivadeActionPerformed;
            }
        }

        protected override void OnSelectExit(SelectExitEventArgs args)
        {
            base.OnSelectExit(args);

            if(args.interactorObject.transform.TryGetComponent(out XRBaseController controller) && controller.Equals(_activeController))
            {
                _activeController.activateAction.action.performed -= OnActivadeActionPerformed;
                _activeController = null;
            }
        }

        private void OnActivadeActionPerformed(InputAction.CallbackContext obj)
        {
            if(IsBehingHeld == false)
                return;

            photonView.RPC(nameof(RPC_SetVibrationActive), RpcTarget.All);
        }

        [PunRPC]
        private void RPC_SetVibrationActive()
        {
            _vibrationIsActive = !_vibrationIsActive;

            if(_vibrationIsActive)
            {
                StartCoroutine(SendHapticImpulse());
            }
        }

        private IEnumerator SendHapticImpulse()
        {
            do
            {
                if(_activeController != null)
                    _activeController.SendHapticImpulse(_vibrationPower, Time.deltaTime);

                yield return null;
            }
            while(_vibrationIsActive);
        }
    }
}
