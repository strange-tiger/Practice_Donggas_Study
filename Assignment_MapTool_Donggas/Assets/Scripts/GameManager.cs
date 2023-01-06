using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    [SerializeField] GameObject _player;
    [SerializeField] Button _restartBtn;
    [SerializeField] Button _quitBtn;

    private static readonly Vector3 INIT_PLAYER_POS = new Vector3(0f, 0.5f, 0f);

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _restartBtn.onClick.AddListener(Restart);
        _quitBtn.onClick.AddListener(Quit);

        _ui.SetActive(false);
        _player.SetActive(false);
    }

    private void OnDisable()
    {
        _restartBtn.onClick.RemoveListener(Restart);
        _quitBtn.onClick.RemoveListener(Quit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    /// <summary>
    /// 플레이어를 지정된 위치로 옮기고 활성화한다.
    /// </summary>
    private void SpawnPlayer()
    {
        _player.transform.position = INIT_PLAYER_POS;
        _player.SetActive(true);
    }

    /// <summary>
    /// UI의 활성화 여부를 제어한다.
    /// </summary>
    private void ToggleUI()
    {
        _ui.SetActive(!_ui.activeSelf);
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
