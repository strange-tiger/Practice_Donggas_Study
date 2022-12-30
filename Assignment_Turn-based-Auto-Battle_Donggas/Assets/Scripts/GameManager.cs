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

            player.ActionEnd -= ContinueGame;
            player.ActionEnd += ContinueGame;
        }

        _currentPassTime = StartCoroutine(passTime());
    }

    private void OnDisable()
    {
        foreach (Player player in _players)
        {
            player.OnDead -= StopGame;
            player.ActionEnd -= ContinueGame;
        }
    }

    /// <summary>
    /// 플레이어 중 하나가 죽으면 호출된다.
    /// 작동하고 있는 코루틴을 모두 끈다.
    /// </summary>
    private void StopGame()
    {
        _gameOver = true;
        StopAllCoroutines();
    }

    /// <summary>
    /// 한 플레이어의 행동이 끝나면 호출된다.
    /// passTime 코루틴을 다시 호출한다.
    /// </summary>
    private void ContinueGame()
    {
        if (_gameOver) { return; }
        _currentPassTime = StartCoroutine(passTime());
    }

    private static readonly WaitForSeconds DELAY_TIME = new WaitForSeconds(1f);
    /// <summary>
    /// 시간을 재어 액션 게이지가 차도록 한다.
    /// 액션 게이지는 1초마다 차오른다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator passTime()
    {
        while (!_gameOver)
        {
            FillActionGauge();

            yield return DELAY_TIME;
        }
    }

    /// <summary>
    /// 각 플레이어의 액션 게이지가 차오르도록 한다.
    /// 만약 플레이어의 액션 게이지가 가득 차지 않았다면 계속한다.
    /// 만약 플레이어의 액션 게이지가 가득 찼다면 플레이어가 행동을 하도록 한다.
    /// 행동의 우선 순위는 스킬 > 일반 공격 순으로, 스킬이나 공격 둘 중 하나를 수행하고
    /// 액션 연출을 수행하고 종료한다.
    /// </summary>
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

    /// <summary>
    /// 일반 공격이다.
    /// 방어측 플레이어의 Damaged를 호출한다.
    /// </summary>
    /// <param name="attackPlayer"></param>
    /// <param name="defensePlayer"></param>
    private void DefaultAttack(Player attackPlayer, Player defensePlayer)
    {
        defensePlayer.Damaged(attackPlayer.Damage);
    }
}
