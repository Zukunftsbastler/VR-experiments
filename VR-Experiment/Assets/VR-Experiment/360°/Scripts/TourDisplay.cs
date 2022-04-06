using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;

public class TourDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _highlightPrefab;
    [Space]
    [SerializeField] private MeshRenderer _displayRenderer;

    private SO_DisplayData _displayData;
    //private TourManager _manager;

    public SO_DisplayData Data => _displayData;

    private void Awake()
    {
        //if(this.TryGetComponentInParent(out _manager) == false)
        //{
        //    Debug.LogWarning($"{gameObject.name} - ensure this gameObject is child of a {typeof(TourManager)}.", this);
        //    return;
        //}
    }

    public void Initialize( SO_DisplayData displayData, TourManager tourManager)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _displayData = displayData;
        _displayData.tourDisplay = this;

        Display(_displayData.Preview);
        InstantiateDirections(tourManager);
        InstantiateHotspots(tourManager);
    }

    public PointOfInterest SpawnPointOfInterest(PointOfInterestData data, TourManager tourManager)
    {
        PointOfInterest pointOfInterest = Instantiate(_highlightPrefab, transform).GetComponent<PointOfInterest>();

        Vector3 pos = transform.TransformPoint(data.position);
        Vector3 dir = (transform.position - pos).normalized;
        pointOfInterest.transform.position = pos + dir * 0.5f;
        pointOfInterest.transform.LookAt(this.transform.position);

        pointOfInterest.Data = data;
        pointOfInterest.TourManager = tourManager;

        return pointOfInterest;
    }

    private void Display(Sprite preview)
    {
        _displayRenderer.material.SetTexture("_mainTexture", preview.texture);
    }

    private void InstantiateDirections(TourManager tourManager)
    {
        if(_displayData.directions == null || _displayData.directions.Count == 0)
            return;

        foreach(DirectionData direction in _displayData.directions)
        {
            direction.pointOfInterest = SpawnPointOfInterest(direction, tourManager);
        }
    }

    private void InstantiateHotspots(TourManager tourManager)
    {
        if(_displayData.hotspots == null || _displayData.hotspots.Count == 0)
            return;

        foreach(HotspotData hotspot in _displayData.hotspots)
        {
            hotspot.pointOfInterest = SpawnPointOfInterest(hotspot, tourManager);
            hotspot.pointOfInterest.TourManager = tourManager;
        }
    }
}
