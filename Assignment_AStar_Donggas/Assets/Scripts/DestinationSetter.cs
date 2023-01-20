using UnityEngine;
using Define;
using System;

public class DestinationSetter : MonoBehaviour
{
    [SerializeField] MapManager mapManager;
    [SerializeField] DestinationMarker destinationMarker;

    public event Action<Vector3> DestinationChanged;
    public Vector3? Destination { get; private set; } = new Vector3?();

    private void Update()
    {
        // 마우스 오른쪽 클릭을 하면 마우스로 가리킨 곳으로 목적지를 설정
        if (Input.GetMouseButtonDown(1))
        {
            Destination = SetDestination();
        }
    }

    /// <summary>
    /// 목적지를 마우스로 가리킨 곳으로 설정
    /// </summary>
    /// <returns>Vector3를 Nullable로 반환</returns>
    private Vector3? SetDestination()
    {
        // 마우스로 가리킨 위치를 받는다.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 마우스의 x, z 위치를 반올림
        float destPosX = RoundPosition(mousePos.x);
        float destPosZ = RoundPosition(mousePos.z);

        // 가리킨 타일의 위치를 계산
        Vector3? dest;

        // 가리킨 위치에 장애물이 있는지 판단
        // 장애물이 있으면 null 반환
        // 없으면 위치 반환
        if (mapManager.Map[(int)destPosX, (int)destPosZ])
        {
            dest = null;
        }
        else
        {
            dest = new Vector3(destPosX, PointTile.TILE_HEIGHT, destPosZ);
        }

        // 목적지 표시
        MarkDestination(dest);

        return dest;
    }

    private float RoundPosition(float pos)
    {
        pos = Mathf.Round(pos);

        pos = Mathf.Clamp(pos, 0f, (float)mapManager.MapSize - 1f);

        return pos;
    }

    /// <summary>
    /// destination Mark 오브젝트를 움직여 목적지 표시
    /// </summary>
    /// <param name="position"></param>
    private void MarkDestination(Vector3? position)
    {
        if (position.HasValue)
        {
            destinationMarker.Mark((Vector3)position);
            DestinationChanged.Invoke((Vector3)position);
        }
    }
}
