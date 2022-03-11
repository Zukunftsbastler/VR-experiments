using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VR_Experiment.Expo.Product;

namespace VR_Experiment.Menu.Hud
{
    public class BulletPointsMessage : MonoBehaviour
    {
        [SerializeField] private ContentSizeFitter _contentFitter;
        [Space]
        [SerializeField] private Text _title;
        [SerializeField] private RectTransform _container;
        [SerializeField] private VerticalLayoutGroup _containerLayout;
        [Space]
        [SerializeField] private GameObject _bulletPointsPrefab;
        [Space]
        [SerializeField] private InputActionProperty _showNext;

        private Queue<BulletPointUI> _bulletPoints = new Queue<BulletPointUI>();

        private void OnEnable()
        {
            _showNext.action.Enable();
            _showNext.action.performed += OnShowNextInvoked;
        }

        private void OnDisable()
        {
            _showNext.action.performed -= OnShowNextInvoked;
            _showNext.action.Disable();
        }

        public void DisplayBulletPoints(SO_Product product)
        {
            _title.text = product.Name;

            foreach(string line in product.Info)
            {
                BulletPointUI bulletPoint = Instantiate(_bulletPointsPrefab, _container).GetComponent<BulletPointUI>();
                bulletPoint.Text = line;
                _bulletPoints.Enqueue(bulletPoint);
            }

            StartCoroutine(UpdateBulletPointsDelayed());
        }

        private void OnShowNextInvoked(InputAction.CallbackContext callback)
        {
            if(_bulletPoints.Count > 1)
            {
                Destroy(_bulletPoints.Dequeue().gameObject);

                StartCoroutine(UpdateBulletPointsDelayed());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator UpdateBulletPointsDelayed()
        {
            //Wait one frame for the containers layout group to process the new child count
            yield return new WaitForEndOfFrame();

            foreach(BulletPointUI bulletPoint in _bulletPoints)
            {
                bulletPoint.UpdateVisuals();
            }

            LayoutRebuilder.MarkLayoutForRebuild(_container);
        }
    }
}
