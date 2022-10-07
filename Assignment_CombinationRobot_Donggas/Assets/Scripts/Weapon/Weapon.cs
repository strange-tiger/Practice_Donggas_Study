using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected bool _onCooltime = false;

    protected abstract void OnTriggerEnter(Collider other);

    protected abstract IEnumerator OnAttack();
}
