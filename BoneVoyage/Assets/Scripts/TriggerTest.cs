using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    public List<GameObject> enemies;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Animator>().SetTrigger("active");
                StartCoroutine(WaitForActive(collision.gameObject));
            }
        }
    }

    private IEnumerator WaitForActive(GameObject player)
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().chase = true;
            enemy.GetComponent<EnemyController>()._chaseB.target = player;
            enemy.GetComponent<EnemyController>().CheckEndingConditions();
        }
        Destroy(gameObject);
    }

}
