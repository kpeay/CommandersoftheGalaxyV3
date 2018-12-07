using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int attack = 4;
    public int defense = 4;
    public int move = 5;
    public float range = 5f; //0.5 is equal to one tile away from unit
    public int health = 10; //set back to 10 later
    public string type = "NoType"; //The type class of the Unit: Range, Armour, Melee, Commander
    public Tile tileBelow;
    public Tile tileBelowClicked;
    RaycastHit hitR;

	// Use this for initialization
	void Start () {
        GetTile();
	}
	
	// Update is called once per frame
	void Update () {

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitR, Mathf.Infinity))
        {
            if (hitR.collider.tag == "Tile")
            {
                tileBelow = hitR.collider.GetComponent<Tile>();

            }
            else
            {
                tileBelow = null;
            }

        }

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
        //bool takingDamage = true;
        //Damage dam = new Damage();
        // this.GetComponent<Damage>().SetDamage(dmg);
        //this.GetComponent<Damage>().ChangeDamage(takingDamage);
        this.GetComponent<HealthBarController>().SetCurrentHealth(dmg);
        health = health - dmg;

        //if health is less than 1 the unit is killed
        if(health < 1)
        {
            // Remove this object from Turn Manager data structures
            TurnManager.RemoveUnit(this.gameObject);
            Debug.Log("===>Unit Died " + this.gameObject.name);
            //destroy this instance of game object
            Destroy(this.gameObject);
        }
    }

    //store the tile the unit is standing on
    public Tile GetTile()
    {
        Tile tile;

        //Create raycast
        RaycastHit hit;

        //Check to see what is above tile. If object is above, return tag. 
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Tile")
            {
                tile = hit.collider.GetComponent<Tile>();

                Debug.Log("Tile returned with Unit.GetTile(): " + tile);
                
            }
            else 
            {
                Debug.Log("No tile was found with Unit.GetTile()");
                tile = null;
            }

        }
        else
        {
            Debug.Log("No tile was found with Unit.GetTile()");
            tile = null;
        }
        
        return tile;

    }
}
