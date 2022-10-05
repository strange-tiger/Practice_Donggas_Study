using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSet : MonoBehaviour
{
    [SerializeField]
    public float MoveSpeed;
    public float Attack;
    // public int WeaponIndex;

    private void Awake()
    {
        Attack = Random.Range(10f, 50f);
    }
}
