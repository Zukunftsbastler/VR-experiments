using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Avatar;
using VR_Experiment.Core;
using VR_Experiment.Expo.Product;
using VR_Experiment.Networking;

namespace VR_Experiment.Menu.UI.Core
{
    public class AvatarInfoUI : MonoBehaviourPun, IInRoomCallbacks
    {
        [Header("Connection")]
        [SerializeField] private Transform _target;
        [SerializeField] private UpdateType _updateVisualization = UpdateType.UpdateAndBeforRender;
        [SerializeField] private LineRenderer _visualization;
        [SerializeField, Range(0f, 15f)] private float _range = 5f;
        [Space]
        [SerializeField] private TransformFollow _uiFollow;
        [SerializeField] private AvatarSoundSettings _avatarSoundSettings;
        [Header("Default View")]
        [SerializeField] private GameObject _defaultContainer;
        [SerializeField] private Text _title;
        [SerializeField] private Text _role;
        [SerializeField] private Text _product;
        [SerializeField] private Button _productInfoButton;
        [SerializeField] private Button _voiceChatButton;
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
        [SerializeField] private Button _saveSoundButton;
        [SerializeField] private Button _cancelSoundButton;

        private bool _isActive;
        private VoiceChatSettings _startSettings;

        private bool TargetOutOfRange => Vector3.Distance(transform.position, _target.position) > _range;

        private void OnEnable()
        {
            _voiceChatButton.gameObject.SetActive(PlayerWrapper.Instance.GetLocalRole() == Role.Experimenter);
            
            _productInfoButton.onClick.AddListener(DisplayProductInfo);
            _voiceChatButton.onClick.AddListener(DisplayVoiceChatSettings);

            _volumeSlider.onValueChanged.AddListener(OnVolumeChange);
            _pitchSlider.onValueChanged.AddListener(OnPitchChange);
            _spatialBlendSlider.onValueChanged.AddListener(OnSpatialBlendChange);
            _distanceSlider.onValueChanged.AddListener(OnDistanceChange);
            _saveSoundButton.onClick.AddListener(SaveSoundChanges);
            _cancelSoundButton.onClick.AddListener(DiscardSoundChanges);

            Application.onBeforeRender += OnBeforRender;
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            _productInfoButton.onClick.RemoveListener(DisplayProductInfo);
            _voiceChatButton.onClick.RemoveListener(DisplayVoiceChatSettings);

            _volumeSlider.onValueChanged.RemoveListener(OnVolumeChange);
            _pitchSlider.onValueChanged.RemoveListener(OnPitchChange);
            _spatialBlendSlider.onValueChanged.RemoveListener(OnSpatialBlendChange);
            _distanceSlider.onValueChanged.RemoveListener(OnDistanceChange);
            _saveSoundButton.onClick.RemoveListener(SaveSoundChanges);
            _cancelSoundButton.onClick.RemoveListener(DiscardSoundChanges);

            Application.onBeforeRender -= OnBeforRender;
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void FixedUpdate()
        {
            if(_isActive && _updateVisualization != UpdateType.BeforRender)
            {
                UpdateVisualization();
            }
        }

        /// <summary>
        /// Enables or disables the UI.
        /// </summary>
        public void ToggleUI()
        {
            _isActive = !gameObject.activeSelf;

            _visualization.enabled = _isActive;
            gameObject.SetActive(_isActive);

            if(_isActive)
            {
                transform.position = _target.position;
                _uiFollow.followTarget = PlayerWrapper.Instance.Rig.Head;

                _productInfoButton.gameObject.SetActive(PlayerWrapper.Instance.CanManageProducts);

                SetInfoUI();
            }
            else
            {
                if(_soundContainer.activeSelf)
                {
                    DiscardSoundChanges();
                }
            }
        }

        private void DisplayProductInfo()
        {
            string productName = PlayerWrapper.GetActiveProduct(photonView.Owner);
            SO_Product product = Inventory.GetProductByName(productName);

            PlayerWrapper.Instance.Hud.DisplayProductBulletPoints(product);
        }

        private void DisplayVoiceChatSettings()
        {
            _defaultContainer.SetActive(false);
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

        private void SaveSoundChanges()
        {
            VoiceChatManager.Instance.SyncVoiceSettings(photonView.Owner, _avatarSoundSettings.GetSettings());

            _soundContainer.SetActive(false);
            _defaultContainer.SetActive(true);
        }

        private void DiscardSoundChanges()
        {
            _avatarSoundSettings.SetSettings(_startSettings);

            _soundContainer.SetActive(false);
            _defaultContainer.SetActive(true);
        }

        private void UpdateVisualization()
        {
            _visualization.SetPosition(0, transform.position);
            _visualization.SetPosition(1, _target.position);

            if(TargetOutOfRange)
                ToggleUI();
        }

        private void SetInfoUI()
        {
            int actorNumber = photonView.OwnerActorNr;
            _title.text = $"Client {actorNumber} info:";

            Role role = PlayerWrapper.GetRole(photonView.Owner);
            _role.text = $"Role: '{role}'";

            string productName = PlayerWrapper.GetActiveProduct(photonView.Owner);
            _product.text = $"Product: '{productName}'";

            _productInfoButton.interactable = PlayerWrapper.Instance.HasActiveProduct;
        }

        private void OnBeforRender()
        {
            if(_isActive && _updateVisualization != UpdateType.Update)
            {
                UpdateVisualization();
            }
        }

        void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer) { }

        void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer) { }

        void IInRoomCallbacks.OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }

        void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if(_isActive && photonView.Owner.Equals(targetPlayer))
            {
                SetInfoUI();
            }
        }

        void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient) { }
    }
}
