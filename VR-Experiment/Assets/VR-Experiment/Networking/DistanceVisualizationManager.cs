using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Avatar.Core;
using VR_Experiment.Menu.Hud;

namespace VR_Experiment.Core
{
    public class DistanceVisualizationManager : SingletonBehaviour<DistanceVisualizationManager>
    {
        public class PlayerDistances
        {
            public int playerA;
            public int playerB;
            public float distance;

            public PlayerDistances(int playerA, int playerB, float distance)
            {
                this.playerA = playerA;
                this.playerB = playerB;
                this.distance = distance;
            }

            public bool HasPlayer(int player)
            {
                return playerA == player || playerB == player;
            }

            public void UpdateDistance(Dictionary<int, Transform> players)
            {
                if(players.TryGetValue(playerA, out Transform gameObjectA) 
                    && players.TryGetValue(playerB, out Transform gameObjectB))
                {
                    this.distance = Vector3.Distance(gameObjectA.position, gameObjectB.position);
                }
                else
                {
                    Debug.LogError($"Trying to {nameof(UpdateDistance)} but eighter playerA '{playerA}' or playerB '{playerB}' are not registered.");
                }
            }
        }

        private enum DistanceDisplay : byte
        {
            None,
            HUD,
            World
        }

        [SerializeField] private Transform _xrHead;
        [SerializeField] private PlayerHud _playerHud;
        [SerializeField] private WorldDistanceDisplay _worldDisplayPrefab;
        [SerializeField] private SO_PlayerColor _colors;

        private Dictionary<int, Transform> _players;
        private List<PlayerDistances> _distances;

        private DistanceDisplay _displayState;

        protected override void OnAwake()
        {
            base.OnAwake();
            _players = new Dictionary<int, Transform>();
            _distances = new List<PlayerDistances>();
            _displayState = DistanceDisplay.None;
        }

        private void Update()
        {
            if(_displayState != DistanceDisplay.None)
            {
                foreach(var distance in _distances)
                {
                    distance.UpdateDistance(_players);
                }
            }
        }

        public void Register(Player player, Transform avatar)
        {
            if(_players.Count > 0)
            {
                foreach(var client in _players)
                {
                    float distance = Vector3.Distance(client.Value.transform.position, avatar.position);
                    _distances.Add(new PlayerDistances(player.ActorNumber, client.Key, distance));
                }
            }

            _players.Add(player.ActorNumber, avatar);

            Debug.Log($"On player registered: count '{_players.Count}'; distances: '{_distances.Count}'.");
        }

        public void Unregister(Player player)
        {
            _distances.RemoveAll(d => d.HasPlayer(player.ActorNumber));
            _players.Remove(player.ActorNumber);
        }

        public void DisableDistanceDisplay()
        {
            SwitchDistanceDisplay(DistanceDisplay.None);
        }

        public void EnableHudDistanceDisplay()
        {
            SwitchDistanceDisplay(DistanceDisplay.HUD);
        }

        public void EnableWorldDistanceDisplay()
        {
            SwitchDistanceDisplay(DistanceDisplay.World);
        }

        private void SwitchDistanceDisplay(DistanceDisplay newDistanceDisplay)
        {
            switch(_displayState)
            {
                case DistanceDisplay.HUD:
                    _playerHud.RemoveDistanceDisplay();
                    break;
                case DistanceDisplay.World:
                    DestroyWorldDistanceDisplays();
                    break;

                case DistanceDisplay.None:
                default:
                    break;
            }

            switch(newDistanceDisplay)
            {
                case DistanceDisplay.HUD:
                    _playerHud.DisplayDistances(_distances, new List<int>(_players.Keys), _colors);
                    break;
                case DistanceDisplay.World:
                    InstantiateWordDistanceDisplay();
                    break;

                case DistanceDisplay.None:
                default:
                    break;
            }

            _displayState = newDistanceDisplay;
        }

        private void InstantiateWordDistanceDisplay()
        {
            foreach(var distance in _distances)
            {
                WorldDistanceDisplay display = Instantiate(_worldDisplayPrefab, transform);
                display.Setup(distance, _players[distance.playerA], _players[distance.playerB], _xrHead, _colors);
            }
        }

        private void DestroyWorldDistanceDisplays()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
