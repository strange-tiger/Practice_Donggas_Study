using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyHeap;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] MapManager mapManager;

    public Queue<MapNode> Path { get; private set; } = new Queue<MapNode>();
    private MapHeap potentialPath = new MapHeap();

    private MapNode curNode;
    private int count = 0;
    private int maxCount;

    public bool FindPath((int x, int z) start, Vector3 destination)
    {
        Reset();

        curNode = new MapNode(start, count, destination);

        while (MapNode.Manhattan(curNode.Position, destination) != 0 
              && count < maxCount)
        {
            PutIntoAdjTiles(curNode.Position, destination);

            PutIntoPath();
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

    public void Reset()
    {
        count = 0;
        maxCount = mapManager.MapSize * mapManager.MapSize;
        Path.Clear();
        potentialPath.Clear();
    }

    private void PutIntoPotentialPath((int x, int z) position, Vector3 destination)
    {
        foreach (MapNode node in Path)
        {
            if (position == node.Position)
            {
                return;
            }
        }
        
        MapNode pathNode = new MapNode(position, count, destination);

        potentialPath.Push(pathNode);
    }

    private void PutIntoPath()
    {
        curNode = potentialPath.Pop();
        potentialPath.Clear();
        Path.Enqueue(curNode);

        ++count;
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
                    || mapManager.Map[newX, newZ] || (i == 0 && j == 0))
                {
                    continue;
                }

                PutIntoPotentialPath((newX, newZ), destination);
            }
        }
    }
}
