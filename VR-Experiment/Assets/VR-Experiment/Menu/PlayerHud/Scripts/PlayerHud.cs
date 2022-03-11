using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Expo.Product;

namespace VR_Experiment.Menu.Hud
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private GameObject _popUpPrefab;
        [SerializeField] private GameObject _pulletPointsPrefab;

        private BulletPointsMessage _message;

        public void DisplayProductNotification(int actorNumber, string interaction, SO_Product product)
        {
            string title = $"Client {actorNumber} {interaction} '{product.Name}'";

            PopUpMessage popUp = Instantiate(_popUpPrefab, transform).GetComponent<PopUpMessage>();
            popUp.DisplayMessage(null, title, product.Info[0]);
        }

        public void DisplayProductBulletPoints(SO_Product product)
        {
            if(_message != null || product == null)
                return;

            _message = Instantiate(_pulletPointsPrefab, transform).GetComponent<BulletPointsMessage>();
            _message.DisplayBulletPoints(product);
        }
    }
}
