using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarHandAnimation : MonoBehaviour
{
    [Header("Left Hand")]
    [SerializeField] private Animator _lHAnimator;
    [SerializeField] private InputActionProperty _lHTrigger;
    [SerializeField] private InputActionProperty _lHGrip;
    [Space]
    [Header("Right Hand")]
    [SerializeField] private Animator _rHAnimator;
    [SerializeField] private InputActionProperty _rHTrigger;
    [SerializeField] private InputActionProperty _rHGrip;

    private bool shouldAnimate;

    private void Start()
    {
        shouldAnimate = _lHAnimator != null && _lHTrigger != null && _lHGrip != null &&
            _rHAnimator != null && _rHTrigger != null && _rHGrip != null &&
            TryGetComponent(out PhotonView pv) && pv.IsMine;
    }

    private void Update()
    {
        if(shouldAnimate)
        {
            //set left hand animations
            float leftTrigger = _lHTrigger.action.ReadValue<float>();
            _lHAnimator.SetFloat("Trigger", leftTrigger);
            float leftGrip = _lHGrip.action.ReadValue<float>();
            _lHAnimator.SetFloat("Grip", leftGrip);


            //set right hand animations
            float rightTrigger = _rHTrigger.action.ReadValue<float>();
            _rHAnimator.SetFloat("Trigger", rightTrigger);
            float rightGrip = _rHGrip.action.ReadValue<float>();
            _rHAnimator.SetFloat("Grip", rightGrip);
        }
    }
}
