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

        Debug.Log(_curBlockType);
    }

    private void ResetBlockType() => _curBlockType = BlockType.MAX;

    private const float RAY_MAX_DISTANCE = Mathf.Infinity;
    private void InputClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Debug.Log("Here");
        if (_curBlockType == BlockType.MAX) return;
        Debug.Log("Now");
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
