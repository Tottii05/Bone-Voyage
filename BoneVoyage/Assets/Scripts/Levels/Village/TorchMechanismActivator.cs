using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TorchMechanismActivator : MonoBehaviour
{
    //Script for when only 1 torch is needed to activate the mechanism
    public GameObject doorToOpen;

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
            StartCoroutine(OpenDoor(doorToOpen));
        }
    }

    IEnumerator OpenDoor(GameObject door)
    {
        float duration = 2f;
        float time = 0f;

        Quaternion startRotation = door.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 120f, 0);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            door.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        door.transform.rotation = endRotation;
    }
}
