using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateUI : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;

    private TextMeshProUGUI _stateText;
    private void Awake()
    {
        _stateText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _playerState.OnStateChanged -= ChangeText;
        _playerState.OnStateChanged += ChangeText;
    }

    private void ChangeText(EPlayerState state)
    {
        string text;
        
        switch(state)
        {
            case EPlayerState.BURNING:
                text = "Burning";
                break;
            case EPlayerState.POISON:
                text = "Poison";
                break;
            case EPlayerState.CURSE:
                text = "Curse";
                break;
            case EPlayerState.FIST:
                text = "Fist";
                break;
            default:
                text = "Error";
                Debug.LogError("존재하지 않는 State입니다.");
                break;
        }

        _stateText.text = text;
    }

    private void OnDisable()
    {
        _playerState.OnStateChanged -= ChangeText;
    }
}
