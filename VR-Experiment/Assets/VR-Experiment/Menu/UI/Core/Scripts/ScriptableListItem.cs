using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR_Experiment.Core
{
    public class ScriptableListItem : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _preview;

        public string Name => _name;
        public Sprite Preview => _preview;
    }
}
