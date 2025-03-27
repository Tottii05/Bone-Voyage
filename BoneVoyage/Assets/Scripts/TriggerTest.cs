using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TriggerTest : MonoBehaviour
{
    public GameObject Enemy;
    public void Start()
    {
        Enemy = GameObject.Find("Skeleton_Warrior");
    }
    private void OnTriggerEnter(Collider collision)
    {
        Enemy.GetComponent<Animator>().SetTrigger("entry");
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<EnemyController>().chase = true;
            Enemy.GetComponent<EnemyController>()._chaseB.target = collision.gameObject;
            Enemy.GetComponent<EnemyController>().CheckEndingConditions();
        }
    }
}
