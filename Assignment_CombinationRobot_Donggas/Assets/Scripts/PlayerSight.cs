using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _MinRotation;
    [SerializeField]
    private float _MaxRotation;
    private void Awake()
    {
        Camera.main.enabled = false;
        _camera.enabled = true;
    }

    private void Update()
    {
        float rotation = (_camera.transform.rotation * Quaternion.Euler(-_rotationSpeed * _input.RotationAxisY, 0f, 0f)).x * 180f;
        Debug.Log(rotation);
        if (rotation > _MinRotation && rotation < _MaxRotation)
        {
            _camera.transform.rotation *= Quaternion.Euler(-_rotationSpeed * _input.RotationAxisY, 0f, 0f);
        }
        
    }
}
