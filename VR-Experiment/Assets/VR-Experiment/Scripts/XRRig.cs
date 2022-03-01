using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

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
    }

    public void TeleportRig(Transform point)
    {
        Vector3 heightAdjustment = _xROrigin.Origin.transform.up * _xROrigin.CameraInOriginSpaceHeight;
        Vector3 cameraDestination = point.position + heightAdjustment;
        _xROrigin.MoveCameraToWorldLocation(cameraDestination);

        float signedAngle = Vector3.SignedAngle(_xROrigin.transform.forward, point.forward, Vector3.up);
        _xROrigin.RotateAroundCameraPosition(Vector3.up, signedAngle);
    }
}
