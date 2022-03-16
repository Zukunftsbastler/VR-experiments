using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using VR_Experiment.Avatar.Core;

public class HighlightManager : MonoBehaviourPun
{
    [SerializeField] private PointOfInterestUI _pointOfInterestUI;
    [SerializeField] private SO_PlayerColor _highlightColors;
    [SerializeField] private Color _emptyColor;
    [SerializeField] private List<HighlightBehaviour> _highlights;
    [SerializeField] private InputActionProperty _closeUIInput;

    private List<SO_PointOfInterest> _pointsOfInterest;
    /// <summary>
    /// Collection of highlights whose POI-data is currently changed by other clients.
    /// </summary>
    private readonly HashSet<HighlightBehaviour> _remoteSelectedHighlights = new HashSet<HighlightBehaviour>();

    public HighlightBehaviour LocalActiveHighlight { get; private set; }
    public List<SO_PointOfInterest> PointsOfInterest => _pointsOfInterest ??= Resources.LoadAll<SO_PointOfInterest>("").ToList();
    public Color EmptyColor => _emptyColor;

    //-----------Unity Methods------------------------------------------------------------------------------------------------------
    private void Start()
    {
        Assert.IsNotNull(_pointOfInterestUI, $"{gameObject.name} - {typeof(PointOfInterestUI)} is null. " +
            $"Please ensure this gameobject has a reference to a {typeof(PointOfInterestUI)}.");
    }

    private void OnEnable()
    {
        _closeUIInput.action.Enable();
        _closeUIInput.action.performed += OnCloseUIInputPressed;
    }

    private void OnDisable()
    {
        _closeUIInput.action.performed -= OnCloseUIInputPressed;
        _closeUIInput.action.Disable();
    }

    //-----------Public Methods------------------------------------------------------------------------------------------------------
    public Color GetHoverColorByActorNumber(int actorNumber)
    {
        return _highlightColors.GetPlayerColorByActorNumber(actorNumber);
    }

    public bool IsRemotelyBlocked(HighlightBehaviour localActiveHighlight)
    {
        return localActiveHighlight != null && _remoteSelectedHighlights.Contains(localActiveHighlight);
    }

    public void SetHover(HighlightBehaviour highlight, bool hoverActive)
    {
        int highlightIndex = _highlights.IndexOf(highlight);
        photonView.RPC(nameof(RPC_SetHighlightHover), RpcTarget.Others, highlightIndex, hoverActive);
    }

    public void SetSelect(HighlightBehaviour highlight)
    {
        LocalActiveHighlight = highlight;

        _pointOfInterestUI.ToggleUI(true);        
    }

    public void OnSelectionUIChanged(bool isOpen)
    {
        int highlightIndex = _highlights.IndexOf(LocalActiveHighlight);
        photonView.RPC(nameof(RPC_OnSelectionUIChanged), RpcTarget.Others, highlightIndex, isOpen);
    }

    public void SetPointOfInterest(bool isActive, string pointOfInterestName)
    {
        int highlightIndex = _highlights.IndexOf(LocalActiveHighlight);
        photonView.RPC(nameof(RPC_SetPointOfInterest), RpcTarget.AllBuffered, highlightIndex, isActive, pointOfInterestName);
    }

    //-----------RPC Events---------------------------------------------------------------------------------------------------------
    [PunRPC]
    private void RPC_SetHighlightHover(int highlightIndex, bool hoverActive, PhotonMessageInfo info)
    {
        HighlightBehaviour highlight = _highlights[highlightIndex];
        highlight.SetNetworkedHover(info.Sender.ActorNumber, hoverActive);
    }

    [PunRPC]
    private void RPC_OnSelectionUIChanged(int highlightIndex, bool isOpen)
    {
        //add remoteInteraction
        HighlightBehaviour highlight = _highlights[highlightIndex];
        if(isOpen)
        {
            _remoteSelectedHighlights.Add(highlight);
        }
        else
        {
            _remoteSelectedHighlights.Remove(highlight);
        }

        //update UI
        if(LocalActiveHighlight != null && LocalActiveHighlight == highlight)
        {
            _pointOfInterestUI.UpdateUIButton();
        }
    }

    [PunRPC]
    private void RPC_SetPointOfInterest(int highlightIndex, bool isActive, string pointOfInterestName)
    {
        HighlightBehaviour highlight = _highlights[highlightIndex];

        if(isActive)
        {
            SO_PointOfInterest pointOfInterest = PointsOfInterest.FirstOrDefault(p => p.Name.Equals(pointOfInterestName));
            highlight.SetPointOfInterest(pointOfInterest);
        }
        else
        {
            highlight.SetPointOfInterest(null);
        }

        if(_pointOfInterestUI.IsActive && LocalActiveHighlight == highlight)
        {
            _pointOfInterestUI.UpdateUIButton();
        }
    }

    //-----------Event Callbacks----------------------------------------------------------------------------------------------------
    private void OnCloseUIInputPressed(InputAction.CallbackContext obj)
    {
        if(_pointOfInterestUI.IsActive)
        {
            if(_pointOfInterestUI.IsSelectingPointOfInterest)
                OnSelectionUIChanged(false);

            _pointOfInterestUI.ToggleUI(false);
        }
    }
}
