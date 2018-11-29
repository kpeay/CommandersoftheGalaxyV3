using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    int playerAtk;
    int playerDef;
    int aiAtk;
    int aiDef;
    

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetStats(int unitAtk, int unitDef, int aiUnitAtk, int aiUnitDef)
    {
        unitAtk = playerAtk;
        unitDef = playerDef;
        aiUnitAtk = aiAtk;
        aiUnitDef = aiDef;
    }


    public int AttackPhase(bool initiator, int playerAtk, int playerDef, int aiAtk, int aiDef)
    {
        int damage;    
        //bool winner = false; //If false, defender wins; If true, attacker wins
        bool init = initiator; //Used to determine whether the player or NPC is the initiator. False = NPC; True = Player

        if (init)
        {
           damage = Combat(playerAtk, aiDef);
        }
        else
        {
            damage = Combat(aiAtk, playerDef);
        }

        return damage;
    }

    int Combat(int attack, int defense)
    {

        int attackAverage = 0;
        int defenseAverage = 0;

        //Determine strength of attack using units attack and dice rolls.
        for (int i = 0; i < attack; i++)
        {
            float atkNum = Random.Range(1f, 6f);
            Debug.Log("Attack roll " + i + ": " + atkNum);
            if (atkNum < 3f)
            {
                attackAverage++;
            }
        }

        //Determine strength of defense using units defense and dice rolls.
        for (int i = 0; i < defense; i++)
        {
            float defNum = Random.Range(1f, 6f);
            Debug.Log("Defence roll " + i + ": " + defNum);
            if (defNum > 4f)
            {
                defenseAverage++;
            }
        }

        Debug.Log("DefAVG: " + defenseAverage + "\n AtkAVG: " + attackAverage);
        //When atk is greater than def, attacker wins. Otherwise, the defender wins. 
        if (attackAverage > defenseAverage)
        {
            return attackAverage - defenseAverage;
        }
        else
        {
            return -1;
        }
    }



}
