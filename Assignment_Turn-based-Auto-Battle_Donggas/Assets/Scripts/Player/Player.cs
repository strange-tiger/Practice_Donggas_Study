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

    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _actionSlider;
    [SerializeField] TextMeshProUGUI _damagedText;
    [SerializeField] float _speed;
    [SerializeField] float _maxHp;
    [SerializeField] float _maxAction;

    public event Action OnDead;

    public float HpGauge
    {
        get => _hpGauge;
        set
        {
            StartCoroutine(OnDamaged(_hpGauge - value));
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
            _actionSlider.value = _actionGauge;
        }
    }
    private float _actionGauge;

    private Vector3 _initTextPosition;
    private void OnEnable()
    {
        _hpSlider.maxValue = _maxHp;
        _actionSlider.maxValue = _maxAction;

        HpGauge = _maxHp;
        ActionGauge = 0f;

        _speed = UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);

        _initTextPosition = _damagedText.rectTransform.localPosition;
        _damagedText.gameObject.SetActive(false);
    }

    private static readonly WaitForEndOfFrame ON_DAMAGED_FRAME = new WaitForEndOfFrame();
    private const float ON_DAMAGED_DELAY = 2f;
    private IEnumerator OnDamaged(float damage)
    {
        float elapsedTime = 0f;
        float percentage = 0f;

        _damagedText.rectTransform.localPosition = _initTextPosition;
        _damagedText.gameObject.SetActive(true);

        _damagedText.color = Color.white;
        _damagedText.text = ((int)damage).ToString();

        while(elapsedTime <= ON_DAMAGED_DELAY)
        {
            yield return ON_DAMAGED_FRAME;
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / ON_DAMAGED_DELAY;

            _damagedText.rectTransform.localPosition = Vector3.Lerp(_initTextPosition, _initTextPosition + Vector3.up, percentage);
            
            _damagedText.color = (1f - percentage) * Color.white;
            
            _hpSlider.value -= damage * Time.deltaTime / ON_DAMAGED_DELAY;

            if (_hpSlider.value <= 0f)
            {
                OnDead.Invoke();
                break;
            }
        }
    }
}
