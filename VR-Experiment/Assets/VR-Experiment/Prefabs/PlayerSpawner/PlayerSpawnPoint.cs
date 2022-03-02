using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private Color _skinColor;

    public Vector3 Position => transform.position;
    public Vector3 Orientation => transform.forward;
    public Color Color => _skinColor;
}
