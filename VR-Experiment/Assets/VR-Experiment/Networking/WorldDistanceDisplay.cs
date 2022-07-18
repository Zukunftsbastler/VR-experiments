using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VR_Experiment.Core
{
    public class WorldDistanceDisplay : MonoBehaviour
    {
        private const float MIN_DIST = 0f;
        private const float MAX_DIST = 20f;

        [SerializeField] private LineRenderer _line;
        [SerializeField] private float _lineCorrection;
        [SerializeField] private Transform _canvas;
        [SerializeField] private float _canvasCorrection;
        [SerializeField] private float _canvasMinScale;
        [SerializeField] private float _canvasMaxScale;
        [Space]
        [SerializeField] private TextMeshProUGUI _distanceText;

        private Transform _playerA;
        private Transform _playerB;
        private DistanceVisualizationManager.PlayerDistances _playerDistance;
        private Transform _xrHead;

        private void Update()
        {
            Vector3 playerA = _playerA.position;
            playerA.y = 0;
            Vector3 playerB = _playerB.position;
            playerB.y = 0;

            //update line positions
            Vector3 lineHightCorrection = Vector3.up * _lineCorrection;
            _line.SetPosition(0, playerA + lineHightCorrection);
            _line.SetPosition(1, playerB + lineHightCorrection);

            //update canvas position and rotation
            Vector3 _canvasHeightCorrection = Vector3.up * _canvasCorrection;
            _canvas.position = playerA + ((playerB - playerA) / 2) + _canvasHeightCorrection;
            float t = Mathf.InverseLerp(MIN_DIST, MAX_DIST, _playerDistance.distance);
            float scale = Mathf.Lerp(_canvasMinScale, _canvasMaxScale, t);
            _canvas.localScale.Set(scale, scale, scale);
            _canvas.forward = _xrHead.forward;

            //update text
            _distanceText.text = $"{_playerDistance.distance:0.00}";
        }

        public void Setup(DistanceVisualizationManager.PlayerDistances playerDistance, Transform playerA, Transform playerB, Transform xrHead, Avatar.Core.SO_PlayerColor colors)
        {
            _playerDistance = playerDistance;
            _playerA = playerA;
            _playerB = playerB;
            _xrHead = xrHead;

            _line.startColor = colors.GetPlayerColorByActorNumber(playerDistance.playerA);
            _line.endColor = colors.GetPlayerColorByActorNumber(playerDistance.playerB);
        }
    }
}
