using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public event Action<EPlayerState> OnAttack;
    public int Damage = 10;

    private PlayerState _state;
    private int _enemyLayerMask = 1 << 3;
    private void Awake()
    {
        _state = GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _enemyLayerMask))
            {
                Debug.Log("Did Hit");

                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                
                OnAttack -= enemy.GetAttack;
                OnAttack += enemy.GetAttack;
                
                OnAttack?.Invoke(_state.CurState);
                OnAttack -= enemy.GetAttack;
            }
        }
    }
}
