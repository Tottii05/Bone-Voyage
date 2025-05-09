using UnityEngine;
[CreateAssetMenu(fileName = "Die", menuName = "StatesSO/Die")]
public class Die : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        ec.animator.SetTrigger("die");
        if (RandomNumber(0, 100) <= 60)
        {
            if (RandomNumber(0, 100) <= 60)
            {
                ec.GetComponent<DropItem>().Drop(ec.coinPrefab, RandomNumber(1, 6), 100);
            }
            else
            {
                ec.GetComponent<DropItem>().Drop(ec.potionPrefab, 1, 100);
            }
        }
        ec.Die();
    }

    public override void OnStateExit(EnemyController ec)
    {

    }

    public override void OnStateUpdate(EnemyController ec)
    {

    }

    public int RandomNumber(int min, int max)
    {
        return Random.Range(min, max);
    }
}