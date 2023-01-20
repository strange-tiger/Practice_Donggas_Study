using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscUI : MonoBehaviour
{
    [SerializeField] GameObject UI;

    [Header("Initialize")]
    [SerializeField] MapManager mapManager;
    [SerializeField] DestinationMarker marker;
    [SerializeField] Pathfinding path;
    [SerializeField] PlayerMove player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.SetActive(!UI.activeSelf);
        }
    }

    public void Restart()
    {
        mapManager.Reset();
        marker.Reset();
        path.Reset();
        player.Reset(mapManager.SendInitPlayerPos());
    }

    public void Quit()
    {
        Application.Quit();
    }
}
