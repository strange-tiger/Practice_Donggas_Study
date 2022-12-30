using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : Skill
{
    [SerializeField] float _damageRate;

    public override void UseSkill(Player usePlayer, Player defensePlayer)
    {
        float damage = _damageRate * usePlayer.Damage;

        damage = Mathf.Round(damage);

        defensePlayer.Damaged(damage);

        base.UseSkill(usePlayer, defensePlayer);
    }
}
