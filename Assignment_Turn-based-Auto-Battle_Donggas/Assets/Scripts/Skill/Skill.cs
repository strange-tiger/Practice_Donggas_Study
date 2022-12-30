using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Skill : MonoBehaviour 
{
    [SerializeField] protected int _maxCooltime;
    [SerializeField] TextMeshProUGUI _cooltimeCountText;

    public int Cooltime
    {
        get { return _cooltime; }
        set 
        { 
            _cooltime = value;
            
            _cooltimeCountText.text = _cooltime.ToString();
        }
    }
    protected int _cooltime;

    protected void Awake()
    {
        Cooltime = _maxCooltime;
    }

    public bool OnAvailable() => Cooltime == 0;

    public virtual void UseSkill(Player usePlayer, Player defensePlayer)
    {
        Cooltime = _maxCooltime;
        return;
    }
}
