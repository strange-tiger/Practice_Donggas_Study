using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player[] _players;

    private Coroutine _currentPassTime;
    private bool _gameOver = false;
    private void Awake()
    {
        foreach (Player player in _players)
        {
            player.OnDead -= StopGame;
            player.OnDead += StopGame;

            player.AttackEnd -= ContinueGame;
            player.AttackEnd += ContinueGame;
        }

        _currentPassTime = StartCoroutine(passTime());
    }

    private void OnDisable()
    {
        foreach (Player player in _players)
        {
            player.OnDead -= StopGame;
            player.AttackEnd -= ContinueGame;
        }
    }

    private void StopGame()
    {
        _gameOver = true;
        StopAllCoroutines();
    }

    private void ContinueGame()
    {
        if (_gameOver) { return; }
        _currentPassTime = StartCoroutine(passTime());
    }

    private static readonly WaitForSeconds DELAY_TIME = new WaitForSeconds(1f);
    private IEnumerator passTime()
    {
        while (!_gameOver)
        {
            FillActionGauge();

            yield return DELAY_TIME;
        }
    }

    private void FillActionGauge()
    {
        for (int i = 0; i < _players.Length; ++i)
        {
            _players[i].ActionGauge += _players[i].Speed;

            if (_players[i].ActionGauge < Player.MAX_ACTION)
            {
                continue;
            }

            // 지금은 플레이어가 둘 뿐이기에 0이냐 1이냐로 판별하여 피격을 실행하지만,
            // 플레이어가 더 많아지는 경우는 ID를 통한 실행이 가능할 것이다.
            if (_players[i].SkillAvailable)
            {
                _players[i].UseSkill(_players[i], _players[1 - i]);
            }
            else
            {
                DefaultAttack(_players[i], _players[1 - i]);
            }

            StopAllCoroutines();
            StartCoroutine(_players[i].onAction());

            return;
        }
    }

    private void DefaultAttack(Player attackPlayer, Player defensePlayer)
    {
        defensePlayer.Damaged(attackPlayer.Damage);
    }
}
