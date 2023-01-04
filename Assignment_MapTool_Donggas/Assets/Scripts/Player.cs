using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Block;

public class Player : MonoBehaviour
{
    [SerializeField] Material _defaultMat;
    [SerializeField] Material _translucentMat;
    [SerializeField] float _speed;

    private Block _curBlock;
    private MeshRenderer _renderer;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _renderer = GetComponent<MeshRenderer>();

        _renderer.material = _defaultMat;

        _speed *= Time.deltaTime;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(_speed * Vector3.forward);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(_speed * Vector3.left);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(_speed * Vector3.back);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(_speed * Vector3.right);
    }

    private void OnTriggerEnter(Collider other)
    {
        //_curBlock = other.GetComponent<Block>();

        
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
