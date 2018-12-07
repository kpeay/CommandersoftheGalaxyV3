using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {

    public const int maxHealth = 10;
    public int currentHealth = maxHealth;
    public RectTransform healthBar;

    public void SetCurrentHealth(int dmg)
    {
        currentHealth = currentHealth - dmg;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }

        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

}
