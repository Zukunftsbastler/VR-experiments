using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VR_Experiment.Menu.Hud
{
    public class DistanceDisplayContentWrapper : MonoBehaviour
    {
        [SerializeField] private Image _imagePrefab;
        [SerializeField] private DistanceDisplayContentTextUpdater _textPrefab;

        public void AddDistance(Core.DistanceVisualizationManager.PlayerDistances playerDistance)
        {
            DistanceDisplayContentTextUpdater text = Instantiate(_textPrefab, transform);
            text.PlayerDistance = playerDistance;
        }

        public void AddColor(Color color)
        {
            Image image = Instantiate(_imagePrefab, transform);
            image.color = color;
        }
    }
}
