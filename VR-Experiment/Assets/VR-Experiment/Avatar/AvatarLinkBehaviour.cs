using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarLinkBehaviour : MonoBehaviour
{
    [SerializeField] private TransformFollow _headFollow;
    [SerializeField] private TransformFollow _leftHandFollow;
    [SerializeField] private TransformFollow _rightHandFollow;

    public void LinkRigToAvatar(XRRig rig)
    {
        LinkRigToAvatar(rig.Head, rig.LeftHand, rig.RightHand);
    }

    public void LinkRigToAvatar(Transform rigHead, Transform rigLeftHand, Transform rigRightHand)
    {
        _headFollow.followTarget = rigHead;
        _leftHandFollow.followTarget = rigLeftHand;
        _rightHandFollow.followTarget = rigRightHand;
    }
}
