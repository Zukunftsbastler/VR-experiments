using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoothBehaviour : MonoBehaviourPun, IInventoryCallbackListener
{
    private enum ProductInteraction : byte
    {
        None = 0,
        Spawned = 1,
    }

    [SerializeField] private PhotonRoomInstatiation _photonRoom;
    [Space]
    [SerializeField] private BoothOccupationUI _occupationUI;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private RevolvingStageBehaviour _stage;
    [Space]
    [SerializeField] private SO_ProductInventory _inventory;

    private Player _owner;

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        yield return _photonRoom.IsConnectedToPhoton;

        _occupationUI.Initialize(PlayerWrapper.Instance.CanOccupyBooths);
        _inventoryUI.Initialize(this);
    }

    public void Occupy()
    {
        photonView.RPC(nameof(RPC_OnOccupationChange), RpcTarget.All, true);
    }

    public void Leave()
    {
        photonView.RPC(nameof(RPC_OnOccupationChange), RpcTarget.All, false);
    }

    [PunRPC]
    private void RPC_OnOccupationChange(bool isActive, PhotonMessageInfo info)
    {
        if (isActive == false)
        {
            _owner = null;
            _inventoryUI.SetInventory(null);
            _occupationUI.UpdateButtons(PlayerWrapper.Instance.CanOccupyBooths, false);

            if (_stage.HasActiveItem)
            {
                DestroyImmediate(_stage.ActiveProduct);
            }
        }
        else
        {
            _owner = info.Sender;
            _inventoryUI.SetInventory(_inventory);
            _occupationUI.UpdateButtons(false, _owner.IsLocal);
        }
    }

    public void OnInventoryProductInvoked(string productId)
    {
        SpawnProduct(productId);
    }

    public void SpawnProduct(string productId)
    {
        SO_Product product = _inventory.Products.First(p => p.Id.Equals(productId));

        if (product == null)
        {
            Debug.LogWarning($"{gameObject.name} product with id '{productId}' has not been found.");
            return;
        }

        if (_stage.HasActiveItem)
        {
            DestroyImmediate(_stage.ActiveProduct);
        }

        //_stage.ActiveProduct = Instantiate(product.Asset, _stage.transform.position, Quaternion.identity);
        _stage.ActiveProduct = Instantiate(product.Asset, _stage.transform.position, Quaternion.identity);

        photonView.RPC(nameof(RPC_OnProductInteractionRecognized), _owner,
            productId,
            (byte)ProductInteraction.Spawned,
            PlayerWrapper.Instance.GetGlobalSlot());
    }

    [PunRPC]
    private void RPC_OnProductInteractionRecognized(string productId, byte interaction, int interactionActor)
    {
        SO_Product product = _inventory.Products.First(p => p.Id.Equals(productId));
        //Player interactor = GetPlayerByActorNumber(interactionActor);

        switch ((ProductInteraction)interaction)
        {
            case ProductInteraction.None:
                break;
            case ProductInteraction.Spawned:
                PlayerWrapper.Instance.Hud.DisplayProductNotification(interactionActor, "spawned", product);
                break;
            default:
                break;
        }
    }
}
