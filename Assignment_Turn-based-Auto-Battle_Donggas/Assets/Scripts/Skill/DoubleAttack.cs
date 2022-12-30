using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : Skill
{
    [SerializeField] float _damageRate;
    [SerializeField] int _count;

    public override void UseSkill(Player usePlayer, Player defensePlayer)
    {
        float damage = _damageRate * usePlayer.Damage;

        damage = Mathf.Round(damage);

        StartCoroutine(doubleAttack(damage, defensePlayer));

        base.UseSkill(usePlayer, defensePlayer);
    }

    private static readonly WaitForSeconds DELAY_FOR_DOUBLE_ATTACK = new WaitForSeconds(0.5f);
    private IEnumerator doubleAttack(float damage, Player victim)
    {
        int count = _count;

        while(count != 0)
        {
            --count;

            victim.Damaged(damage);

            yield return DELAY_FOR_DOUBLE_ATTACK;
        }
    }
}
