using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisionscript : MonoBehaviour
{
    public VillageBoss villageBoss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            villageBoss.playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            villageBoss.playerInRange = false;
        }
    }
}
