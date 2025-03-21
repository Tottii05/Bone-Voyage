using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour
{
    public GameObject mageParent;
    public bool isShieldActive = false;

    public void Awake()
    {
        mageParent = transform.parent.gameObject;
    }

    public void OnEnable()
    {
        isShieldActive = true;
        mageParent.GetComponent<Mage>().shielded = isShieldActive;
    }
    public void OnDisable()
    {
        isShieldActive = false;
        mageParent.GetComponent<Mage>().shielded = isShieldActive;
    }
}
