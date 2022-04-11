using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Core;

[CreateAssetMenu(fileName = "SO_DisplayData", menuName = "VR-Experiment/360°/DisplayData", order = 0)]
public class SO_DisplayData : ScriptableListItem
{
    [HideInInspector] public TourDisplay tourDisplay;

    public bool isStartDisplay = false;
    public List<DirectionData> directions = new List<DirectionData>();
    public List<HotspotData> hotspots = new List<HotspotData>();
}
