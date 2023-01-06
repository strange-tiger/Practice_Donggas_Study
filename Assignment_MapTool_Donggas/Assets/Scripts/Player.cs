using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockType = Block.EBlockType;

public class Player : MonoBehaviour
{
    [SerializeField] Material _defaultMat;
    [SerializeField] Material _translucentMat;
    [SerializeField] float _speed;

    private const string BLOCK_LAYER = "Block";

    private BlockType _curBlockType;
    private MeshRenderer _renderer;
    private float _radius;
    private float _defaultSpeed;
    private float _curSpeed;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = _defaultMat;

        _radius = GetComponent<CapsuleCollider>().radius;

        _defaultSpeed = _speed * Time.deltaTime;
        _curSpeed = _defaultSpeed;
    }

    private void Update()
    {
        MoveInput();
    }

    /// <summary>
    /// WASD 입력에 따라 이동한다.
    /// </summary>
    private void MoveInput()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(_curSpeed * Vector3.forward);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(_curSpeed * Vector3.left);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(_curSpeed * Vector3.back);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(_curSpeed * Vector3.right);
    }

    /// <summary>
    /// 현재 충돌하고 있는 블록의 내부에 중앙점이 들어가면 
    /// 블록의 영향을 받는다.
    /// 즉, InfluencedByBlock을 호출한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(BLOCK_LAYER) && CheckInField(other.transform.position))
        {
            _curBlockType = other.GetComponent<Block>().CurType;

            InfluencedByBlock(_curBlockType, other.transform.position);
        }
    }

    /// <summary>
    /// 블록의 내부에 플레이어의 중앙점이 들어갔는지 판별한다.
    /// </summary>
    /// <param name="blockPos"></param>
    /// <returns></returns>
    private bool CheckInField(Vector3 blockPos)
    {
        float xDif = blockPos.x - transform.position.x;
        float zDif = blockPos.z - transform.position.z;

        if (xDif < -1f * _radius || xDif > _radius)
            return false;

        if (zDif < -1f * _radius || zDif > _radius)
            return false;

        return true;
    }

    /// <summary>
    /// 매개변수로 받는 블록의 타입에 따라 
    /// 플레이어가 받는 영향을 바꾼다.
    /// </summary>
    private const float RIVER_DEVELERATION = 0.5f;
    private void InfluencedByBlock(BlockType blockType, Vector3 blockPos)
    {
        ResetInfluence();

        switch (blockType)
        {
            case BlockType.NONE:
                break;
            case BlockType.BUSH:
                _renderer.material = _translucentMat;
                break;
            case BlockType.RIVER:
                _curSpeed *= RIVER_DEVELERATION;
                break;
            case BlockType.WALL:
                MoveOutToField(blockPos);
                break;
            default:
                Debug.LogError("Error: 존재하지 않는 블록 종류");
                break;
        }
    }

    /// <summary>
    /// 플레이어의 변화를 초기화한다.
    /// </summary>
    private void ResetInfluence()
    {
        _renderer.material = _defaultMat;
        _curSpeed = _defaultSpeed;
    }

    /// <summary>
    /// 벽 블록에 들어갔을때, 들어간 만큼 다시 나가게 하는 것으로 
    /// 블록을 지나갈 수 없도록 한다.
    /// </summary>
    /// <param name="blockPos"></param>
    private void MoveOutToField(Vector3 blockPos)
    {
        Vector3 direction = transform.position - blockPos;

        direction -= direction.y * Vector3.up;

        transform.Translate(direction);
    }
}
