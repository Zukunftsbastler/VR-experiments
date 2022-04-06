using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Core;

[RequireComponent(typeof(PhotonView))]
public class TourManager : MonoBehaviourPun
{
    [SerializeField] private GameObject _displayPrefab;
    [Space]
    [SerializeField] private SO_DisplayData _startDisplay;
    [SerializeField] private List<SO_DisplayData> _tourData;
    [Space]
    [SerializeField] private TourInformationUI _tourInformation;

    private TourDisplay _activeDisplay;
    private PointOfInterestData _activePoIData;

    private void Awake()
    {
        InstantiateTourDisplays();
    }

    public void DisplayPointOfInteresstData(PointOfInterestData activeData)
    {
        _activePoIData = activeData;

        if(_activePoIData is DirectionData directionData)
        {
            _tourInformation.ShowPointOfInterestData(directionData, _tourData.Cast<ScriptableListItem>().ToList());
        }

        //TODO: Add case for HotspotData
    }

    public void SetPoIData(bool active, string dataName)
    {
        photonView.RPC(nameof(RPC_SetPointOfInterestData), RpcTarget.AllBuffered, active, dataName);
    }

    [PunRPC]
    private void RPC_SetPointOfInterestData(bool active, string dataName)
    {
        if(_activePoIData is DirectionData directionData)
        {
            if(active)
            {
                directionData.targetDisplay = _tourData.FirstOrDefault(data => data.Name.Equals(dataName));
            }
            else
            {
                _activeDisplay.Data.directions.Remove(directionData);
                Destroy(directionData.pointOfInterest.gameObject);

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(_activeDisplay.Data);
#endif
            }
        }

        //TODO: Add case for HotspotData
        //if(_activePoIData is HotspotData hotspotData)
        //{
        //    //Take data from hotspot list.
        //}
    }

    public void ChangeDisplayScene()
    {
        photonView.RPC(nameof(RPC_ChangeDisplayScene), RpcTarget.AllBuffered, _activePoIData.ListItem.Name);
    }

    [PunRPC]
    private void RPC_ChangeDisplayScene(string dataName)
    {
        DirectionData directionData = _activeDisplay.Data.directions.FirstOrDefault(direction => direction.targetDisplay.Name.Equals(dataName));

        if(directionData == null)
        {
            Debug.LogError($"{gameObject.name} tring to switch display, with no selected {nameof(DirectionData)}.");
            return;
        }

        _activeDisplay.gameObject.SetActive(false);
        _activeDisplay = directionData.targetDisplay.tourDisplay;
        _activeDisplay.gameObject.SetActive(true);
    }

    public void AddDirection(Vector3 position)
    {
        photonView.RPC(nameof(RPC_AddDirectionPointOfInterest), RpcTarget.AllBuffered, position);
    }

    [PunRPC]
    private void RPC_AddDirectionPointOfInterest(Vector3 position)
    {
        DirectionData directionToAdd = new DirectionData();
        directionToAdd.position = _activeDisplay.transform.InverseTransformPoint(position);
        directionToAdd.pointOfInterest = _activeDisplay.SpawnPointOfInterest(directionToAdd, this);

        _activeDisplay.Data.directions.Add(directionToAdd);
        _activePoIData = directionToAdd;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_activeDisplay.Data);
#endif
    }

    private void InstantiateTourDisplays()
    {
        foreach(SO_DisplayData displayData in _tourData)
        {
            TourDisplay display = Instantiate(_displayPrefab, transform).GetComponent<TourDisplay>();

            bool active = displayData.Name.Equals(_startDisplay.Name);
            display.gameObject.SetActive(active);
            display.Initialize(displayData, this);

            if(active)
            {
                _activeDisplay = display;
            }
        }
    }
}
