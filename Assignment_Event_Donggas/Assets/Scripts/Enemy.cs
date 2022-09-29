using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
#region FSM
    public enum EPlayerState
    {
        IDLE,
        BURN,
        POISONED,
        CURSED,
        MAX,
    }

    class FSM
    {
        private Dictionary<EPlayerState, State> _dicState;
        State _curState = null;

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

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }

    class IdleState : State
    {
        public override void OnEnter()
        {

        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }

    class BurnState : State
    {
        public override void OnEnter()
        {

        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }

    class PoisonedState : State
    {
        public override void OnEnter()
        {

        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }

    class CursedState : State
    {
        public override void OnEnter()
        {

        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
    #endregion
    //[SerializeField] private PlayerState _playerState;

    public int Health
    {
        get
        {
            return _health;
        }

        private set
        {
            _health = value;
        }
    }
    private int _health;

    private Material _material;
    private FSM _fsm;
    private void Awake()
    {
        _material = GetComponent<Material>();

        _fsm = new FSM();
        _fsm.Init();

        _fsm.AddState(EPlayerState.IDLE, new IdleState());

        _fsm.AddState(EPlayerState.BURN, new BurnState());

        _fsm.AddState(EPlayerState.POISONED, new PoisonedState());

        _fsm.AddState(EPlayerState.CURSED, new CursedState());

        _fsm.ChangeState(EPlayerState.IDLE);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}
