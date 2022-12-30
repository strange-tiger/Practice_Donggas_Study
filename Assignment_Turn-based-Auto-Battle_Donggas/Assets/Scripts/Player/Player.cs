using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public const float DAMAGE = 10f;
    public const float MAX_HP = 100f;
    public const float MAX_ACTION = 100f;

    private const float MIN_SPEED = 10f;
    private const float MAX_SPEED = 40f;

    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _actionSlider;
    [SerializeField] TextMeshProUGUI _damagedText;
    [SerializeField] float _speed;
    [SerializeField] float _damage;
    [SerializeField] float _defense;
    [SerializeField] Skill _firstSkill;
    [SerializeField] Skill _secondSkill;

    public event Action OnDead;
    public event Action AttackEnd;

    public float Speed { get => _speed; }
    public float Damage { get => _damage; }
    public float Defense { get => _defense; }
    public bool SkillAvailable
    {
        get => _firstSkill.OnAvailable() || _secondSkill.OnAvailable();
    }

    public float HpGauge
    {
        get => _hpGauge;
        set
        {
            StartCoroutine(onDamaged(_hpGauge - value));
            _hpGauge = value;
        }
    }
    private float _hpGauge;

    public float ActionGauge
    {
        get => _actionGauge;
        set
        {
            _actionGauge = value;
            StartCoroutine(onIdle());
        }
    }
    private float _actionGauge;

    private Vector3 _textInitPosition;
    private bool _onDamaged = false;
    private void OnEnable()
    {
        _hpSlider.maxValue = MAX_HP;
        _actionSlider.maxValue = MAX_ACTION;

        _hpGauge = MAX_HP;
        _actionGauge = 0f;

        _hpSlider.value = HpGauge;
        _actionSlider.value = ActionGauge;

        _speed = UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);
        _speed = Mathf.Round(_speed);

        _textInitPosition = _damagedText.transform.localPosition;
        _damagedText.gameObject.SetActive(false);
    }

    private static readonly WaitForEndOfFrame COROUTINE_FRAME = new WaitForEndOfFrame();
    private const float ON_DAMAGED_DELAY = 2f;
    private const float TEXT_MOVE_POSITION = 200f;
    private IEnumerator onDamaged(float damage)
    {
        float elapsedTime = 0f;
        float percentage = 0f;

        _onDamaged = true;

        ShowDamage(damage);

        while (elapsedTime <= ON_DAMAGED_DELAY)
        {
            yield return COROUTINE_FRAME;
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / ON_DAMAGED_DELAY;

            _damagedText.transform.localPosition =
                Vector3.Lerp(_textInitPosition, _textInitPosition + TEXT_MOVE_POSITION * Vector3.up, percentage);

            _damagedText.color = (1f - percentage) * Color.white;

            _hpSlider.value -= damage * Time.deltaTime / ON_DAMAGED_DELAY;

            if (_hpSlider.value <= 0f)
            {
                OnDead.Invoke();
                break;
            }
        }
    }

    private void ShowDamage(float damage)
    {
        _damagedText.rectTransform.localPosition = _textInitPosition;

        _damagedText.color = Color.white;
        _damagedText.text = Mathf.RoundToInt(damage).ToString();

        _damagedText.gameObject.SetActive(true);
    }

    private const float ON_ACTION_DELAY = 1f;
    private IEnumerator onIdle()
    {
        float elapsedTime = 0f;

        _onDamaged = false;

        while (elapsedTime <= ON_ACTION_DELAY)
        {
            while (_onDamaged) { yield return COROUTINE_FRAME; }

            yield return COROUTINE_FRAME;
            elapsedTime += Time.deltaTime;

            _actionSlider.value += Speed * Time.deltaTime;
        }
    }

    private static readonly WaitForSeconds DELAY_FOR_ATTACK = new WaitForSeconds(0.5f);
    private const float ON_MOVE_DELAY = 0.5f;
    private const float ON_MOVE_DESTINATION = 5f;
    private Vector3 _playerInitPosition;
    public IEnumerator onAction()
    {
        float elapsedTime = 0f;
        float percentage = 0f;

        _playerInitPosition = transform.localPosition;

        CountSkillCooltime();

        while (elapsedTime <= ON_MOVE_DELAY)
        {
            yield return COROUTINE_FRAME;
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / ON_MOVE_DELAY;

            transform.localPosition =
                Vector3.Lerp(_playerInitPosition, _playerInitPosition + ON_MOVE_DESTINATION * transform.forward, percentage);
        }

        yield return DELAY_FOR_ATTACK;

        while (elapsedTime >= 0f)
        {
            yield return COROUTINE_FRAME;
            elapsedTime -= Time.deltaTime;
            percentage = elapsedTime / ON_MOVE_DELAY;

            transform.localPosition =
                Vector3.Lerp(_playerInitPosition, _playerInitPosition + ON_MOVE_DESTINATION * transform.forward, percentage);
        }

        transform.localPosition = _playerInitPosition;

        ResetActionGauge();

        AttackEnd.Invoke();
    }

    private void CountSkillCooltime()
    {
        if (!SkillAvailable)
        {
            --_firstSkill.Cooltime;
            --_secondSkill.Cooltime;
        }
    }

    private void ResetActionGauge()
    {
        _actionGauge = 0f;
        _actionSlider.value = 0f;
    }

    public void Damaged(float damage)
    {
        if (damage < Defense)
        {
            damage = 0;
        }
        else
        {
            damage -= Defense;
        }

        HpGauge -= damage;
    }

    public void UseSkill(Player usePlayer, Player defensePlayer)
    {
        if (_secondSkill.OnAvailable())
        {
            _secondSkill.UseSkill(usePlayer, defensePlayer);
        }
        else if (_firstSkill.OnAvailable())
        {
            _firstSkill.UseSkill(usePlayer, defensePlayer);
        }
    }
}
