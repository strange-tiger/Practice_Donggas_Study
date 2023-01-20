using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Define;

public class DestinationSetter : MonoBehaviour
{
    [SerializeField] MapManager mapManager;

    public Vector3 Destination { get; private set; } = new Vector3();

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destination = SetDestination();
        }
    }

    private Vector3 SetDestination()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float destPosX = Mathf.Round(mousePos.x);
        float destPosZ = Mathf.Round(mousePos.z);

        Vector3 dest = new Vector3(destPosX, PointTile.tileHeight, destPosZ);

        return dest;
    }
}
