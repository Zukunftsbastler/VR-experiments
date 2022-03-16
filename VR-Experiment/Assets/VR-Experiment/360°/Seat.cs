using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool IsOccupied { get; private set; }
    public Player Player { get; private set; }

    public void AddPlayer(Player player)
    {
        Player = player;
        IsOccupied = true;
    }

    public void RemovePlayer()
    {
        Player = null;
        IsOccupied = false;
    }
}
