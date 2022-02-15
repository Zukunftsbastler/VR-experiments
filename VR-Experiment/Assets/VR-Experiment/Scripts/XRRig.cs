using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRRig : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    public Transform Head => _head;
    public Transform LeftHand => _leftHand;
    public Transform RightHand => _rightHand;
}
