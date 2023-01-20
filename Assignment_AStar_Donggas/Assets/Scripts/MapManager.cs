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

    public bool[,] Map { get; private set; }
    public int MapSize { get => mapSize; }

    private Stack<GameObject> obstaclePool;
    private Stack<GameObject> usedObstacles;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 초기화를 실행
    /// 인스턴스화, 장애물의 오브젝트 풀 생성, 맵 초기 설정 실행
    /// </summary>
    private void Initialize()
    {
        Map = new bool[mapSize, mapSize];
        obstaclePool = new Stack<GameObject>();
        usedObstacles = new Stack<GameObject>();

        InitObstaclePool();

        InitMap();
    }

    /// <summary>
    /// 장애물의 오브젝트 풀을 생성
    /// Stack으로 관리
    /// </summary>
    private void InitObstaclePool()
    {
        for (int i = 0; i < mapSize * mapSize; ++i)
        {
            GameObject obstacle = Instantiate(obstaclePrototype);
            obstacle.SetActive(false);

            obstaclePool.Push(obstacle);
        }
    }
    
    /// <summary>
    /// Map의 각 위치에 장애물을 설치할지를 20% 확률로 계산
    /// </summary>
    private void InitMap()
    {
        for (int i = 0; i < Map.GetLength(0); ++i)
        {
            for (int j = 0; j < Map.GetLength(1); ++j)
            {
                if (i == playerPosX && j == playerPosZ)
                {
                    continue;
                }
                else if (Random.Range(Percent.MIN, Percent.MAX) < Percent.OBSTACLE)
                {
                    Map[i, j] = true;

                    InstallObstacle(i, j);
                }
                else
                {
                    Map[i, j] = false;
                }
            }
        }
    }

    /// <summary>
    /// 장애물을 매개변수로 받은 위치에 설치
    /// 오브젝트 풀에서 장애물을 꺼내, usedObstacles에 저장하고 활성화
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    private void InstallObstacle(int x, int z)
    {
        GameObject obstacle = obstaclePool.Pop();
        usedObstacles.Push(obstacle);

        obstacle.transform.position = new Vector3(x, 0, z);
        obstacle.SetActive(true);
    }
}
