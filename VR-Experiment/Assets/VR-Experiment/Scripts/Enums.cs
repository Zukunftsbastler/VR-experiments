using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR_Experiment.Enums
{
    /// <summary>
    /// The Roles for all Users.
    /// </summary>
    public enum Role : byte
    {
        //TODO: Discuss Godmode and its implementation.
        None = 0,
        Visitor = 1,
        Presenter = 2,
        Experimenter = 3,
    }

    public enum UpdateType
    {
        UpdateAndBeforRender,
        Update,
        BeforRender
    }
}