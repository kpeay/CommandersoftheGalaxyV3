using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage: MonoBehaviour {

    public int damage;
    public Text DamageText;
    public float targetTime = 2f;
	
	// Update is called once per frame
	void Update ()
    {
      //  targetTime -= Time.deltaTime;

      //  if (targetTime <= 0.0f)
      //  {
      //      TimerEnded();
      //  }
	}

    public void ChangeDamage(bool takingDmg)
    {
        if (takingDmg)
        { 
            SetDamageText();
            DamageText.GetComponent<Text>().enabled = true;
        }
    }

    void SetDamageText()
    {
        DamageText.text = "-" + damage.ToString();
    }

    void TimerEnded()
    {
        DamageText.GetComponent<Text>().enabled = false;
    }

    public void SetDamage(int dam)
    {
        damage = dam;
    }

}
