using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    private enum FollowSettings
    {
        PositionAndRotation,
        Position,
        X_Position,
        Y_Position,
        Z_Position,
        Rotation,
    }

    public Transform followTarget;
    [Space]
    [SerializeField] private FollowSettings _settings;



    private void FixedUpdate()
    {
        if(followTarget != null)
        {
            switch(_settings)
            {
                case FollowSettings.PositionAndRotation:
                    transform.SetPositionAndRotation(followTarget.position, followTarget.rotation);
                    break;
                case FollowSettings.Position:
                    transform.position = followTarget.position;
                    break;
                case FollowSettings.X_Position:
                    Vector3 newX_Pos = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
                    transform.position = newX_Pos;
                    break;
                case FollowSettings.Y_Position:
                    Vector3 newY_Pos = new Vector3(transform.position.x, followTarget.position.y, transform.position.z);
                    transform.position = newY_Pos;
                    break;
                case FollowSettings.Z_Position:
                    Vector3 newZ_Pos = new Vector3(transform.position.x, transform.position.y, followTarget.position.z);
                    transform.position = newZ_Pos;
                    break;
                case FollowSettings.Rotation:
                    transform.rotation = followTarget.rotation;
                    break;
                default:
                    Debug.LogWarning($"<b>{gameObject.name}</b> - {_settings} are not implemented jet.");
                    break;
            }
        }
    }
}
