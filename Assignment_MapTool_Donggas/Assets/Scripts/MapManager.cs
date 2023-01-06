using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BlockType = Block.EBlockType;

[System.Serializable]
public class MapData
{
    public int[] data = new int[400];
}

public class MapManager : MonoBehaviour
{
    [Header("Block Group")]
    [SerializeField] Transform[] _rows;

    private const string FILE_NAME = "MapData.json";

    private Block[,] _blockGroup;
    private string _path;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Debug.Assert(_rows.Length * _rows[0].childCount == new MapData().data.Length, "블록 그룹 개수 부족 혹은 MapData 변경 필요");

        _blockGroup = new Block[_rows.Length, _rows[0].childCount];

        for (int i = 0; i < _rows.Length; ++i)
        {
            for (int j = 0; j < _rows[i].childCount; ++j)
            {
                _blockGroup[i, j] = _rows[i].GetChild(j).GetComponent<Block>();
            }
        }

        _path = Path.Combine(Application.dataPath, FILE_NAME);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveMap();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadMap();
        }
    }

    /// <summary>
    /// 각 블록의 타입을 int 형태로 변환하고 Json에 저장한다.
    /// </summary>
    private void SaveMap()
    {
        MapData mapData = new MapData();

        for (int i = 0; i < _blockGroup.GetLength(0); ++i)
        {
            for (int j = 0; j < _blockGroup.GetLength(1); ++j)
            {
                mapData.data[i * _blockGroup.GetLength(0) + j] = (int)_blockGroup[i, j].CurType;
            }
        }

        string saveJson = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(_path, saveJson);
    }

    /// <summary>
    /// 저장해 놓은 Json 파일을 로드하여 블록에 대입한다.
    /// </summary>
    private void LoadMap()
    {
        /*
         * 저장한 데이터 파일이 없다면 종료
         */
        if (!File.Exists(_path))
        {
            return;
        }

        MapData mapData = new MapData();

        string loadJson = File.ReadAllText(_path);
        mapData = JsonUtility.FromJson<MapData>(loadJson);

        /*
         * 데이터에 오류는 없는지 검사
         * 있다면 종료
         */
        int typeMax = (int)BlockType.MAX;
        foreach (int type in mapData.data)
        {
            if (type < 0 || type >= typeMax)
            {
                return;
            }
        }

        /*
         * 블록에 로드한 데이터를 대입
         */
        for (int i = 0; i < _blockGroup.GetLength(0); ++i)
        {
            for (int j = 0; j < _blockGroup.GetLength(1); ++j)
            {
                _blockGroup[i, j].CurType = (BlockType)mapData.data[i * _blockGroup.GetLength(0) + j];
            }
        }
    }
}
