using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using VR_Experiment.Core;
using VR_Experiment.Expo.Product;
using VR_Experiment.Menu.UI.Core;
using VR_Experiment.Networking;

namespace VR_Experiment.Expo.Booth
{
    [RequireComponent(typeof(PhotonView))]
    public class RevolvingStageBehaviour : MonoBehaviour, IItemListCallbackListener
    {
        [SerializeField] private PhotonRoomInstatiation _photonRoom;
        [SerializeField] private ScrollableItemListUI _productSelectionUI;
        [SerializeField] private Transform _productAnchor;
        [SerializeField] private float _respawndelay = .5f;
        [Space]
        [Header("Product Animation")]
        [Tooltip("Distance local to this transform.")]
        [SerializeField] private float _hoverHeight;
        [Tooltip("Distance the product hoves up and down relative to the hover height.")]
        [SerializeField] private float _hoverAmplitude;
        [Tooltip("One cycle (up, down up) equals 360. \nSpeed in cycles per second.")]
        [SerializeField] private float _hoverSpeed;
        [Tooltip("One full rotation equals 360. \nSpeed in rotation per seconds.")]
        [SerializeField] private float _rotationSpeed;

        private ProductBehaviour _activeProduct;
        private float _hoverFrequenz;

        private PhotonView _photonView;

        public bool HasActiveItem => _activeProduct != null;
        public ProductBehaviour ActiveProduct
        {
            get
            {
                return _activeProduct;
            }

            set
            {
                _activeProduct = value;

                if(_activeProduct != null)
                    _activeProduct.transformFollow.followTarget = _productAnchor;
            }
        }

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();

            Assert.IsNotNull(_photonView, $"{gameObject.name}'s {nameof(_photonView)} is null. Ensure the gameobject has a PhotonView component.");

            StartCoroutine(SetUp());
        }

        void Update()
        {
            if(HasActiveItem)
            {
                //spin platform
                float yRotation = _productAnchor.eulerAngles.y + _rotationSpeed * Time.deltaTime;
                _productAnchor.rotation = Quaternion.AngleAxis(yRotation, Vector3.up);

                //hover item
                _hoverFrequenz += _hoverSpeed * Mathf.Deg2Rad * Time.deltaTime;
                float hoverHeight = _hoverHeight + _hoverAmplitude * Mathf.Sin(_hoverFrequenz);
                _productAnchor.position = transform.position + Vector3.up * hoverHeight;
            }
        }

        public void SetInventory()
        {
            _productSelectionUI.SetItems(Inventory.Products.Cast<ScriptableListItem>().ToList());
        }

        public void OnItemToggleInvoked(bool isActive, string productName)
        {
            _photonView.RPC(nameof(RPC_OnItemToggleInvoked), RpcTarget.AllBuffered, isActive, productName);
        }

        [PunRPC]
        private void RPC_OnItemToggleInvoked(bool isActive, string productName, PhotonMessageInfo info)
        {
            //Update UI on other clients
            if(info.Sender.Equals(PhotonNetwork.LocalPlayer) == false)
            {
                _productSelectionUI.SetItem(productName);
            }

            //MasterClient handles product spawning
            if(PhotonNetwork.IsMasterClient)
            {
                if(HasActiveItem)
                {
                    ActiveProduct.productGrabbed -= OnProductGrabbed;
                    PhotonNetwork.Destroy(ActiveProduct.gameObject);
                }

                if(isActive)
                {
                    SO_Product productToSpawn = Inventory.GetProductByName(productName);
                    SpawnActiveProduct(productToSpawn);
                }
            }
        }

        private IEnumerator SetUp()
        {
            yield return _photonRoom.IsConnectedToPhoton;

            if(PlayerWrapper.Instance.CanManageProducts)
            {
                _productSelectionUI.SetCallbackListener(this);
            }
            else
            {
                _productSelectionUI.gameObject.SetActive(false);
            }
        }

        private IEnumerator RecreateActiveProductRoutine(string productName)
        {
            yield return new WaitForSeconds(_respawndelay);

            SO_Product productToSpawn = Inventory.GetProductByName(productName);
            SpawnActiveProduct(productToSpawn);
        }

        private void SpawnActiveProduct(SO_Product product)
        {
            if(product != null)
            {
                object[] instantiationData = new object[]
                {
                product.Name
                };

                GameObject go = PhotonNetwork.Instantiate(product.Id, _productAnchor.position, Quaternion.identity, data: instantiationData);
                ActiveProduct = go.GetComponent<ProductBehaviour>();
                ActiveProduct.productGrabbed += OnProductGrabbed;
            }
            else
            {
                _activeProduct = null;
            }
        }

        private void OnProductGrabbed(string productId)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //Unsubscribe from previous product
                ActiveProduct.productGrabbed -= OnProductGrabbed;

                //spawn new product
                StartCoroutine(RecreateActiveProductRoutine(productId));
            }
        }
    }
}
