using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageLVL2Manager : MonoBehaviour
{
    public int mechanismsActivated = 0;
    public GameObject door1;
    public GameObject door2;

    private bool door1open = false;
    private bool door2open = false;

    void Update()
    {
        if (mechanismsActivated == 1 && !door1open)
        {
            door1open = true;
            StartCoroutine(OpenDoor(door1));
        }

        if (mechanismsActivated == 6 && !door2open)
        {
            door2open = true;
            StartCoroutine(OpenDoor(door2));
        }
    }

    public void activateMechanism()
    {
        mechanismsActivated++;
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
