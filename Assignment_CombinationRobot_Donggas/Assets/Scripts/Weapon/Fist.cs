using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;
    [SerializeField]
    private RobotSet _set;
    private int _enemyLayer = 3;

    private const float _cooltime = 1f;
    private const float _attackRatio = 1f;

    private static bool _onCooltime = false;
    private static bool _onWhirlwind = false;
    private float _attack;
    private void Start()
    {
        _attack = _attackRatio * _set.Attack;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_onCooltime || !_input.AttackMouseOne)
        {
            return;
        }
        if (other.gameObject.layer != _enemyLayer || _onWhirlwind)
        {
            return;
        }

        Dummy dummy = other.GetComponent<Dummy>();
        dummy.TakeDamage(_attack);
        StartCoroutine(OnCooltime());
    }

    private static readonly YieldInstruction COOLTIME = new WaitForSeconds(_cooltime);
    private IEnumerator OnCooltime()
    {
        _onCooltime = true;
        yield return COOLTIME;
        _onCooltime = false;
    }
}
