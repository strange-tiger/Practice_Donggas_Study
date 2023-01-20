using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyHeap;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] MapManager mapManager;

    private Queue<MapNode> path;
    private MapHeap adjacentTiles;

    private void PutIntoAdjTiles()
    {

    }
}
