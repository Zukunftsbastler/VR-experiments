using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VR_Experiment.Menu.Hud
{
    public class PopUpMessage : MonoBehaviour
    {
        [SerializeField] private Image _avatarPreview;
        [SerializeField] private Text _title;
        [SerializeField] private Text _message;
        [Space]
        [SerializeField] private float _upTime;

        private void Start()
        {
            StartCoroutine(DestroyDelayed());
        }

        private IEnumerator DestroyDelayed()
        {
            yield return new WaitForSeconds(_upTime);

            Destroy(gameObject);
        }

        /// <summary>
        /// Setting the content of the message.
        /// </summary>
        /// <param name="avatarPreview"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void DisplayMessage(Sprite avatarPreview, string title, string message)
        {
            _avatarPreview.sprite = avatarPreview;
            _title.text = title;
            _message.text = message;
        }
    }
}