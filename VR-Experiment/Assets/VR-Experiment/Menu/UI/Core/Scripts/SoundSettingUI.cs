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

        private VoiceChatSettings _startSettings;
        private AvatarSoundSettings _avatarSoundSettings;

        private void Start()
        {
            _soundContainer.SetActive(false);
        }

        private void OnEnable()
        {
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
            _volumeSlider.value = _startSettings.volume;
            _pitchValue.text = $"{_startSettings.pitch}";
            _pitchSlider.value = _startSettings.pitch;
            _spatialBlendValue.text = $"{_startSettings.spatialBlend}";
            _spatialBlendSlider.value = _startSettings.spatialBlend;
            _distanceValue.text = $"{_startSettings.distance}";
            _distanceSlider.value = _startSettings.distance;
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
            _volumeValue.text = $"{value:0.00}";
            _avatarSoundSettings.SetVolume(value);
        }

        private void OnPitchChange(float value)
        {
            _pitchValue.text = $"{value:0.00}";
            _avatarSoundSettings.SetPitch(value);
        }

        private void OnSpatialBlendChange(float value)
        {
            _spatialBlendValue.text = $"{value:0.00}";
            _avatarSoundSettings.SetSpatialBlend(value);
        }

        private void OnDistanceChange(float value)
        {
            _distanceValue.text = $"{value:0}";
            _avatarSoundSettings.SetMinDistance(value);
        }
    }
}
