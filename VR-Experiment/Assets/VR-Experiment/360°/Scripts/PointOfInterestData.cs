using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VR_Experiment.Core;

[Serializable]
public abstract class PointOfInterestData
{
    public Vector3 position;
    [NonSerialized] public PointOfInterest pointOfInterest;
    public abstract ScriptableListItem ListItem { get; set; }
}

[Serializable]
public class DirectionData : PointOfInterestData
{
    [SerializeField] private SO_DisplayData _targetDisplay;
    public override ScriptableListItem ListItem
    {
        get
        {
            return _targetDisplay;
        }

        set
        {
            _targetDisplay = value as SO_DisplayData;
            pointOfInterest.Data = this;
        }
    }
}

[Serializable]
public class HotspotData : PointOfInterestData
{
    [SerializeField] private SO_HotspotData _hotspotData;
    public override ScriptableListItem ListItem
    {
        get
        {
            return _hotspotData;
        }

        set
        {
            _hotspotData = value as SO_HotspotData;
            pointOfInterest.Data = this;
        }
    }
}
