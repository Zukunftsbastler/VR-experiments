using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Assertions;

namespace VR_Experiment.XR
{
    [RequireComponent(typeof(XROrigin))]
    public class XRRig : MonoBehaviour
    {
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightHand;

        private XROrigin _xROrigin;

        public Transform Head => _head;
        public Transform LeftHand => _leftHand;
        public Transform RightHand => _rightHand;

        private void Awake()
        {
            _xROrigin = GetComponent<XROrigin>();

            Assert.IsNotNull(_xROrigin, $"{gameObject.name} - {nameof(_xROrigin)} is null. Please ensure this {typeof(XRRig)} is part of a XR Origin.");
        }

        /// <summary>
        /// <see cref="XROrigin.MoveCameraToWorldLocation(Vector3)">Teleports</see> and 
        /// <see cref="XROrigin.RotateAroundCameraPosition(Vector3, float)">rotates</see> the
        /// player to a spesific location.
        /// </summary>
        /// <param name="position">Position the player gets teleported to.</param>
        /// <param name="orientation">Forward orientation after teleport.</param>
        public void TeleportRig(Vector3 position, Vector3 orientation)
        {
            Vector3 heightAdjustment = _xROrigin.Origin.transform.up * _xROrigin.CameraInOriginSpaceHeight;
            Vector3 cameraDestination = position + heightAdjustment;
            _xROrigin.MoveCameraToWorldLocation(cameraDestination);

            float signedAngle = Vector3.SignedAngle(_xROrigin.transform.forward, orientation, Vector3.up);
            _xROrigin.RotateAroundCameraPosition(Vector3.up, signedAngle);
        }
    }
}
