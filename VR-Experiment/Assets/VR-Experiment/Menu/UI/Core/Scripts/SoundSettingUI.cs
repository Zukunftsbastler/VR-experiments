using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Avatar;
using VR_Experiment.Core;
using VR_Experiment.Networking;

namespace VR_Experiment.Menu.UI.Core
{
    public class SoundSettingUI : MonoBehaviour
    {
        [SerializeField] private bool _useFixedSettings;
        [SerializeField] private AvatarSoundSettings _avatarSoundSettings;
        [Space]
        [SerializeField] private Button _voiceChatButton;
        [SerializeField] private Button _saveSoundButton;
        [SerializeField] private Button _cancelSoundButton;

        [Header("Sound Settings")]
        [SerializeField] private GameObject _soundContainer;
        [SerializeField] private TextMeshProUGUI _volumeValue;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private TextMeshProUGUI _pitchValue;
        [SerializeField] private Slider _pitchSlider;
        [SerializeField] private TextMeshProUGUI _spatialBlendValue;
        [SerializeField] private Slider _spatialBlendSlider;
        [SerializeField] private TextMeshProUGUI _distanceValue;
        [SerializeField] private Slider _distanceSlider;

        private const float VALUE_MULTIPLIER = 0.1f;
        private const float INVERSE_VALUE_MULTIPLIER = 10f;
        private const float DIST_MULTIPLIER = 0.5f;
        private const float INVERS_DIST_MULTIPLIER = 2f;

        private VoiceChatSettings _startSettings;

        private void Start()
        {
            _soundContainer.SetActive(false);
        }

        private void OnEnable()
        {
            if(!_useFixedSettings)
                StartCoroutine(SetUpAvatarSettings());

            _voiceChatButton.gameObject.SetActive(PlayerWrapper.Instance.GetLocalRole() == Role.Experimenter);

            _voiceChatButton.onClick.AddListener(DisplayVoiceChatSettings);
            _saveSoundButton.onClick.AddListener(SaveSoundChanges);
            _cancelSoundButton.onClick.AddListener(DiscardSoundChanges);

            _volumeSlider.onValueChanged.AddListener(OnVolumeChange);
            _pitchSlider.onValueChanged.AddListener(OnPitchChange);
            _spatialBlendSlider.onValueChanged.AddListener(OnSpatialBlendChange);
            _distanceSlider.onValueChanged.AddListener(OnDistanceChange);
        }

        private void OnDisable()
        {
            _voiceChatButton.onClick.RemoveListener(DisplayVoiceChatSettings);
            _saveSoundButton.onClick.RemoveListener(SaveSoundChanges);
            _cancelSoundButton.onClick.RemoveListener(DiscardSoundChanges);

            _volumeSlider.onValueChanged.RemoveListener(OnVolumeChange);
            _pitchSlider.onValueChanged.RemoveListener(OnPitchChange);
            _spatialBlendSlider.onValueChanged.RemoveListener(OnSpatialBlendChange);
            _distanceSlider.onValueChanged.RemoveListener(OnDistanceChange);
        }

        private IEnumerator SetUpAvatarSettings()
        {
            yield return PhotonRoomInstatiation.Instance.IsConnectedToPhoton;

            _avatarSoundSettings = VoiceChatManager.Instance.GetAvatarSettings(PlayerWrapper.Instance.Player);
        }

        private void DisplayVoiceChatSettings()
        {
            _soundContainer.SetActive(true);

            _startSettings = _avatarSoundSettings.GetSettings();
            _volumeValue.text = $"{_startSettings.volume}";
            _volumeSlider.value = _startSettings.volume * INVERSE_VALUE_MULTIPLIER;
            _pitchValue.text = $"{_startSettings.pitch}";
            _pitchSlider.value = _startSettings.pitch * INVERSE_VALUE_MULTIPLIER;
            _spatialBlendValue.text = $"{_startSettings.spatialBlend}";
            _spatialBlendSlider.value = _startSettings.spatialBlend * INVERSE_VALUE_MULTIPLIER;
            _distanceValue.text = $"{_startSettings.distance}";
            _distanceSlider.value = _startSettings.distance * INVERS_DIST_MULTIPLIER;
        }

        private void SaveSoundChanges()
        {
            VoiceChatManager.Instance.SyncVoiceSettings(PlayerWrapper.Instance.Player, _avatarSoundSettings.GetSettings());
            _soundContainer.SetActive(false);
        }

        private void DiscardSoundChanges()
        {
            _avatarSoundSettings.SetSettings(_startSettings);
            _soundContainer.SetActive(false);
        }

        private void OnVolumeChange(float value)
        {
            float multipliedValue = value * VALUE_MULTIPLIER;
            _volumeValue.text = $"{multipliedValue:0.00}";
            _avatarSoundSettings.SetVolume(multipliedValue);
        }

        private void OnPitchChange(float value)
        {
            float multipliedValue = value * VALUE_MULTIPLIER;
            _pitchValue.text = $"{multipliedValue:0.00}";
            _avatarSoundSettings.SetPitch(multipliedValue);
        }

        private void OnSpatialBlendChange(float value)
        {
            float multipliedValue = value * VALUE_MULTIPLIER;
            _spatialBlendValue.text = $"{multipliedValue:0.00}";
            _avatarSoundSettings.SetSpatialBlend(multipliedValue);
        }

        private void OnDistanceChange(float value)
        {
            float multipliedValue = value * DIST_MULTIPLIER;
            _distanceValue.text = $"{multipliedValue:0.0}";
            _avatarSoundSettings.SetMinDistance(multipliedValue);
        }
    }
}
