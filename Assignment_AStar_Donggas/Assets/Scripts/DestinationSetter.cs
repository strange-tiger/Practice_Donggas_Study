using UnityEngine;
using Define;

public class DestinationSetter : MonoBehaviour
{
    [SerializeField] MapManager mapManager;
    
    public Vector3? Destination { get; private set; } = new Vector3();

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
        float destPosX = Mathf.Round(mousePos.x);
        float destPosZ = Mathf.Round(mousePos.z);

        // 가리킨 타일의 위치를 계산
        Vector3? dest = new Vector3(destPosX, PointTile.tileHeight, destPosZ);

        // 가리킨 위치에 장애물이 있는지 판단
        // 장애물이 있으면 null 반환
        if (mapManager.Map[(int)destPosX, (int)destPosZ])
        {
            dest = null;
        }

        return dest;
    }
}
