using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VR_Experiment.Menu.Hud
{
    public class DistanceDisplayContentTextUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public Core.DistanceVisualizationManager.PlayerDistances PlayerDistance { get; set; }

        void Update()
        {
            if(PlayerDistance == null)
            {
                _text.text = "";
            }
            else
            {
                _text.text = $"{PlayerDistance.distance:0.00}";
            }
        }
    }
}
