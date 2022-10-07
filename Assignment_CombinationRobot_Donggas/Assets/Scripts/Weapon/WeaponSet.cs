using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shuffle;

public class WeaponSet : MonoBehaviour
{
    [SerializeField]
    private Transform[] _arms;
    private GameObject[,] _weapons;
    private List<int> _weaponIndexList = new List<int>();

    private void Awake()
    {
        int armNum = _arms.Length;
        int weaponNum = _arms[0].childCount;
        _weapons = new GameObject[armNum, weaponNum];

        for (int i = 0; i < weaponNum; ++i)
        {
            _weaponIndexList.Add(i);
        }
        _weaponIndexList = ShuffleList.GetShuffleList(_weaponIndexList);
        
        for (int i = 0; i < _arms.Length; ++i)
        {
            for (int j = 0; j < weaponNum; ++j)
            {
                _weapons[i, j] = _arms[i].GetChild(j).gameObject;
                _weapons[i, j].SetActive(false);
            }
            _weapons[i, _weaponIndexList[i]].SetActive(true);
        }
    }
}
