using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR_Experiment.Core;
using VR_Experiment.Networking;
using VR_Experiment.XR;

public class SortingHat : NetworkedGrabInteractable
{
    [SerializeField] private Role _sortingRole;
    [SerializeField] private TransformFollow _transformFollow;

    private Role _prevPlayerRole;

    public Rigidbody Rigidbody => _rigidbody;
    public TransformFollow TransformFollow => _transformFollow;
    public Role PrevPlayerRole => _prevPlayerRole;
    public Role SortingRole => _sortingRole;

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        yield return PlayerWrapper.Instance.HasSpawned;

        _prevPlayerRole = PlayerWrapper.Instance.GetLocalRole();
    }
}
