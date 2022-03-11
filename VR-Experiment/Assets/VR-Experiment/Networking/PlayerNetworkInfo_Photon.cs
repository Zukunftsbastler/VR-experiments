using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using VR_Experiment.Enums;

public class PlayerNetworkInfo_Photon : PlayerNetworkInfo
{
    private Player _client;

    public Player Client => _client;

    public PlayerNetworkInfo_Photon(Player client)
    {
        _client = client;
    }
}
