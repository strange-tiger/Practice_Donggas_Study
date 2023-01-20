using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrototype;
    [SerializeField] int mapSize;

    [Header("Player Init Position")]
    [SerializeField] int playerPosX;
    [SerializeField] int playerPosZ;

    private Stack<GameObject> obstaclePool;
    private Stack<GameObject> usedObstacles;
    private bool[,] map;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        map = new bool[mapSize, mapSize];
        obstaclePool = new Stack<GameObject>();
        usedObstacles = new Stack<GameObject>();

        InitObstaclePool();

        InitMap();
    }

    private void InitObstaclePool()
    {
        for (int i = 0; i < mapSize * mapSize; ++i)
        {
            GameObject obstacle = Instantiate(obstaclePrototype);
            obstacle.SetActive(false);

            obstaclePool.Push(obstacle);
        }
    }
    
    private void InitMap()
    {
        for (int i = 0; i < map.GetLength(0); ++i)
        {
            for (int j = 0; j < map.GetLength(1); ++j)
            {
                if (i == playerPosX && j == playerPosZ)
                {
                    continue;
                }
                else if (Random.Range(Percent.MIN, Percent.MAX) < Percent.OBSTACLE)
                {
                    map[i, j] = true;

                    InstallObstacle(i, j);
                }
                else
                {
                    map[i, j] = false;
                }
            }
        }
    }

    private void InstallObstacle(int x, int z)
    {
        GameObject obstacle = obstaclePool.Pop();
        usedObstacles.Push(obstacle);

        obstacle.transform.position = new Vector3(x, 0, z);
        obstacle.SetActive(true);
    }
}
