using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;

public class AvatarHatLink : MonoBehaviour
{
    [SerializeField] private Transform _anchor;

    private PhotonView _photonView;
    private SortingHat _hat;

    public bool HasHat => _hat != null;

    private void Start()
    {
        _photonView = GetComponentInParent<PhotonView>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(HasHat)
        {
            //remove hat
            if(other.TryGetComponentInParent(out SortingHat hat) && _hat.Equals(hat) && hat.IsBehingHeld)
            {
                if(_photonView.IsMine)
                {
                    PlayerWrapper.Instance.SetRole(_hat.PrevPlayerRole);
                }

                _hat.TransformFollow.followTarget = null;
                _hat = null;
            }
        }
        else
        {
            //add hat
            if(other.TryGetComponentInParent(out SortingHat hat) && hat.IsBehingHeld == false)
            {
                _hat = hat;

                _hat.Rigidbody.velocity = Vector3.zero;
                _hat.Rigidbody.useGravity = false;

                _hat.TransformFollow.followTarget = _anchor;

                if(_photonView.IsMine)
                {
                    PlayerWrapper.Instance.SetRole(_hat.SortingRole);
                }
            }
        }
    }
}
