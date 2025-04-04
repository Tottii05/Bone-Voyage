using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    public delegate void SpikeTriggerDelegate();
    public static event SpikeTriggerDelegate OnSpikeTriggerActivated;
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            OnSpikeTriggerActivated?.Invoke();
            hasBeenTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasBeenTriggered = false;
        }
    }
}