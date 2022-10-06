using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    static public event Action<int> OnRobotIndexChanged;

    public float MoveAxisHorizontal { get; private set; }
    public float MoveAxisVertical { get; private set; }
    public float RotationAxisX { get; private set; }
    public float RotationAxisY { get; private set; }
    public bool AttackMouseOne { get; private set; }
    public bool AttackMouseTwo { get; private set; }
    public int RobotIndex
    {
        get
        {
            return _robotIndex;
        }
        private set
        {
            _robotIndex = value;
            OnRobotIndexChanged.Invoke(_robotIndex);
        }
    }
    private int _robotIndex = 0;
    private void Awake()
    {
        MoveAxisHorizontal = transform.position.x;
        MoveAxisVertical = transform.position.z;

        RotationAxisX = transform.rotation.eulerAngles.x;
        RotationAxisY = transform.rotation.eulerAngles.y;
        
        AttackMouseOne = false;
        AttackMouseTwo = false;

        RobotIndex = 0;
    }

    private void Update()
    {
        MoveAxisHorizontal = Input.GetAxis("Horizontal");
        MoveAxisVertical = Input.GetAxis("Vertical");

        RotationAxisX = Input.GetAxis("Mouse X");
        RotationAxisY = Input.GetAxis("Mouse Y");
        
        AttackMouseOne = Input.GetMouseButtonDown(0);
        AttackMouseTwo = Input.GetMouseButtonDown(1);
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RobotIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RobotIndex = 1;
        }
    }
}
