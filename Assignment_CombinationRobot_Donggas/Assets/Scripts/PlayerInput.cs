using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MoveAxisHorizontal;
    public float MoveAxisVertical;
    public float RotationAxisX;
    public float RotationAxisY;
    public bool AttackMouseOne;
    public bool AttackMouseTwo;
    public int RobotIndex;
    private void Awake()
    {
        MoveAxisHorizontal = transform.position.x;
        MoveAxisVertical = transform.position.z;

        RotationAxisX = transform.rotation.eulerAngles.x;
        RotationAxisY = transform.rotation.eulerAngles.y;
        
        AttackMouseOne = false;
        AttackMouseTwo = false;
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
