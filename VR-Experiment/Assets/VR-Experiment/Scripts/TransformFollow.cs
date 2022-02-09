using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    public Transform followTarget;

    private void FixedUpdate()
    {
        if(followTarget != null)
        {
            transform.SetPositionAndRotation(followTarget.position, followTarget.rotation);
        }
    }
}
