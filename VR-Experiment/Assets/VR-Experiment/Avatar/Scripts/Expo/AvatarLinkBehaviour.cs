using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;
using VR_Experiment.XR;

namespace VR_Experiment.Avatar.Expo
{
    public class AvatarLinkBehaviour : MonoBehaviour
    {
        [SerializeField] private TransformFollow _headFollow;
        [SerializeField] private TransformFollow _leftHandFollow;
        [SerializeField] private TransformFollow _rightHandFollow;

        private PhotonView _photonView;
        public PhotonView PhotonView => _photonView ??= GetComponent<PhotonView>();

        public Transform LeftHand => _leftHandFollow.transform;
        public Transform RightHand => _rightHandFollow.transform;

        /// <summary>
        /// Connects the rig to a networked avater. Sets <see cref="XRRig.Head">Head</see>, <see cref="XRRig.LeftHand">left Hand</see> 
        /// and <see cref="XRRig.RightHand">right Hand</see> as <see cref="TransformFollow.followTarget">follow Targets</see> of this avatar.
        /// </summary>
        /// <param name="rig"></param>
        public void LinkRigToAvatar(XRRig rig)
        {
            LinkRigToAvatar(rig.Head, rig.LeftHand, rig.RightHand);
        }

        /// <summary>
        /// Sets head and hand transforms as <see cref="TransformFollow.followTarget">follow Targets</see> of this avatar.
        /// </summary>
        /// <param name="rigHead"></param>
        /// <param name="rigLeftHand"></param>
        /// <param name="rigRightHand"></param>
        public void LinkRigToAvatar(Transform rigHead, Transform rigLeftHand, Transform rigRightHand)
        {
            _headFollow.followTarget = rigHead;
            _leftHandFollow.followTarget = rigLeftHand;
            _rightHandFollow.followTarget = rigRightHand;
        }
    }
}
