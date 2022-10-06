using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shuffle;

public class WeaponSet : MonoBehaviour
{
    static public List<GameObject> WeaponList = new List<GameObject>();
    [SerializeField]
    private GameObject[] _weapons;
    private void Awake()
    {
        WeaponList = ShuffleList.GetShuffleList(WeaponList);
        
    }
}
