using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotion : MonoBehaviour
{
    public float healthAmount = 30f;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<ACharacter>().health + healthAmount > 100)
            {
                other.GetComponent<ACharacter>().health = 100;
                GameObject.Find("HealthBar").GetComponent<Slider>().value = 100;
            }
            else
            {
                other.GetComponent<ACharacter>().health += healthAmount;
                GameObject.Find("HealthBar").GetComponent<Slider>().value = other.GetComponent<ACharacter>().health / 100;
            }
            Destroy(gameObject);
        }
    }
}
