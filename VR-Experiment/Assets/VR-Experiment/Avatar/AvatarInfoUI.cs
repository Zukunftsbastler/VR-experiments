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
    [SerializeField] private UpdateType _updateFollowType = UpdateType.UpdateAndBeforRender;
    [SerializeField] private Transform _target;
    [SerializeField] private LineRenderer _visualization;
    [SerializeField, Range(0f, 15f)] private float _range = 5f;
    [Space]
    [SerializeField] private TransformFollow uiFollow;
    [SerializeField] private Text _debugText;

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
        if(_isActive && _updateFollowType != UpdateType.BeforRender)
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
            uiFollow.followTarget = PlayerWrapper.Instance.Rig.Head;

            SetInfoUI();
        }
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
        Role role = PlayerWrapper.GetRole(photonView.Owner);
        string product = PlayerWrapper.GetActiveProduct(photonView.Owner);
        _debugText.text = $"Client {photonView.OwnerActorNr} Info: \nRole: '{role}' \nProduct: '{product}'";
    }

    private void OnBeforRender()
    {
        if(_isActive && _updateFollowType != UpdateType.Update)
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
