using Player.FSM.States;
using UnityEngine;

namespace Player.FSM
{
    public class KnightFSM : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private AttackController attackController;
        [SerializeField] private MovementController movementController;
        [Header("Movement")]
        [SerializeField] private float minRangeToPlayer;
        [Header("attacks")] 
        [SerializeField] private GameObject swordObject;
        [SerializeField] private float duration;
        [Header("idle")]
        [SerializeField] private float idleDuration;

        // FSM states
        private MeleeAttackState meleeAttackState;
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
            meleeAttackState = new MeleeAttackState(attackController, duration, swordObject);
            idleState = new IdleState(idleDuration);
            moveState = new MoveState(0, minRangeToPlayer, movementController);

            //Transitions setup

            //While Attacking:
            meleeAttackState.transitions.Add(new Transition(meleeAttackState.AttackOver, idleState));
            
            //while idling:
            idleState.transitions.Add(new Transition(idleState.IdleOver,  moveState));
            
            //while moving:
            moveState.transitions.Add(new Transition(moveState.PlayerInRange, meleeAttackState));

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
}