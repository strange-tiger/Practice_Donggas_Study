using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockType = Block.EBlockType;

public class BlockController : MonoBehaviour
{
    [SerializeField] LayerMask _layer;

    private Camera _cam;
    private Vector3 _rayEndpointAdjustment;
    private BlockType _curBlockType;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _cam = Camera.main;
        _rayEndpointAdjustment = _cam.transform.position.y * 2f * Vector3.up;
        _curBlockType = BlockType.MAX;
    }

    private void Update()
    {
        InputBlockType();

        InputClick();
    }

    /// <summary>
    /// 입력에 맞춰 바꿀 블록의 타입(이하 현재 타입)을 지정한다.
    /// </summary>
    private void InputBlockType()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            _curBlockType = BlockType.NONE;
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            _curBlockType = BlockType.BUSH;
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            _curBlockType = BlockType.RIVER;
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            _curBlockType = BlockType.WALL;
    }

    /// <summary>
    /// 현재 타입을 MAX로 되돌린다.
    /// </summary>
    private void ResetBlockType() => _curBlockType = BlockType.MAX;

    /// <summary>
    /// 레이캐스트로 바꿀 블록을 지정한다.
    /// </summary>
    private const float RAY_MAX_DISTANCE = Mathf.Infinity;
    private void InputClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (_curBlockType == BlockType.MAX) return;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition) - _rayEndpointAdjustment;

        Ray ray = new Ray(_cam.transform.position, mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, RAY_MAX_DISTANCE, _layer.value))
        {
            Block block = hit.collider.GetComponent<Block>();

            block.CurType = _curBlockType;
        }
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        ResetBlockType();
    }
}
