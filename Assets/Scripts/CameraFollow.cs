using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void LateUpdate()
    {
        if (Target.position.y > transform.position.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, Target.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
