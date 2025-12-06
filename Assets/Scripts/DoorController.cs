using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour // probably just throw this out lol figure out something differently
{
    [SerializeField] private GameObject doorParent;
    private GameManager gameManager;
    public GameObject[] doorObjects;
    private Animator[] doorAnimators;
    private int roomNumber;
    
    private void OnDisable()
    {
        gameManager.RoomClearedEvent -= OpenDoors;
        gameManager.RoomStartEvent -= CloseDoors;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.RoomClearedEvent += OpenDoors;
        gameManager.RoomStartEvent += CloseDoors;
        
        doorObjects = doorParent.GetComponentsInChildren<GameObject>();
        doorAnimators = doorParent.GetComponentsInChildren<Animator>();
    }

    private void OpenDoors()
    {
        foreach (Animator door in doorAnimators)
        {
            door.SetBool("Open", true);
        }
    }
    
    private void CloseDoors(List<GameObject> enemies)
    {
        foreach (Animator door in doorAnimators)
        {
            door.SetBool("Open", false);
        }
    }
}
