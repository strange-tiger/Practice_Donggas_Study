using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooseRobot : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _robotHeads = new GameObject[2];

    private void OnEnable()
    {
        PlayerInput.OnRobotIndexChanged -= ChooseRobot;
        PlayerInput.OnRobotIndexChanged += ChooseRobot;
    }

    private void ChooseRobot(int index)
    {
        foreach(GameObject head in _robotHeads)
        {
            head.SetActive(false);
        }
        _robotHeads[index].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerInput.OnRobotIndexChanged -= ChooseRobot;
    }
}
