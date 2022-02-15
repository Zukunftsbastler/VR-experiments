using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using VR_Experiment.Enums;

public class PlayerWrapper : SingletonBehaviour<PlayerWrapper>
{
    private PlayerNetworkInfo _networkInfo = null;

    private XRRig _rig;

    private string _avatarName;
    private GameObject _avatar;

    private Role _role;


    public void SetNetworkInfo(PlayerNetworkInfo networkInfo)
    {
        _networkInfo = networkInfo;
    }

    public void SetAvatar(GameObject avatarPrefab)
    {
        _avatarName = avatarPrefab.name;

    }


    //TODO: Handle playerspawn in here something like this:
    //NODE: the avatar has to be linked to the rig. Rig exposes head- and handtransforms
    /*
        GameObject avatar;

        //TODO: Depending on master client spawn presenter or visitor

        //TODO: Spawn player at diffrent spawn locations depending on their global spot

        if(PhotonNetwork.IsConnected)
        {
            avatar = PhotonNetwork.Instantiate(_avatarPrefab.name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            avatar = Instantiate(_avatarPrefab, Vector3.zero, Quaternion.identity);
        }

        AvatarLinkBehaviour avatarLink = avatar.GetComponent<AvatarLinkBehaviour>();
        avatarLink.LinkRigToAvatar(_head, _leftHand, _rightHand);
     */

}
