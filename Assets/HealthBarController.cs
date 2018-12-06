using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {

    public const int maxHealth = 10;
    public int currentHealth = maxHealth;

    public void setCurrentHealth(int unitCurHealth)
    {
        currentHealth = unitCurHealth;
    }

}
