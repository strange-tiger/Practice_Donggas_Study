using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyHeap;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] MapManager mapManager;

    private Queue<MapNode> path = new Queue<MapNode>();
    private MapHeap potentialPath = new MapHeap();

    private MapNode curNode;
    private int count = 0;
    private int maxCount;

    private void PutIntoPotentialPath((int x, int z) position, Vector3 destination)
    {
        MapNode node = new MapNode(position, count, destination);

        potentialPath.Push(node);
    }

    private void PutIntoPath()
    {
        curNode = potentialPath.Pop();
        potentialPath.Clear();
        path.Enqueue(curNode);

        ++count;
    }

    public bool FindPath((int x, int z) start, Vector3 destination)
    {
        count = 0;
        maxCount = mapManager.MapSize * mapManager.MapSize;

        curNode = new MapNode(start, count, destination);

        Debug.Log($"[Dest] {destination}");

        while (MapNode.Manhattan(curNode.Position, destination) != 0 
              && count < maxCount)
        {
            PutIntoAdjTiles(curNode.Position, destination);

            PutIntoPath();

            Debug.Log($"[Node] {curNode.Position.x}, {curNode.Position.z}");
        }

        if (count < maxCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PutIntoAdjTiles((int x, int z) center, Vector3 destination)
    {
        for (int i = -1; i < 2; ++i)
        {
            for (int j = -1; j < 2; ++j)
            {
                int newX = center.x + i;
                int newZ = center.z + j;

                if (newX < 0 || newX >= mapManager.MapSize
                    || newZ < 0 || newZ >= mapManager.MapSize
                    || mapManager.Map[newX, newZ] || i == 0 || j == 0)
                {
                    continue;
                }

                PutIntoPotentialPath((newX, newZ), destination);
            }
        }
    }
}
