using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Avatar.Expo;
using VR_Experiment.Core;
using VR_Experiment.XR;

[RequireComponent(typeof(PhotonView), typeof(TransformFollow))]
public class NetworkedReticle : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private TransformFollow _reticleFollow;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private MeshRenderer _meshRenderer;

    private LaserpointerManager _origin;
    private AvatarLinkBehaviour _avatar;
    private Transform _activeHand;

    private void Awake()
    {
        if(_reticleFollow == null)
            _reticleFollow = GetComponent<TransformFollow>();
    }

    private void Start()
    {
        _origin = FindObjectOfType<LaserpointerManager>();
        _avatar = FindObjectsOfType<AvatarLinkBehaviour>().FirstOrDefault(a => a.PhotonView.Owner.Equals(photonView.Owner));

        Color myColor = _origin.GetColorByActorNumber(photonView.OwnerActorNr);
        _lineRenderer.startColor = myColor;
        _lineRenderer.endColor = myColor;
        _meshRenderer.material.color = myColor;

        _lineRenderer.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_lineRenderer.enabled)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _activeHand.position);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            Vector3 sendDirection = _origin.transform.InverseTransformDirection(transform.position - _origin.transform.position);
            stream.SendNext(sendDirection);
        }
        else if(info.Sender.IsLocal == false)
        {
            if(_origin != null)
            {
                Vector3 receivedDirection = (Vector3)stream.ReceiveNext();
                Vector3 networkedDirection = _origin.transform.TransformDirection(receivedDirection);
                
                transform.position = _origin.transform.position + networkedDirection;
            }
        } 
    }

    public void ToggleReticle(bool active, Hand activeHand = Hand.None, Transform localReticle = null)
    {
        _reticleFollow.followTarget = localReticle;
        gameObject.SetActive(active);
        photonView.RPC(nameof(RPC_ToggleNetworkedReticle), RpcTarget.Others, active, (byte)activeHand);
    }

    [PunRPC]
    private void RPC_ToggleNetworkedReticle(bool active, byte handByte)
    {
        _lineRenderer.enabled = active;
        gameObject.SetActive(active);

        Hand hand = (Hand)handByte;
        switch(hand)
        {
            case Hand.None:
                _activeHand = null;
                break;
            case Hand.Left:
                _activeHand = _avatar.LeftHand;
                break;
            case Hand.Right:
                _activeHand = _avatar.RightHand;
                break;
            default:
                break;
        }
    }
}
