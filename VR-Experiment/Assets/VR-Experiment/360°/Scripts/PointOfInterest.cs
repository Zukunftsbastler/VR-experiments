using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PointOfInterest : XRBaseInteractable
{
    public PointOfInterestData Data { get; set; }
    public TourManager TourManager { get; set; }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        TourManager.DisplayPointOfInteresstData(Data);
    }
}
