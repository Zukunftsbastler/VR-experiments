using Photon.Pun;
using Photon.Realtime;
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

    private XRDistanceInteractionActivator _activeDistanceActivator;
    private NetworkedReticle _networkedRedicle;

    private bool _forceActive;

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private void Update()
    {
        if(_activeDistanceActivator != null && _activeDistanceActivator.IsActive)
        {
            Vector3 position;

            XRRayInteractor activeRayIntaractor = _activeDistanceActivator.RayInteractor;
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
        _leftDistanceActivator.AllowChange += AllowActivatorChange;
        _leftDistanceActivator.ActiveChanged += OnActiveChanged;
        
        _rightDistanceActivator.AllowChange += AllowActivatorChange;
        _rightDistanceActivator.ActiveChanged += OnActiveChanged;

        PlayerWrapper.Instance.onPropertiesChanged += LocalPlayerPropertiesChanged;
    }

    private void OnDisable()
    {
        _leftDistanceActivator.AllowChange -= AllowActivatorChange;
        _leftDistanceActivator.ActiveChanged -= OnActiveChanged;

        _rightDistanceActivator.AllowChange -= AllowActivatorChange;
        _rightDistanceActivator.ActiveChanged -= OnActiveChanged;

        PlayerWrapper.Instance.onPropertiesChanged += LocalPlayerPropertiesChanged;
    }

    public Color GetColorByActorNumber(int actorNumber)
    {
        return _highlightColors.GetPlayerColorByActorNumber(actorNumber);
    }

    private IEnumerator SetUp()
    {
        yield return PlayerWrapper.Instance.HasSpawned;

        CheckForceActive(PlayerWrapper.Instance.GetLocalRole());
        _networkedRedicle = PhotonNetwork.Instantiate(_networkedReticlePrefab.name, Vector3.zero, Quaternion.identity).GetComponent<NetworkedReticle>();
    }

    private bool AllowActivatorChange(XRDistanceInteractionActivator distanceActivator)
    {
        if(_activeDistanceActivator == distanceActivator)
        {
            return !_forceActive;
        }

        return true;
    }

    private void OnActiveChanged(XRDistanceInteractionActivator distanceActivator, bool isActive)
    {
        // Check what action has been performed => active or not.
        if(isActive)
        {
            // Check for other active activator and disable it
            if(_activeDistanceActivator != null && _activeDistanceActivator.IsActive 
                && _activeDistanceActivator != distanceActivator)
            {
                _activeDistanceActivator.SetActiveDirty(false, true);
                HandleSelectActionSubscription(false, _activeDistanceActivator.Hand);
            }

            _activeDistanceActivator = distanceActivator;
            _networkedRedicle.ToggleReticle(true, _activeDistanceActivator.Hand, _localReticle);
            HandleSelectActionSubscription(true, _activeDistanceActivator.Hand);
        }
        else
        {
            if(_activeDistanceActivator == null || _activeDistanceActivator != distanceActivator)
                return;

            HandleSelectActionSubscription(false, _activeDistanceActivator.Hand);
            _networkedRedicle.ToggleReticle(false);
            _activeDistanceActivator = null;
        }

        void HandleSelectActionSubscription(bool active, Hand hand)
        {
            InputAction action = hand switch
            {
                Hand.Left => _leftHandSelect.action,
                Hand.Right => _rightHandSelect.action,
                _ => null
            };

            if(action != null)
            {
                if(active)
                {
                    action.performed += OnSelectActionPerformed;
                }
                else
                {
                    action.performed -= OnSelectActionPerformed;
                }
            }
        }
    }

    private void OnSelectActionPerformed(InputAction.CallbackContext obj)
    {
        if(PlayerWrapper.Instance.CanManagePointsOfInterest == false)
            return;

        if(_activeDistanceActivator.RayInteractor.TryGetCurrent3DRaycastHit(out _) || 
            _activeDistanceActivator.RayInteractor.TryGetCurrentUIRaycastResult(out _))
            return;

        _pointOfInterest.ToggleUI(_localReticle.position);
    }

    private void CheckForceActive(Role role)
    {
        _forceActive = role > Role.Attendee;

        Debug.LogWarning($"Check for forceActive: {_forceActive}");

        if(_activeDistanceActivator != null)
        {
            _activeDistanceActivator.SetActiveDirty(_forceActive, false);
        }
        else
        {
            _rightDistanceActivator.SetActiveDirty(_forceActive, true);
        }
    }

    private void LocalPlayerPropertiesChanged(object[] data)
    {
        if(data[0] is Role role)
        {
            CheckForceActive(role);
        }
    }
}
