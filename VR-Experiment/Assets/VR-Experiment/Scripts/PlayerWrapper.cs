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
    private PlayerHud _hud = null;

    private string _avatarName = null;
    private AvatarLinkBehaviour _avatarLink = null;

    private Role _role = Role.None;

    public bool CanConnectToPhoton => _avatarName != null;
    public bool CanConnectToRoom => _role != Role.None;
    public bool CanOccupyBooths => _role > Role.Visitor;

    private XRRig Rig => _rig ??= FindObjectOfType<XRRig>();
    public PlayerHud Hud => _hud ??= FindObjectOfType<PlayerHud>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetPlayerData(PlayerNetworkInfo networkInfo, GameObject avatarPrefab, Role role)
    {
        SetNetworkInfo(networkInfo);
        SetAvatar(avatarPrefab, false);
        SetRole(role);
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
        if(_avatarLink != null)
            Destroy(_avatarLink.gameObject);

        //spawn avatar via network
        GameObject avatar;
        if(_networkInfo is PlayerNetworkInfo_Photon)
        {
            avatar = PhotonNetwork.Instantiate(_avatarName, spawnPoint.position, Quaternion.identity);
            LinkAvatarToRig(avatar);
        }

        //Teleport Rig
        Rig.TeleportRig(spawnPoint);
    }

    private void LinkAvatarToRig(GameObject avatar)
    {
        _avatarLink = avatar.GetComponent<AvatarLinkBehaviour>();
        _avatarLink.LinkRigToAvatar(Rig);
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
