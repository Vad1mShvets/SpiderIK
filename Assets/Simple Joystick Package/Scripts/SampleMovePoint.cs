using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMovePoint : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    [SerializeField] private float _moveSpeed = 0.1f;

    private void FixedUpdate()
    {
        if (_joystick.Horizontal == 0 && _joystick.Vertical == 0)
            return;

        Vector3 direction = new Vector3(_joystick.Horizontal, _joystick.Vertical, 0);

        transform.Translate(direction * _moveSpeed);
    }
}
