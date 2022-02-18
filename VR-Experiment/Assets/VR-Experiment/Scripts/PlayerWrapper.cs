using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using VR_Experiment.Enums;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using Photon.Pun;

public class PlayerWrapper : SingletonBehaviour<PlayerWrapper>
{
    private PlayerNetworkInfo _networkInfo = null;

    private XRRig _rig = null;

    private string _avatarName = null;
    private AvatarLinkBehaviour _avatarLink = null;

    private Role _role = Role.None;

    public bool CanConnectToPhoton => _avatarName != null;
    public bool CanConnectToRoom => _role != Role.None;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetNetworkInfo(PlayerNetworkInfo networkInfo)
    {
        _networkInfo = networkInfo;
    }

    public void SetAvatar(GameObject avatarPrefab, bool updateAvatar)
    {
        if(updateAvatar)
        {
            if(_avatarName != null)
            {
                Destroy(_avatarLink.gameObject);
            }

            if(avatarPrefab != null)
            {
                GameObject avatar = Instantiate(avatarPrefab, _rig.transform.position, Quaternion.identity);
                avatar.transform.SetParent(this.transform);

                LinkAvatarToRig(avatar);
            }
        }

        _avatarName = avatarPrefab != null ? avatarPrefab.name : null;
    }

    public void SetRole(Role role)
    {
        _role = role;
    }

    public int GetGlobalSlot()
    {
        if(_networkInfo is PlayerNetworkInfo_Photon photonInfo)
        {
            return photonInfo.Client.ActorNumber;
        }

        return -1;
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        //Destroy local avatar
        Destroy(_avatarLink.gameObject);

        //spawn avatar via network
        GameObject avatar;
        if(_networkInfo is PlayerNetworkInfo_Photon)
        {
            avatar = PhotonNetwork.Instantiate(_avatarName, spawnPoint.position, Quaternion.identity);
            LinkAvatarToRig(avatar);
        }

        //Teleport Rig
        _rig.TeleportRig(spawnPoint);
    }

    private void LinkAvatarToRig(GameObject avatar)
    {
        _avatarLink = avatar.GetComponent<AvatarLinkBehaviour>();
        _avatarLink.LinkRigToAvatar(_rig);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _rig = FindObjectOfType<XRRig>();

        if(_avatarLink != null)
        {
            _avatarLink.LinkRigToAvatar(_rig);
        }
    }
}
