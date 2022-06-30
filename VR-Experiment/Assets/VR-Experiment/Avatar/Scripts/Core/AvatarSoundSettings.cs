using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using VR_Experiment.Core;
using VR_Experiment.Networking;

namespace VR_Experiment.Avatar
{
    public class AvatarSoundSettings : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private AudioSource _speaker;

        private void Awake()
        {
            if(_speaker == null)
            {
                this.TryGetComponentInChildren(out _speaker, true);
                Assert.IsNotNull(_speaker, $"{typeof(AvatarSoundSettings)} needs a {typeof(AudioSource)} to work. Please add a {typeof(AudioSource)} to {gameObject.name}.");
            }
        }

        private void OnEnable()
        {
            VoiceChatManager.Instance.Register(_photonView.Owner, this);
        }

        private void OnDisable()
        {
            VoiceChatManager.Instance.Unregister(_photonView.Owner);
        }

        public void SetSettings(VoiceChatSettings settings)
        {
            _speaker.volume = settings.volume;
            _speaker.pitch = settings.pitch;
            _speaker.spatialBlend = settings.spatialBlend;
            _speaker.minDistance = settings.distance;
        }

        public void SetVolume(float volume)
        {
            _speaker.volume = volume;
        }

        public void SetPitch(float pitch)
        {
            _speaker.pitch = pitch;
        }

        public void SetSpatialBlend(float spatialBlend)
        {
            _speaker.spatialBlend = spatialBlend;
        }

        public void SetMinDistance(float minDistance)
        {
            _speaker.minDistance = minDistance;
        }

        public VoiceChatSettings GetSettings()
        {
            return new VoiceChatSettings()
            {
                volume = _speaker.volume,
                pitch = _speaker.pitch,
                spatialBlend = _speaker.spatialBlend,
                distance = _speaker.minDistance
            };
        }
    }
}
