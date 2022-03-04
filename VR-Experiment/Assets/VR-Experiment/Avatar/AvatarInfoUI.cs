using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Enums;

public class AvatarInfoUI : MonoBehaviourPun, IInRoomCallbacks
{
    [Header("Connection")]
    [SerializeField] private Transform _target;
    [SerializeField] private UpdateType _updateVisualization = UpdateType.UpdateAndBeforRender;
    [SerializeField] private LineRenderer _visualization;
    [SerializeField, Range(0f, 15f)] private float _range = 5f;
    [Space]
    [SerializeField] private TransformFollow _uiFollow;
    [Header("Info")]
    [SerializeField] private Text _title;
    [SerializeField] private Text _role;
    [SerializeField] private Text _product;

    private bool _isActive;

    private bool TargetOutOfRange => Vector3.Distance(transform.position, _target.position) > _range;

    private void OnEnable()
    {
        Application.onBeforeRender += OnBeforRender;
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
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

    public void ToggleUI()
    {
        _isActive = !gameObject.activeSelf;

        _visualization.enabled = _isActive;
        gameObject.SetActive(_isActive);

        if(_isActive)
        {
            transform.position = _target.position;
            _uiFollow.followTarget = PlayerWrapper.Instance.Rig.Head;

            SetInfoUI();
        }
    }

    public void DisplayProductInfo()
    {
        string productName = PlayerWrapper.GetActiveProduct(photonView.Owner);
        SO_Product product = Inventory.GetProductByName(productName);

        PlayerWrapper.Instance.Hud.DisplayProductTalkingpoints(product.Info);
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
