using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Avatar.Core;
using static VR_Experiment.Core.DistanceVisualizationManager;

namespace VR_Experiment.Menu.Hud
{
    public class HudDistanceDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _matrixWrapper;
        [SerializeField] private DistanceDisplayContentWrapper _contentPrefab;

        public void SetupDistanceDisplay(List<PlayerDistances> distances, List<int> players, SO_PlayerColor colors)
        {
            if(distances.Count > 0)
            {
                foreach(int player in players)
                {
                    if(Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber == player)
                        continue;

                    DistanceDisplayContentWrapper contentWrapper = Instantiate(_contentPrefab, _matrixWrapper);
                    contentWrapper.AddColor(colors.GetPlayerColorByActorNumber(player));
                    foreach(var distance in distances.Where(pd => pd.playerA == player))
                    {
                        contentWrapper.AddDistance(distance);
                    }
                    contentWrapper.gameObject.SetActive(true);
                    contentWrapper.name = $"Player {player}";
                }
            }

            DistanceDisplayContentWrapper lastCollumn = Instantiate(_contentPrefab, _matrixWrapper);
            lastCollumn.AddDistance(null);
            for(int i = 0; i < players.Count - 1; i++)
            {
                lastCollumn.AddColor(colors.GetPlayerColorByActorNumber(players[i]));
            }
            lastCollumn.gameObject.SetActive(true);
            lastCollumn.name = $"LastCollumn";
        }
    }
}
