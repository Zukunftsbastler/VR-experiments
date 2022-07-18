using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Core;

namespace VR_Experiment.Menu.UI.Core
{
    public class DistanceSettingsUI : MonoBehaviour
    {
        [SerializeField] private Button _distanceButton;

        [Header("Distance Settings")]
        [SerializeField] private GameObject _distanceContainer;
        [SerializeField] private Toggle _offToggle;
        [SerializeField] private Toggle _hudToggle;
        [SerializeField] private Toggle _worldToggle;

        private void Start()
        {
            _distanceContainer.SetActive(false);
        }

        private void OnEnable()
        {
            _distanceButton.gameObject.SetActive(PlayerWrapper.Instance.GetLocalRole() == Role.Experimenter);

            _distanceButton.onClick.AddListener(OnDistanceButtonClick);

            _offToggle.onValueChanged.AddListener(OnOffToggleChange);
            _hudToggle.onValueChanged.AddListener(OnHudToggleChange);
            _worldToggle.onValueChanged.AddListener(OnWorldToggleChange);
        }

        private void OnDisable()
        {
            _distanceButton.onClick.RemoveListener(OnDistanceButtonClick);

            _offToggle.onValueChanged.RemoveListener(OnOffToggleChange);
            _hudToggle.onValueChanged.RemoveListener(OnHudToggleChange);
            _worldToggle.onValueChanged.RemoveListener(OnWorldToggleChange);
        }

        private void OnDistanceButtonClick()
        {
            _distanceContainer.SetActive(!_distanceContainer.activeSelf);
        }

        private void OnOffToggleChange(bool value)
        {
            if(!value)
                return;

            DistanceVisualizationManager.Instance.DisableDistanceDisplay();
            OnDistanceButtonClick();
        }

        private void OnHudToggleChange(bool value)
        {
            if(!value)
                return;

            DistanceVisualizationManager.Instance.EnableHudDistanceDisplay();
            OnDistanceButtonClick();
        }

        private void OnWorldToggleChange(bool value)
        {
            if(!value)
                return;

            DistanceVisualizationManager.Instance.EnableWorldDistanceDisplay();
            OnDistanceButtonClick();
        }

    }
}
