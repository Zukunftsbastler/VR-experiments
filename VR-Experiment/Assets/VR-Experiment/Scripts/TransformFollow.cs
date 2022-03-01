using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR_Experiment.Enums;

public class TransformFollow : MonoBehaviour
{
    private enum PositionSetting
    {
        None,
        Full_Position,
        X_Position,
        Y_Position,
        Z_Position
    }

    private enum RotationSetting
    {
        None,
        Full_Rotation,
        X_Rotation,
        Y_Rodation,
        Z_Rotation,
        LookAtTarget
    }

    public Transform followTarget;
    [Space]
    [SerializeField] private UpdateType _updateFollowType = UpdateType.UpdateAndBeforRender;
    [Space]
    [SerializeField] private PositionSetting _positionSetting;
    [SerializeField] private RotationSetting _rotationSetting;

    private void OnEnable()
    {
        Application.onBeforeRender += OnBeforRender;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= OnBeforRender;
    }

    private void FixedUpdate()
    {
        if(followTarget != null && _updateFollowType != UpdateType.BeforRender)
        {
            UpdatePosition();
            UpdateRotation();
        }
    }

    private void OnBeforRender()
    {
        if(followTarget != null && _updateFollowType != UpdateType.Update)
        {
            UpdatePosition();
            UpdateRotation();
        }
    }

    private void UpdatePosition()
    {
        switch(_positionSetting)
        {
            case PositionSetting.None:
                break;
            case PositionSetting.Full_Position:
                transform.position = followTarget.position;
                break;
            case PositionSetting.X_Position:
                Vector3 newX_Pos = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
                transform.position = newX_Pos;
                break;
            case PositionSetting.Y_Position:
                Vector3 newY_Pos = new Vector3(transform.position.x, followTarget.position.y, transform.position.z);
                transform.position = newY_Pos;
                break;
            case PositionSetting.Z_Position:
                Vector3 newZ_Pos = new Vector3(transform.position.x, transform.position.y, followTarget.position.z);
                transform.position = newZ_Pos;
                break;
            default:
                Debug.LogWarning($"<b>{gameObject.name}</b> - {_positionSetting} are not implemented jet.");
                break;
        }
    }

    private void UpdateRotation()
    {
        switch(_rotationSetting)
        {
            case RotationSetting.None:
                break;
            case RotationSetting.Full_Rotation:
                transform.rotation = followTarget.rotation;
                break;
            case RotationSetting.X_Rotation:
                Vector3 newX_Rot = new Vector3(followTarget.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Euler(newX_Rot);
                break;
            case RotationSetting.Y_Rodation:
                Vector3 newY_Rot = new Vector3(transform.rotation.eulerAngles.x, followTarget.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Euler(newY_Rot);
                break;
            case RotationSetting.Z_Rotation:
                Vector3 newZ_Rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, followTarget.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Euler(newZ_Rot);
                break;
            case RotationSetting.LookAtTarget:
                transform.LookAt(followTarget);
                break;
            default:
                Debug.LogWarning($"<b>{gameObject.name}</b> - {_rotationSetting} are not implemented jet.");
                break;
        }
    }
}
