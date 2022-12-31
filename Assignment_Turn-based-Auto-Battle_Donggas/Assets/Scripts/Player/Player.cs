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
    public event Action ActionEnd;

    public float Speed { get => _speed; }
    public float Damage { get => _damage; }
    public float Defense { get => _defense; }
    
    /// <summary>
    /// 소유하는 두 스킬 중 어느 것이든 사용가능하면 true, 아니면 false
    /// </summary>
    public bool SkillAvailable
    {
        get => _firstSkill.OnAvailable() || _secondSkill.OnAvailable();
    }

    /// <summary>
    /// HpGauge에 변동이 있을 때마다 
    /// HP 슬라이더가 줄어드는 연출을 보인다.
    /// </summary>
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

    /// <summary>
    /// ActionGauge에 변동이 있을 때마다 
    /// Action 슬라이더가 차오르는 연출을 보인다.
    /// </summary>
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
    /// <summary>
    /// 대미지를 입을 때 연출을 보인다.
    /// 텍스트가 대미지를 표시하고 올라가며 사라진다.
    /// 대미지 만큼 체력바(_hpSlider)가 점점 줄어든다.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 텍스트를 초기화하고 활성화한다.
    /// 대미지를 텍스트에 적용한다.
    /// </summary>
    /// <param name="damage"></param>
    private void ShowDamage(float damage)
    {
        _damagedText.rectTransform.localPosition = _textInitPosition;

        _damagedText.color = Color.white;
        _damagedText.text = Mathf.RoundToInt(damage).ToString();

        _damagedText.gameObject.SetActive(true);
    }

    private const float ON_ACTION_DELAY = 1f;
    /// <summary>
    /// 액션 게이지가 차오르는 연출을 보인다.
    /// 대미지를 입었다면 잠시 멈춘다.
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 행동 게이지가 가득 찼다면 수행한다.
    /// 스킬 턴을 하나씩 줄인다.
    /// 적 앞까지 이동하는 모션을 보인다.
    /// 행동 게이지를 비운다.
    /// </summary>
    /// <returns></returns>
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

        ActionEnd.Invoke();
    }

    /// <summary>
    /// 스킬 턴을 줄인다.
    /// </summary>
    private void CountSkillCooltime()
    {
        if (!SkillAvailable)
        {
            --_firstSkill.Cooltime;
            --_secondSkill.Cooltime;
        }
    }

    /// <summary>
    /// 행동 게이지를 비운다.
    /// </summary>
    private void ResetActionGauge()
    {
        _actionGauge = 0f;
        _actionSlider.value = 0f;
    }

    /// <summary>
    /// 공격을 받아 대미지를 입는다.
    /// 대미지가 방어(Defense)보다 작다면 대미지는 0이 된다.
    /// 대미지가 방어보다 크다면 대미지에서 방어만큼 뺀다.
    /// </summary>
    /// <param name="damage"></param>
    public void Damaged(float damage)
    {
        damage = Mathf.Max(damage - Defense, 0f);
        
        HpGauge -= damage;
    }

    /// <summary>
    /// 스킬을 사용한다.
    /// 2번째 스킬(_secondSkill)이 우선 사용된다.
    /// </summary>
    /// <param name="usePlayer"></param>
    /// <param name="defensePlayer"></param>
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
