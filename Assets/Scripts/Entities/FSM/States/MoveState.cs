using Player;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveState : State
{
    private MovementController movementController;
    private MovementData movementData;
    private float moveDuration;
    
    public MoveState(float pDuration, MovementController pMvementController)
    {
        moveDuration = pDuration;
        movementController = pMvementController;
    }

    public override void Step()
    {
        base.Step();
        movementController.moveDirection = (PlayerController.Instance.transform.position - movementController.transform.position).normalized;
        Debug.Log($"set boss movedirection to {movementController.moveDirection}");
    }

    public override void Exit()
    {
        movementController.moveDirection = Vector2.zero;
    }

    public bool MoveOver()
    {
        return Time.time > startTime + moveDuration;
    }

}
