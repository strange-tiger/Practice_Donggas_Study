using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region FSM
public enum EPlayerState
{
    BURNING,
    POISON,
    CURSE,
    FIST,
    MAX,
}

class FSM
{
    public Action<EPlayerState> OnStateChanged;
    public EPlayerState CurState
    {
        get
        {
            return _curStateTag;
        }
        set
        {
            _curStateTag = value;
            OnStateChanged.Invoke(_curStateTag);
        }
    }
    private EPlayerState _curStateTag;

    private Dictionary<EPlayerState, State> _dicState;
    private State _curState = null;

    public void Init()
    {
        _dicState = new Dictionary<EPlayerState, State>();
    }

    public void OnUpdate()
    {
        _curState.OnUpdate();
    }

    public void AddState(EPlayerState tag, State state)
    {
        Debug.Assert(state != null, "매개변수에 존재하지 않는 State 할당");

        state.Init(this);
        _dicState[tag] = state;
    }

    public void ChangeState(EPlayerState tag)
    {
        if (_curState != null)
        {
            _curState.OnExit();
        }

        CurState = tag;
        _curState = _dicState[tag];
        _curState.OnEnter();
    }
}

abstract class State
{
    protected FSM fsm;
    public void Init(FSM fsm)
    {
        this.fsm = fsm;
    }

    public void ChangeStateByKey()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            fsm.ChangeState(EPlayerState.BURNING);
        }
        if (Input.GetKey(KeyCode.W))
        {
            fsm.ChangeState(EPlayerState.POISON);
        }
        if (Input.GetKey(KeyCode.E))
        {
            fsm.ChangeState(EPlayerState.CURSE);
        }
        if (Input.GetKey(KeyCode.R))
        {
            fsm.ChangeState(EPlayerState.FIST);
        }
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}

class BurningState : State
{
    public override void OnEnter()
    {
            
    }

    public override void OnUpdate()
    {
        ChangeStateByKey();
    }

    public override void OnExit()
    {

    }
}

class PoisonState : State
{
    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        ChangeStateByKey();
    }

    public override void OnExit()
    {

    }
}

class CurseState : State
{
    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        ChangeStateByKey();
    }

    public override void OnExit()
    {

    }
}

class FistState : State
{
    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        ChangeStateByKey();
    }

    public override void OnExit()
    {

    }
}
#endregion
public class PlayerState : MonoBehaviour
{
    public event Action<EPlayerState> OnStateChanged;
    public EPlayerState CurState;

    private FSM _fsm;
    private void Awake()
    {
        _fsm = new FSM();
        _fsm.Init();

        _fsm.AddState(EPlayerState.BURNING, new BurningState());
        _fsm.AddState(EPlayerState.POISON, new PoisonState());
        _fsm.AddState(EPlayerState.CURSE, new CurseState());
        _fsm.AddState(EPlayerState.FIST, new FistState());
    }

    private void Start()
    {
        _fsm.ChangeState(EPlayerState.FIST);
    }

    private void OnEnable()
    {
        _fsm.OnStateChanged -= OnStateChanged.Invoke;
        _fsm.OnStateChanged += OnStateChanged.Invoke;
        _fsm.OnStateChanged -= SetCurState;
        _fsm.OnStateChanged += SetCurState;
    }

    private void SetCurState(EPlayerState state)
    {
        CurState = state;
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

    private void OnDisable()
    {
        _fsm.OnStateChanged -= OnStateChanged.Invoke;
        _fsm.OnStateChanged -= SetCurState;
    }
}
