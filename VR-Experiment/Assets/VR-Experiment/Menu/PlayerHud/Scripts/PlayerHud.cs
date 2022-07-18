using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Expo.Product;

namespace VR_Experiment.Menu.Hud
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private PopUpMessage _popUpPrefab;
        [SerializeField] private BulletPointsMessage _pulletPointsPrefab;
        [SerializeField] private HudDistanceDisplay _distanceDisplayPrefab;

        private BulletPointsMessage _message;
        private HudDistanceDisplay _distanceDisplay;

        /// <summary>
        /// Spawning <see cref="PopUpMessage"/>
        /// </summary>
        /// <param name="actorNumber"></param>
        /// <param name="interaction"></param>
        /// <param name="product"></param>
        public void DisplayProductNotification(int actorNumber, string interaction, SO_Product product)
        {
            string title = $"Client {actorNumber} {interaction} '{product.Name}'";

            PopUpMessage popUp = Instantiate(_popUpPrefab, transform);
            popUp.DisplayMessage(null, title, product.Info[0]);
        }

        /// <summary>
        /// Spawning <see cref="BulletPointsMessage"/>. 
        /// </summary>
        /// <param name="product"></param>
        public void DisplayProductBulletPoints(SO_Product product)
        {
            if(_message != null || product == null)
                return;

            _message = Instantiate(_pulletPointsPrefab, transform);
            _message.DisplayBulletPoints(product);
        }

        /// <summary>
        /// Spawning <see cref="HudDistanceDisplay"/>. 
        /// </summary>
        /// <param name="product"></param>
        public void DisplayDistances(List<Core.DistanceVisualizationManager.PlayerDistances> distances, List<int> players, Avatar.Core.SO_PlayerColor colors)
        {
            if(_distanceDisplay != null)
                return;

            _distanceDisplay = Instantiate(_distanceDisplayPrefab, transform);
            _distanceDisplay.SetupDistanceDisplay(distances, players, colors);
        }

        /// <summary>
        /// Destroying <see cref="HudDistanceDisplay"/>. 
        /// </summary>
        public void RemoveDistanceDisplay()
        {
            if(_distanceDisplay == null)
                return;

            Destroy(_distanceDisplay.gameObject);
        }
    }
}
