using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;
using VR_Experiment.Core;
using VR_Experiment.Expo.Booth;
using VR_Experiment.Expo.Product;
using VR_Experiment.Menu.UI.Core;
using VR_Experiment.Networking;

[Obsolete]
public class BoothBehaviour : MonoBehaviourPun, IItemListCallbackListener
{
    private enum ProductInteraction : byte
    {
        None = 0,
        Spawned = 1,
    }

    [SerializeField] private BoothOccupationUI _occupationUI;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private RevolvingStageBehaviour _stage;

    private Player _owner;

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        yield return PhotonRoomInstatiation.Instance.IsConnectedToPhoton;

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
            _inventoryUI.SetInventory();
            _occupationUI.UpdateButtons(PlayerWrapper.Instance.CanOccupyBooths, false);

            if (_stage.HasActiveItem)
            {
                DestroyImmediate(_stage.ActiveProduct);
            }
        }
        else
        {
            _owner = info.Sender;
            _inventoryUI.SetInventory();
            _occupationUI.UpdateButtons(false, _owner.IsLocal);
        }
    }

    public void OnItemToggleInvoked(bool isActive, string productId)
    {
        SpawnProduct(productId);
    }

    public void SpawnProduct(string productId)
    {
        SO_Product product = Inventory.GetProductByName(productId);

        if (product == null)
        {
            Debug.LogWarning($"{gameObject.name} product with id '{productId}' has not been found.");
            return;
        }

        if (_stage.HasActiveItem)
        {
            DestroyImmediate(_stage.ActiveProduct);
        }

        _stage.ActiveProduct = Instantiate(product.Asset, _stage.transform.position, Quaternion.identity).GetComponent<ProductBehaviour>();

        photonView.RPC(nameof(RPC_OnProductInteractionRecognized), _owner,
            productId,
            (byte)ProductInteraction.Spawned,
            PlayerWrapper.Instance.GetGlobalSlot());
    }

    [PunRPC]
    private void RPC_OnProductInteractionRecognized(string productId, byte interaction, int interactionActor)
    {
        SO_Product product = Inventory.GetProductByName(productId);
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
