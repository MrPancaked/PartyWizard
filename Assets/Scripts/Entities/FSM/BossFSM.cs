using System;
using Player;
using Player.FSM.States;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(MovementController))]

public class BossFSM : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private AttackController attackController;
    [SerializeField] private MovementController movementController;
    [Header("Movement")]
    [SerializeField] private float moveDuration;
    [SerializeField] private float minRangeToPlayer;
    [Header("Area Attacks")] 
    [SerializeField] private GameObject areaSpell;
    [SerializeField] private int areaAmount;
    [SerializeField] private float areaAngle;
    [SerializeField] private float areaDelayBetweenSpells;
    [SerializeField] private float areaAttackDuration;
    [Header("ranged attacks")] 
    [SerializeField] private GameObject rangedSpell;
    [SerializeField] private int rangedAmount;
    [SerializeField] private float rangedAngle;
    [SerializeField] private float rangedDelayBetweenSpells;
    [SerializeField] private float rangedAttackDuration;
    [Header("idle")]
    [SerializeField] private float idleDuration;

    // FSM states
    private AreaAttackState areaAttackState;
    private RangedAttackState rangedAttackState;
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
        areaAttackState = new AreaAttackState(attackController, areaAmount, areaAngle, areaDelayBetweenSpells, areaAttackDuration, areaSpell);
        rangedAttackState = new RangedAttackState(attackController, rangedAmount, rangedAngle, rangedDelayBetweenSpells, rangedAttackDuration, rangedSpell);
        idleState = new IdleState(idleDuration);
        moveState = new MoveState(moveDuration, minRangeToPlayer, movementController);

        //Transitions setup

        //While Attacking:
        areaAttackState.transitions.Add(new Transition(areaAttackState.AttackOver, rangedAttackState));
        rangedAttackState.transitions.Add(new Transition(rangedAttackState.AttackOver, moveState));
        
        //while idling:
        idleState.transitions.Add(new Transition(idleState.IdleOver,  areaAttackState));
        
        //while moving:
        moveState.transitions.Add(new Transition(moveState.MoveOver, idleState));
        moveState.transitions.Add(new Transition(moveState.PlayerInRange, idleState));

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
