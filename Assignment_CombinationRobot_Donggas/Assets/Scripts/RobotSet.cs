using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSet : MonoBehaviour
{
    public float MoveSpeed;
    public float Attack { get; private set; }

    private void Awake()
    {
        Attack = Random.Range(10f, 50f);
    }
}
