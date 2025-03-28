using System.Collections;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    public GameObject Enemy;

    public void Start()
    {
        Enemy = GameObject.Find("Skeleton_Rogue");
        if (Enemy == null)
        {
            Enemy = GameObject.Find("Skeleton_Minion");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<Animator>().SetTrigger("active");
            StartCoroutine(WaitForActive(collision.gameObject));
        }
    }

    private IEnumerator WaitForActive(GameObject player)
    {
        yield return new WaitForSeconds(2);

        Enemy.GetComponent<EnemyController>().chase = true;
        Enemy.GetComponent<EnemyController>()._chaseB.target = player;
        Enemy.GetComponent<EnemyController>().CheckEndingConditions();

        Destroy(gameObject);
    }

}
