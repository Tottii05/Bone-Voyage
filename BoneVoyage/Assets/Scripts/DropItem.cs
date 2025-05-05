using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1, 0);
    public void Drop(GameObject item, int amount, int probability)
    {
        if (Random.Range(0, 100) < probability)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject droppedItem = Instantiate(item, transform.position + offset, Quaternion.identity);
            }
        }
    }
}
