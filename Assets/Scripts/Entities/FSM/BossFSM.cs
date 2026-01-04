using System;
using Player;
using Player.FSM.States;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(MovementController))]

public class BossFSM : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveDuration;
    [SerializeField] private MovementController movementController;
    [Header("attacks")] 
    [SerializeField] private AttackController attackController;
    [SerializeField] private int amount;
    [SerializeField] private float angle;
    [SerializeField] private float delayBetweenSpells;
    [SerializeField] private float attackDuration;
    [Header("idle")]
    [SerializeField] private float idleDuration;

    // FSM states
    private AttackState attackState;
    private IdleState idleState;
    private MoveState moveState;

    // The current state
    [SerializeReference] private State currentState;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        attackController = GetComponent<AttackController>();
    }
    private void Start()
    {
        //Create states
        attackState = new AttackState(attackController ,amount,  angle, delayBetweenSpells, attackDuration);
        idleState = new IdleState(idleDuration);
        moveState = new MoveState(moveDuration, movementController);

        //Transitions setup

        //While Attacking:
        attackState.transitions.Add(new Transition(attackState.AttackOver, moveState));
        
        //while idling:
        idleState.transitions.Add(new Transition(idleState.IdleOver,  attackState));
        
        //while moving:
        moveState.transitions.Add(new Transition(moveState.MoveOver, idleState));

        //Default state is idleState.
        currentState = idleState;
        currentState.Enter();
    }

    void Update()
    {
        currentState.Step();
        if (currentState.NextState() != null)
        {
            //Cache the next state, because after currentState.Exit, calling
            //currentState.NextState again might return null because of change
            //of context.
            State nextState = currentState.NextState();
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }
    }
}
