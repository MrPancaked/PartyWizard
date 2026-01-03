using System;
using Player;
using Player.FSM.States;
using UnityEngine;

[RequireComponent(typeof(AttackController))]
public class BossFSM : MonoBehaviour
{
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

    // The current state
    [SerializeReference] private State currentState;

    private void Start()
    {
        //Create states
        attackState = new AttackState(attackController ,amount,  angle, delayBetweenSpells, attackDuration);
        idleState = new IdleState(idleDuration);

        //Transitions setup

        //While Attacking:
        attackState.transitions.Add(new Transition(attackState.AttackOver, idleState));
        
        //while idling:
        idleState.transitions.Add(new Transition(idleState.IdleOver,  attackState));

        //Default state is idleState.
        currentState = idleState;
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
