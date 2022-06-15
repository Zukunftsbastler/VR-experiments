using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Core;

[RequireComponent(typeof(PhotonView))]
public class TourManager : MonoBehaviourPun
{
    [SerializeField] private SO_TourData _tourData;
    [SerializeField] private Transform _tourWrapper;
    [SerializeField] private GameObject _displayPrefab;
    [SerializeField] private TourInformationUI _tourInformation;

    private TourDisplay _activeDisplay;
    private PointOfInterestData _activePoIData;
    private TourData _activeTour;

    private void Awake()
    {
        InstantiateTourDisplays();
    }

    public void DisplayPointOfInteresstData(PointOfInterestData activeData)
    {
        _activePoIData = activeData;

        if(_activePoIData is DirectionData directionData)
        {
            _tourInformation.ShowPointOfInterestData(directionData, _activeTour.displayData.Cast<ScriptableListItem>().ToList());
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
        
        if(active)
        {
            if(_activePoIData is DirectionData directionData)
            {
                directionData.ListItem = _activeTour.displayData.FirstOrDefault(data => data.Name.Equals(dataName));
            }

            //TODO: Add case for HotspotData
            //if(_activePoIData is HotspotData hotspotData)
            //{
            //    //Take data from hotspot list.
            //}
        }
        else
        {
            DestroyPointOfInterest(_activePoIData);
        }
    }

    public void DestroyPointOfInterest(PointOfInterestData pointOfInterestData)
    {
        if(pointOfInterestData is DirectionData directionData)
        {
            _activeDisplay.Data.directions.Remove(directionData);
            Destroy(directionData.pointOfInterest.gameObject);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_activeDisplay.Data);
#endif
        }

        //TODO: Add case for HotspotData
        //if(_activePoIData is HotspotData hotspotData)
        //{
        //    //Take data from hotspot list.
        //}
    }

    public void ChangeDisplayScene()
    {
        ChangeDisplayScene(_activePoIData);
    }

    public void ChangeDisplayScene(PointOfInterestData pointOfInterestData)
    {
        photonView.RPC(nameof(RPC_ChangeDisplayScene), RpcTarget.AllBuffered, pointOfInterestData.ListItem.Name);
    }

    [PunRPC]
    private void RPC_ChangeDisplayScene(string dataName)
    {
        DirectionData directionData = _activeDisplay.Data.directions.FirstOrDefault(direction => direction.ListItem.Name.Equals(dataName));

        if(directionData == null)
        {
            Debug.LogError($"{gameObject.name} tring to switch display, with no selected {nameof(DirectionData)}.");
            return;
        }

        _activeDisplay.gameObject.SetActive(false);
        _activeDisplay = (directionData.ListItem as SO_DisplayData).tourDisplay;
        _activeDisplay.gameObject.SetActive(true);
    }

    public void AddDirection(Vector3 position)
    {
        Vector3 inversePosition = _activeDisplay.transform.InverseTransformPoint(position);
        photonView.RPC(nameof(RPC_AddDirectionPointOfInterest), RpcTarget.AllBuffered, inversePosition);
    }

    [PunRPC]
    private void RPC_AddDirectionPointOfInterest(Vector3 inversePosition)
    {
        DirectionData directionToAdd = new DirectionData();
        directionToAdd.position = inversePosition;
        directionToAdd.pointOfInterest = _activeDisplay.SpawnPointOfInterest(directionToAdd, this);

        _activeDisplay.Data.directions.Add(directionToAdd);
        _activePoIData = directionToAdd;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_activeDisplay.Data);
#endif
    }

    private void InstantiateTourDisplays()
    {
        _activeTour = _tourData.ActiveTour;

        foreach(SO_DisplayData displayData in _activeTour.displayData)
        {
            TourDisplay display = Instantiate(_displayPrefab, _tourWrapper).GetComponent<TourDisplay>();
            display.Initialize(displayData, this);

            bool active = _activeTour.HasStartDisplay ? displayData.isStartDisplay : display.transform.childCount == 0;
            if(active)
            {
                _activeDisplay = display;
            }
            display.gameObject.SetActive(active);
        }
    }
}
