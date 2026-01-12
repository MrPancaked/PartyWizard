using Player;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveState : State
{
    private MovementController movementController;
    private MovementData movementData;
    private float moveDuration;
    private float minRangeToPlayer;
    
    public MoveState(float pDuration, float pMinRangeToPlayer, MovementController pMvementController)
    {
        moveDuration = pDuration;
        minRangeToPlayer = pMinRangeToPlayer;
        movementController = pMvementController;
    }

    public override void Step()
    {
        base.Step();
        movementController.moveDirection = (PlayerController.Instance.transform.position - movementController.transform.position).normalized;
    }

    public override void Exit()
    {
        movementController.moveDirection = Vector2.zero;
    }

    public bool MoveOver()
    {
        return Time.time > startTime + moveDuration;
    }

    public bool PlayerInRange()
    {
        return (PlayerController.Instance.transform.position - movementController.transform.position).magnitude <= minRangeToPlayer;
    }
}
