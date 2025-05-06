using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechanismTest : MonoBehaviour
{
    public VillageLVL2Manager levelManager;

    public GameObject objectToActivate;
    private bool playerInside = false;
    private bool interacted = false;

    private Player inputActions;

    private void Awake()
    {
        inputActions = new Player();
        inputActions.Interact.Enable();
        inputActions.Interact.Interact.performed += OnInteract;
        objectToActivate.SetActive(false);
    }

    private void OnDestroy()
    {
        inputActions.Interact.Interact.performed -= OnInteract;
        inputActions.Interact.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInside && !interacted)
        {
            interacted = true;
            objectToActivate.SetActive(true);
            levelManager.activateMechanism();
        }
    }
}
