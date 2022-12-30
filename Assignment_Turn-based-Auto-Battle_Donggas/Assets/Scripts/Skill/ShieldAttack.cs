using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : Skill
{
    [SerializeField] float _fixedDamage;
    [SerializeField] float _defenseDamageRate;

    public override void UseSkill(Player usePlayer, Player defensePlayer)
    {
        float damage = _fixedDamage + _defenseDamageRate * usePlayer.Defense;

        damage = Mathf.Round(damage);

        defensePlayer.Damaged(damage);

        base.UseSkill(usePlayer, defensePlayer);
    }
}
