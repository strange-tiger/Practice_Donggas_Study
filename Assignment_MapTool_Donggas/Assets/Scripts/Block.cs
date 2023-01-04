using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum EBlockType
    {
        NONE,
        BUSH,
        RIVER,
        WALL,
        MAX
    }

    [SerializeField] Material[] _materials;

    public EBlockType CurType
    {
        get => _curType;
        set => ChangeType(value);
    }
    private EBlockType _curType;

    private delegate void BlockFunction();

    private MeshRenderer _renderer;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _renderer = GetComponent<MeshRenderer>();

        CurType = EBlockType.NONE;
    }

    private void ChangeType(EBlockType type)
    {
        if (type == _curType) return;

        int typeNum = (int)type;
        _curType = type;

        _renderer.material = _materials[typeNum];
    }
}
