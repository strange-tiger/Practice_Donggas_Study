using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private RobotSet _set;
    private float _moveSpeed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private PlayerInput _input;
    [SerializeField]
    private Transform _transform;
    
    private void Awake()
    {
        _moveSpeed = _set.MoveSpeed;
    }

    private void Update()
    {
        _transform.Translate(_moveSpeed * new Vector3(_input.MoveAxisHorizontal, 0f, _input.MoveAxisVertical), Space.Self);
        _transform.Rotate(new Vector3(0f, _rotationSpeed * _input.RotationAxisX, 0f), Space.Self);
    }
}
