using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private const float MIN_SPEED = 5f;
    private const float MAX_SPEED = 40f;
    private const float DAMAGE = 10f;

    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _actionSlider;
    [SerializeField] TextMeshProUGUI _damagedText;
    [SerializeField] float _speed;
    [SerializeField] float _damage;
    [SerializeField] float _maxHp;
    [SerializeField] float _maxAction;

    public event Action OnDead;

    public float Speed { get => _speed; }
    public float Damage { get => _damage; }

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
            StartCoroutine(onAction());
        }
    }
    private float _actionGauge;

    private Vector3 _textInitPosition;
    private void OnEnable()
    {
        _hpSlider.maxValue = _maxHp;
        _actionSlider.maxValue = _maxAction;

        _hpGauge = _maxHp;
        _actionGauge = 0f;

        _hpSlider.value = HpGauge;
        _actionSlider.value = ActionGauge;

        _speed = UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);

        _textInitPosition = _damagedText.rectTransform.localPosition;
        _damagedText.gameObject.SetActive(false);
    }

    private static readonly WaitForEndOfFrame COROUTINE_FRAME = new WaitForEndOfFrame();
    private const float ON_DAMAGED_DELAY = 2f;
    private IEnumerator onDamaged(float damage)
    {
        float elapsedTime = 0f;
        float percentage = 0f;

        _damagedText.rectTransform.localPosition = _textInitPosition;

        _damagedText.color = Color.white;
        _damagedText.text = Mathf.RoundToInt(damage).ToString();

        _damagedText.gameObject.SetActive(true);

        while(elapsedTime <= ON_DAMAGED_DELAY)
        {
            yield return COROUTINE_FRAME;
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / ON_DAMAGED_DELAY;

            _damagedText.rectTransform.localPosition = Vector3.Lerp(_textInitPosition, _textInitPosition + Vector3.up, percentage);
            
            _damagedText.color = (1f - percentage) * Color.white;
            
            _hpSlider.value -= damage * Time.deltaTime / ON_DAMAGED_DELAY;

            if (_hpSlider.value <= 0f)
            {
                OnDead.Invoke();
                break;
            }
        }
    }

    private const float ON_ACTION_DELAY = 1f;
    private IEnumerator onAction()
    {
        float elapsedTime = 0f;

        while (elapsedTime <= ON_ACTION_DELAY)
        {
            yield return COROUTINE_FRAME;
            elapsedTime += Time.deltaTime;

            _actionSlider.value += Speed * Time.deltaTime;

            if (_actionSlider.value >= 100f)
            {
                _actionSlider.value = 0f;
                break;
            }
        }
    }

    private static readonly WaitForSeconds DELAY_FOR_ATTACK = new WaitForSeconds(0.5f);
    private const float ON_MOVE_DELAY = 0.5f;
    private static readonly Vector3 ON_MOVE_DESTINATION = 7.5f * Vector3.forward;
    private Vector3 _playerInitPosition;
    private IEnumerator onMove()
    {
        float elapsedTime = 0f;
        float percentage = 0f;

        _playerInitPosition = transform.localPosition;

        while (elapsedTime <= ON_MOVE_DELAY)
        {
            yield return COROUTINE_FRAME;
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / ON_MOVE_DELAY;

            transform.localPosition = Vector3.Lerp(_playerInitPosition, _playerInitPosition + ON_MOVE_DESTINATION, percentage);
        }

        yield return DELAY_FOR_ATTACK;

        while (elapsedTime >= 0f)
        {
            yield return COROUTINE_FRAME;
            elapsedTime -= Time.deltaTime;
            percentage = elapsedTime / ON_MOVE_DELAY;

            transform.localPosition = Vector3.Lerp(_playerInitPosition, _playerInitPosition + ON_MOVE_DESTINATION, percentage);
        }

    }
}
