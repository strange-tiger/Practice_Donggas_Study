using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    [SerializeField] float _recoveryRate;

    public override void UseSkill(Player usePlayer, Player defensePlayer)
    {
        usePlayer.HpGauge += Mathf.Round(_recoveryRate * Player.MAX_HP);

        base.UseSkill(usePlayer, defensePlayer);
    }
}
