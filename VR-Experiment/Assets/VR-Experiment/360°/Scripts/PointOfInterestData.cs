using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;

[Serializable]
public abstract class PointOfInterestData
{
    public Vector3 position;
    [NonSerialized] public PointOfInterest pointOfInterest;
    public abstract ScriptableListItem ListItem { get; }
}

[Serializable]
public class DirectionData : PointOfInterestData
{
    public SO_DisplayData targetDisplay;
    public override ScriptableListItem ListItem => targetDisplay;
}

[Serializable]
public class HotspotData : PointOfInterestData
{
    public SO_HotspotData hotspotData;
    public override ScriptableListItem ListItem => hotspotData;
}
