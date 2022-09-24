using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegRaycast : MonoBehaviour
{
    public Vector3 RaycastPoint { get; private set; }

    private RaycastHit _raycastHit;

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _raycastHit))
        {
            RaycastPoint = _raycastHit.point;
        }
    }
}
