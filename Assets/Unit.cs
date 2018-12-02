using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int attack = 4;
    public int defense = 4;
    public int move = 5;
    public float range = 5f; //0.5 is equal to one tile away from unit
    public int health = 10; //set back to 10 later

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
     

	}

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetMove()
    {
        return move;
    }

    public float GetRange()
    {
        return range;
    }

    public int GetHealth()
    {
        return health;
    }

    //Used to change the attack of an unit
    public void SetAttack(int atk)
    {
        attack = atk;
    }

    //Used to change the defense of an unit
    public void SetDefense(int def)
    {
        defense = def;
    }

    //Used to change the movement of an unit
    public void SetMove(int mv)
    {
        move = mv;
    }

    //Used to change the range of an unit
    public void SetRange(float rng)
    {
        range = rng;
    }

    //Used to change the health of an unit
    public void SetHealth(int hlth)
    {
        health = hlth;
    }

    //Used to change the tag of an unit
    public void SetTag(string tg)
    {
        
        transform.gameObject.tag = tg;
    }

    //called when ever a unit takes damage
    public void TakeDmg(int dmg)
    {
        health = health - dmg;

        //if health is less than 1 the unit is killed
        if(health < 1)
        {
            // Remove this object from Turn Manager data structures
            TurnManager.RemoveUnit(this.gameObject);

            //destroy this instance of game object
            Destroy(this.gameObject);
        }
    }
}
