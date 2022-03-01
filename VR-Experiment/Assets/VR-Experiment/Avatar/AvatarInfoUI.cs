using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VR_Experiment.Enums;

public class AvatarInfoUI : MonoBehaviourPun
{
    [Header("Connection")]
    [SerializeField] private UpdateType _updateFollowType = UpdateType.UpdateAndBeforRender;
    [SerializeField] private Transform _target;
    [SerializeField] private LineRenderer _visualization;
    [SerializeField, Range(0f, 15f)] private float _range = 5f;

    public TransformFollow uiFollow;
    public Text _debugText;

    private bool _isActive;

    private bool TargetOutOfRange => Vector3.Distance(transform.position, _target.position) > _range;

    private void OnEnable()
    {
        Application.onBeforeRender += OnBeforRender;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= OnBeforRender;
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

            Role role = PlayerWrapper.GetRole(photonView.Owner);
            _debugText.text = $"Client {photonView.OwnerActorNr} Info: \n Role: '{role}'";
        }
    }

    private void UpdateVisualization()
    {
        _visualization.SetPosition(0, transform.position);
        _visualization.SetPosition(1, _target.position);

        if(TargetOutOfRange)
            ToggleUI();
    }

    private void OnBeforRender()
    {
        if(_isActive && _updateFollowType != UpdateType.Update)
        {
            UpdateVisualization();
        }
    }
}
