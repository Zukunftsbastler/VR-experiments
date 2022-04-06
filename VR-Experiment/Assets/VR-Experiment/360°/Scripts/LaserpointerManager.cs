using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using VR_Experiment.Avatar.Core;
using VR_Experiment.Core;
using VR_Experiment.Networking;
using VR_Experiment.XR;

public class LaserpointerManager : MonoBehaviour
{
    [SerializeField] private SO_PlayerColor _highlightColors;
    [Space]
    [SerializeField] private GameObject _networkedReticlePrefab;
    [SerializeField] private Transform _localReticle;
    [Space]
    [SerializeField] private XRDistanceInteractionActivator _leftDistanceActivator;
    [SerializeField] private XRDistanceInteractionActivator _rightDistanceActivator;
    [Space]
    [SerializeField] private PointOfInterestUI _pointOfInterest;
    [SerializeField] private InputActionProperty _leftHandSelect;
    [SerializeField] private InputActionProperty _rightHandSelect;

    private XRDistanceInteractionActivator _activeDistanceInteractor;
    private NetworkedReticle _networkedRedicle;

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private void Update()
    {
        if(_activeDistanceInteractor != null && _activeDistanceInteractor.IsActive)
        {
            Vector3 position;

            XRRayInteractor activeRayIntaractor = _activeDistanceInteractor.RayInteractor;
            if(activeRayIntaractor.TryGetCurrent3DRaycastHit(out RaycastHit rayInteractorHit))
            {
                position = rayInteractorHit.point;
            }
            else if(activeRayIntaractor.TryGetCurrentUIRaycastResult(out RaycastResult rayInteractorResult))
            {
                position = rayInteractorResult.worldPosition;
            }
            else
            {
                Transform rayEndOrigin = activeRayIntaractor.rayOriginTransform;
                LayerMask raycastMask = activeRayIntaractor.raycastMask;
                float rayDist = activeRayIntaractor.maxRaycastDistance;
                Ray ray = new Ray()
                {
                    direction = rayEndOrigin.forward * -1,
                    origin = rayEndOrigin.position + rayEndOrigin.forward * rayDist,
                };

                Physics.Raycast(ray, out RaycastHit backwardsRayHit, rayDist, raycastMask, QueryTriggerInteraction.Collide);
                position = backwardsRayHit.point;
            }

            _localReticle.position = position;
        }
    }

    private void OnEnable()
    {
        _leftDistanceActivator.activeChanged += OnActiveChanged;
        _rightDistanceActivator.activeChanged += OnActiveChanged;
    }

    private void OnDisable()
    {
        _leftDistanceActivator.activeChanged -= OnActiveChanged;
        _rightDistanceActivator.activeChanged -= OnActiveChanged;
    }

    public Color GetColorByActorNumber(int actorNumber)
    {
        return _highlightColors.GetPlayerColorByActorNumber(actorNumber);
    }

    private IEnumerator SetUp()
    {
        yield return PlayerWrapper.Instance.HasSpawned;

        _networkedRedicle = PhotonNetwork.Instantiate(_networkedReticlePrefab.name, Vector3.zero, Quaternion.identity).GetComponent<NetworkedReticle>();
    }

    private void OnActiveChanged(XRDistanceInteractionActivator distanceActivator, bool isActive)
    {
        if(isActive)
        {
            if(_activeDistanceInteractor != null && _activeDistanceInteractor.IsActive)
            {
                _activeDistanceInteractor.SetActiveDirty(false);
                ToggleInput(false, _activeDistanceInteractor.Hand);
            }

            _activeDistanceInteractor = distanceActivator;
            _networkedRedicle.ToggleReticle(true, _activeDistanceInteractor.Hand, _localReticle);
            ToggleInput(true, _activeDistanceInteractor.Hand);
        }
        else
        {
            ToggleInput(false, _activeDistanceInteractor.Hand);
            _networkedRedicle.ToggleReticle(false);
            _activeDistanceInteractor = null;
        }

        void ToggleInput(bool active, Hand hand)
        {
            switch(hand)
            {
                case Hand.None:
                    break;
                case Hand.Left:
                    if(active)
                    {
                        _leftHandSelect.action.performed += OnSelectActionPerformed;
                    }
                    else
                    {
                        _leftHandSelect.action.performed -= OnSelectActionPerformed;
                    }
                    break;
                case Hand.Right:
                    if(active)
                    {
                        _rightHandSelect.action.performed += OnSelectActionPerformed;
                    }
                    else
                    {
                        _rightHandSelect.action.performed -= OnSelectActionPerformed;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnSelectActionPerformed(InputAction.CallbackContext obj)
    {
        if(PlayerWrapper.Instance.CanManagePointsOfInterest == false)
            return;

        if(_activeDistanceInteractor.RayInteractor.TryGetCurrent3DRaycastHit(out _) || 
            _activeDistanceInteractor.RayInteractor.TryGetCurrentUIRaycastResult(out _))
            return;

        _pointOfInterest.ToggleUI(_localReticle.position);
    }
}
