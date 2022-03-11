using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "PlayerColor", menuName = "VR-Experiment/PlayerColor", order = 3)]
public class SO_PlayerColor : ScriptableObject
{
    [SerializeField] private List<Color> _playerColors;

    public Color GetPlayerColorByActorNumber(int actor)
    {
        Assert.IsNotNull(_playerColors, "There are no Colors defined");
        if (actor < 0)
            return _playerColors.FirstOrDefault();
        int index = actor % _playerColors.Count;
        Color c = _playerColors[index];
        Debug.Log($"Getting color {c} for actor {actor}");
        return c;
    }
}
