using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarLinkBehaviour : MonoBehaviour
{
    [SerializeField] private TransformFollow _headFollow;
    [SerializeField] private TransformFollow _leftHandFollow;
    [SerializeField] private TransformFollow _rightHandFollow;

    public void LinkRigToAvatar(Transform head, Transform leftHand, Transform rightHand)
    {
        _headFollow.followTarget = head;
        _leftHandFollow.followTarget = leftHand;
        _rightHandFollow.followTarget = rightHand;
    }
}
