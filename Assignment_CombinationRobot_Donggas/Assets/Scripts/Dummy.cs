using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dummy : MonoBehaviour
{
    private Renderer _renderer;
    private TextMeshProUGUI[] _damagedTexts = new TextMeshProUGUI[15];
    private int _damagedTextIndex = 0;
    private LayerMask _attackLayer = 1 << 6;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        Transform canvas = transform.GetChild(0).transform;
        int count = canvas.childCount;
        for (int i = 0; i < count; ++i)
        {
            _damagedTexts[i] = canvas.GetChild(i).GetComponent<TextMeshProUGUI>();
            _damagedTexts[i].gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _attackLayer)
        {
            RobotSet temp = other.GetComponent<RobotSet>();
            StartCoroutine(OnChangeColor());
            StartCoroutine(OnShowDamage(temp.Attack));
        }
    }

    static private float _delayChangeColor = 0.1f;
    private static readonly YieldInstruction DELAY_CHANGE_COLOR = new WaitForSeconds(_delayChangeColor);
    private IEnumerator OnChangeColor()
    {
        _renderer.material.color = Color.red;
        yield return DELAY_CHANGE_COLOR;
        _renderer.material.color = Color.white;
    }

    static private float _delayShowDamage = 1f;
    static private float _delayOnFadeOutText = 0.1f;
    private static readonly YieldInstruction DELAY_SHOW_DAMAGE = new WaitForSeconds(_delayOnFadeOutText);
    private IEnumerator OnShowDamage(float damage)
    {
        int index = _damagedTextIndex;
        ++_damagedTextIndex;

        Debug.Log(damage);
        _damagedTexts[index].text = $"{damage}";
        _damagedTexts[index].gameObject.SetActive(true);

        float time = _delayShowDamage;
        while (time >= 0f)
        {
            yield return DELAY_SHOW_DAMAGE;
            time -= _delayOnFadeOutText;
            
            _damagedTexts[index].color = new Color(255f, 0f, 0f, _damagedTexts[index].color.a - _delayOnFadeOutText);
        }

        _damagedTexts[index].gameObject.SetActive(false);
    }
}
