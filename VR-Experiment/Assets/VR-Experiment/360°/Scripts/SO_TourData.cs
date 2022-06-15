using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TourData", menuName = "VR-Experiment/360°/TourData", order = 1)]
public class SO_TourData : ScriptableObject
{

    [SerializeField] private string _activeTour;
    public List<TourData> tourData;

    public TourData ActiveTour => tourData.FirstOrDefault(tD => tD.name.Equals(_activeTour));
}

[System.Serializable]
public class TourData
{
    public string name;
    public SO_DisplayData[] displayData;
    public SO_HotspotData[] hotspotData;

    public bool HasStartDisplay => displayData.Any(dD => dD.isStartDisplay);

    public TourData(string name, SO_DisplayData[] displayData, SO_HotspotData[] hotspotData)
    {
        this.name = name;
        this.displayData = displayData;
        this.hotspotData = hotspotData;
    }
}
