using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public enum EEnemyState
    {
        BURN,
        POISONED,
        CURSED,
        MAX,
    }

    static public event Action<Vector3> IsVacancy;
    public UnityEvent<EPlayerState> OnTakeAttack;

    private event Action OnDie;
    public int Health
    {
        get
        {
            return _health;
        }

        private set
        {
            _health = value;
            Debug.Log($"{gameObject.name}:{_health}");
            if(_health <= 0)
            {
                OnDie.Invoke();
            }
        }
    }
    private int _health;

    [SerializeField] private int _initHealth;
    [SerializeField] private int _initScale;

    private PlayerAttack _player;
    private Collider _collider;
    private Renderer _renderer;
    private void Awake()
    {
        _player = Camera.main.GetComponent<PlayerAttack>();
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();

        Health = _initHealth;
    }

    private void OnEnable()
    {
        OnDie -= IsDead;
        OnDie += IsDead;
        OnTakeAttack.RemoveListener(GetAttack);
        OnTakeAttack.AddListener(GetAttack);
    }

    public void GetAttack(EPlayerState state)
    {
        switch(state)
        {
            case EPlayerState.BURNING:
                BurningAttacked();
                break;
            case EPlayerState.POISON:
                PoisonAttacked();
                break;
            case EPlayerState.CURSE:
                CurseAttacked();
                break;
            case EPlayerState.FIST:
                FistAttacked();
                break;
            default:
                break;
        }
    }

    private void IsDead()
    {
        StopAllCoroutines();
        transform.localScale = _initScale * Vector3.one;
        _collider.enabled = true;
        Health = _initHealth;

        gameObject.SetActive(false);
    }

    #region Damaged
    private bool _burnActive = false;
    private int _burnStack = 0;
    private int _burnMaxStack = 10;
    private void BurningAttacked()
    {
        if (_burnStack < _burnMaxStack)
        {
            ++_burnStack;
        }

        if (!_burnActive)
        {
            StartCoroutine(BurnCoroutine());
        }
    }
    
    public IEnumerator BurnCoroutine()
    {
        _burnActive = true;
        int count = 4;
        yield return new WaitForSeconds(0.1f);
        while (count != 0)
        {
            yield return new WaitForSeconds(4.9f);

            --count;
            _renderer.material.color = Color.red;
            StartCoroutine(ReturnColor());
            Health -= 5 * _burnStack;
        }
        _burnStack = 0;
        _burnActive = false;

        yield return null;
    }

    private void PoisonAttacked()
    {
        StartCoroutine(PoisonedCoroutine());
    }

    public IEnumerator PoisonedCoroutine()
    {
        int count = 2;
        yield return new WaitForSeconds(0.1f);
        while (count != 0)
        {
            yield return new WaitForSeconds(2.9f);
            
            --count;
            _renderer.material.color = Color.magenta;
            StartCoroutine(ReturnColor());
            Health -= 2;
        }
    }

    private void CurseAttacked()
    {
        StopCoroutine(CursedCoroutine());
        StartCoroutine(CursedCoroutine());
    }

    public IEnumerator CursedCoroutine()
    {
        while(true)
        {
            if (Health != 0 && Health < _initHealth / 10)
            {
                StopAllCoroutines();
                StartCoroutine(ReturnColor());
                StartCoroutine(IsDeadByCurse());
            }

            yield return null;
        }
    }

    public IEnumerator IsDeadByCurse()
    {
        float scale = 0f;
        _collider.enabled = false;
        while (scale <= 0.5f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, scale);
            scale += 0.01f;
        }
        Health = 0;
    }

    public IEnumerator ReturnColor()
    {
        yield return new WaitForSeconds(0.1f);
        _renderer.material.color = Color.white;
    }
    
    private void FistAttacked()
    {
        Health -= _player.Damage;
    }
#endregion

    private void OnDisable()
    {
        OnTakeAttack.RemoveListener(GetAttack);
        OnDie -= IsDead;
        IsVacancy?.Invoke(transform.position);
    }
}
