using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;
using VR_Experiment.Core;
using VR_Experiment.Networking;

[Obsolete]
[RequireComponent(typeof(MeshRenderer))]
public class HighlightBehaviour : XRBaseInteractable
{
    [SerializeField] private SO_HotspotData _pointOfInterest;

    private HighlightManager _manager;
    private Renderer _renderer;

    private bool _hasLocalHover;
    private HashSet<int> _networkedHoverClients;

    public bool HasPointOfInterest => _pointOfInterest != null;
    public SO_HotspotData PointOfInterest => _pointOfInterest;

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<MeshRenderer>();
        _manager = GetComponentInParent<HighlightManager>();

        Assert.IsNotNull(_renderer, $"{gameObject.name} - {typeof(MeshRenderer)} is null. " +
            $"Please ensure this gameobject has a {typeof(MeshRenderer)}.");
        Assert.IsNotNull(_renderer, $"{gameObject.name} - {typeof(HighlightManager)} is null. " +
            $"Please ensure this gameobject is child of a {typeof(HighlightManager)}.");

        _networkedHoverClients = new HashSet<int>();
        _renderer.enabled = HasPointOfInterest;
    }

    private void Start()
    {
        StartCoroutine(InitializeColors());
    }

    public void SetNetworkedHover(int hoverClient, bool hoverActive)
    {
        if(hoverActive)
        {
            _networkedHoverClients.Add(hoverClient);
            if(_renderer.enabled == false)
            {
                _renderer.enabled = true;
                SetHighlightColor(_manager.GetHoverColorByActorNumber(hoverClient));
            }
        }
        else
        {
            _networkedHoverClients.Remove(hoverClient);

            if(_hasLocalHover)
                return;

            if(_networkedHoverClients.Count == 0)
            {
                SetHighlightColor();
                _renderer.enabled = false;
            }
            else
            {
                SetHighlightColor(_manager.GetHoverColorByActorNumber(_networkedHoverClients.First()));
            }
        }
    }

    public void SetPointOfInterest(SO_HotspotData pointOfInterest)
    {
        _pointOfInterest = pointOfInterest;
        SetHighlightColor();
    }

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);

        _hasLocalHover = true;
        _manager.SetHover(this, true);

        if(_networkedHoverClients.Count > 0)
        {
            SetHighlightColor();
        }
        else
        {
            _renderer.enabled = true;
        }
    }

    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);

        //return if other hand is still hovering
        if(args.interactableObject.isHovered)
            return;

        _hasLocalHover = false;
        _manager.SetHover(this, false);

        if(_networkedHoverClients.Count > 0)
        {
            SetHighlightColor(_manager.GetHoverColorByActorNumber(_networkedHoverClients.First()));
        }
        else
        {
            _renderer.enabled = false;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if(PlayerWrapper.Instance.CanManagePointsOfInterest || HasPointOfInterest)
            _manager.SetSelect(this);
    }

    private IEnumerator InitializeColors()
    {
        yield return PhotonRoomInstatiation.Instance.IsConnectedToPhoton;
        SetHighlightColor();
    }

    private void SetHighlightColor()
    {
        if(HasPointOfInterest)
        {
            SetHighlightColor(_manager.GetHoverColorByActorNumber(PlayerWrapper.Instance.GetGlobalSlot()));
        }
        else
        {
            SetHighlightColor(_manager.EmptyColor);
        }
    }

    private void SetHighlightColor(Color highlightColor)
    {
        _renderer.material.color = highlightColor;
        _renderer.material.SetColor("_EmissionColor", highlightColor);
    }
}
