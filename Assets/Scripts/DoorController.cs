using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour // probably just throw this out lol figure out something differently
{
    [SerializeField] private Transform otherDoorTransform;
    private Animator doorAnimator;
    private int roomNumber;
    private GameManager gameManager;
    
    private void OnDisable()
    {
        gameManager.RoomStartEvent -= CloseDoor;
        gameManager.RoomClearedEvent -= OpenDoor;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.RoomStartEvent += CloseDoor;
        gameManager.RoomClearedEvent += OpenDoor;
        
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    private void OpenDoor()
    {
        doorAnimator.SetBool("Open", true);
    }
    
    private void CloseDoor()
    {
        doorAnimator.SetBool("Open", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(gameManager.NewRound());
            other.transform.position = otherDoorTransform.position - otherDoorTransform.up;
            Debug.Log($"player entered {other.name} at position {other.transform.position}");
        }
    }
}
