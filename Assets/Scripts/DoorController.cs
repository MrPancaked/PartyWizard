using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the opening and closing of the dungeon doors using animation states.
/// If game gets developed further each door would have different waves attached to them so choosing a door would actually matter
/// </summary>
public class DoorController : MonoBehaviour
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

    //When player enters a door
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = otherDoorTransform.position - otherDoorTransform.up;
            StartCoroutine(gameManager.NewRound());
            Debug.Log($"player entered {other.name} at position {other.transform.position}");
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.doorCloseSounds, Vector2.zero);
        }
    }
}
