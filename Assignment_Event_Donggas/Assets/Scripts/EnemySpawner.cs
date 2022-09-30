using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _kingPrefab;

    private Vector3[] _positions =
    {
        new Vector3(3f, 0f, 0f),
        new Vector3(-3f, 0f, 0f),
        new Vector3(0f, 0f, 3f),
        new Vector3(0f, 0f, -3f)
    };
    private GameObject[] _enemys = new GameObject[40];
    private int _normalEnemyIndex;
    private int _kingEnemyIndex;
    private void Awake()
    {
        _normalEnemyIndex = _enemys.Length * 9 / 10;
        _kingEnemyIndex = _enemys.Length / 10;
        for (int i = 0; i < _normalEnemyIndex; ++i)
        {
            _enemys[i] = Instantiate(_enemyPrefab);
            _enemys[i].transform.parent = transform; 
            _enemys[i].SetActive(false);
        }

        for (int i = _normalEnemyIndex; i < _enemys.Length; ++i)
        {
            _enemys[i] = Instantiate(_kingPrefab);
            _enemys[i].transform.parent = transform;
            _enemys[i].SetActive(false);
        }

        foreach (Vector3 pos in _positions)
        {
            int index = Random.Range(0, _enemys.Length);
            while (_enemys[index].activeSelf)
            {
                index = Random.Range(0, _enemys.Length);
            }
            _enemys[index].transform.position = pos;
            if(index < _normalEnemyIndex)
            {
                _enemys[index].transform.position += new Vector3(0f, 0.5f, 0f);
            }
            else
            {
                _enemys[index].transform.position += new Vector3(0f, 1f, 0f);
            }
            _enemys[index].SetActive(true);
        }
    }

    private void OnEnable()
    {
        Enemy.IsVacancy -= Spawn;
        Enemy.IsVacancy += Spawn;
    }

    private void Spawn(Vector3 pos)
    {
        StartCoroutine(SpawnRandomEnemy(pos));
    }

    public IEnumerator SpawnRandomEnemy(Vector3 pos)
    {
        pos.y = 0f;
        yield return new WaitForSeconds(Random.Range(2f, 4f));

        int index = Random.Range(0, _enemys.Length);
        while (_enemys[index].activeSelf)
        {
            index = Random.Range(0, _enemys.Length);
        }
        
        _enemys[index].transform.position = pos;
        if (index < _normalEnemyIndex)
        {
            _enemys[index].transform.position += new Vector3(0f, 0.5f, 0f);
        }
        else
        {
            _enemys[index].transform.position += new Vector3(0f, 1f, 0f);
        }

        _enemys[index].SetActive(true);
    }

    private void OnDisable()
    {
        Enemy.IsVacancy -= Spawn;
    }
}
