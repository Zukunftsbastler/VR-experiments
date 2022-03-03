using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RevolvingStageBehaviour : MonoBehaviour, IInventoryCallbackListener
{
    [SerializeField] private PhotonRoomInstatiation _photonRoom;
    [SerializeField] private BoothColumnUI _productSelectionUI;
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

    private SO_ProductInventory _inventory;

    public bool HasActiveItem => _activeProduct != null;
    public ProductBehaviour ActiveProduct {
        get 
        {
            return _activeProduct;
        } 
        
        set 
        {
            _activeProduct = value;
            _activeProduct.transfromFollow.followTarget = _productAnchor;
            _activeProduct.productGrabbed += OnProductGrabbed;
        } 
    }

    private void Start()
    {
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

    public void SetInventory(SO_ProductInventory inventory)
    {
        _inventory = inventory;
        _productSelectionUI.SetInventory(inventory);
    }

    public void OnInventoryProductInvoked(bool isActive, string productName)
    {
        if(isActive)
        {
            SO_Product productToSpawn = _inventory.Products.FirstOrDefault(p => p.Name.Equals(productName));
            ActiveProduct = PhotonNetwork.Instantiate(productToSpawn.Id, _productAnchor.position, Quaternion.identity).GetComponent<ProductBehaviour>();
            ActiveProduct.info = productToSpawn;
        }
        else
        {
            ActiveProduct.productGrabbed -= OnProductGrabbed;
            PhotonNetwork.Destroy(ActiveProduct.GetComponent<PhotonView>());
        }
    }

    private IEnumerator SetUp()
    {
        yield return _photonRoom.IsConnectedToPhoton;

        if(PlayerWrapper.Instance.CanInteractWithBooths)
        {
            _productSelectionUI.SetCallbackListener(this);
        }
        else
        {
            _productSelectionUI.gameObject.SetActive(false);
        }
    }

    private IEnumerator SpawnDelayed(string productName)
    {
        yield return new WaitForSeconds(_respawndelay);

        SO_Product productToSpawn = _inventory.Products.FirstOrDefault(p => p.Name.Equals(productName));
        ActiveProduct = PhotonNetwork.Instantiate(productToSpawn.Id, _productAnchor.position, Quaternion.identity).GetComponent<ProductBehaviour>();
        ActiveProduct.info = productToSpawn;
    }

    private void OnProductGrabbed(string productId)
    {
        //Unsubscribe from previous product
        ActiveProduct.productGrabbed -= OnProductGrabbed;

        //spawn new product
        StartCoroutine(SpawnDelayed(productId));
    }
}
