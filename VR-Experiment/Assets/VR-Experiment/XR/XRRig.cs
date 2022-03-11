using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class XRRig : MonoBehaviour
{
    [SerializeField] private UnityEngine.Transform _head;
    [SerializeField] private UnityEngine.Transform _leftHand;
    [SerializeField] private UnityEngine.Transform _rightHand;

    private XROrigin _xROrigin;

    public UnityEngine.Transform Head => _head;
    public UnityEngine.Transform LeftHand => _leftHand;
    public UnityEngine.Transform RightHand => _rightHand;

    private void Awake()
    {
        _xROrigin = GetComponent<XROrigin>();
    }

    public void TeleportRig(Vector3 position, Vector3 orientation)
    {
        Vector3 heightAdjustment = _xROrigin.Origin.transform.up * _xROrigin.CameraInOriginSpaceHeight;
        Vector3 cameraDestination = position + heightAdjustment;
        _xROrigin.MoveCameraToWorldLocation(cameraDestination);

        float signedAngle = Vector3.SignedAngle(_xROrigin.transform.forward, orientation, Vector3.up);
        _xROrigin.RotateAroundCameraPosition(Vector3.up, signedAngle);
    }
}
