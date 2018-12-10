using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove
{
    GameObject target;
    GameObject playerUnit;
    public static bool NPCMoving = false;
    public static bool NPC_Attacking = false;
    public static bool NPC_WillAttack = false;
    public bool animateMove = false;
    public bool animateAttack = false;

    List<Tile> selectedTiles;   

    private static int attackCount = 0;

	// Use this for initialization
	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(NPCMoving == false)
        {
            animateMove = false;
        }
        else
        {
           // animateMove = true;
        }

        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)  // Is it my turn
        {   // No, do nothing.
            return;
        }

        if (newUnitTurn)
        {   // Just got turn. Find path and start moving
            NPCMoving = false;
            //animateMove = false;
            selectedTiles = FindSelectableTiles(gameObject);  // Shows all potential target tile moves
            FindNearestTarget();    // Find nearest target "Player"
            CalculatePath();        // Calculate A* path to nearest Player
            //selectedTiles = FindSelectableTiles(gameObject);  // Shows all potential target tile moves
            newUnitTurn = false;
            moving = true;
            return;
        }

        if (moving)
        {   // Continue moving to target tile
            animateMove = true;
            Move(this.gameObject);
            NPCMoving = true;
            PlayerMove.playerMoving = false;
            PlayerMove.playerAttacking = false;
            return;
        }

        if (attacking)
        {
            //animateMove = false;
            NPC_Attacking = true;
            NPCAttacksPlayer(playerUnit);
        }

	}

    void CalculatePath()
    {
        //Tile targetTile = GetTargetTile(target);
        FindPath(actualTargetTile);   // Perform A* search
    }

    // This module looks for player objects. If there is a player within my range
    // than npc attack mode is set. Otherwise, npc will move toward closest player.
    void FindNearestTarget()
    {   // Player objects are enemies for NPC. Therefore,
        // create an array of targets with Player tags. 
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        // Simplest AI. Look for nearest player unit. Therefore, 
        // distance will be used to find an attackable player
        GameObject nearest = null;
       // GameObject removeThis = null;
        GameObject smallestHealthObj = null;
        //float distance = Mathf.Infinity;
        int smallestHealth = 1000;
        int smallestDefense = 1000;
        int myHealth = this.GetComponent<Unit>().GetHealth();
       // Debug.Log("My health:" + myHealth);
        float myRange = this.GetComponent<Unit>().GetRange();
        //Debug.Log("My range:" + myRange);
        //int highestHealth = 0;

        List<GameObject> posTarget = new List<GameObject>();
        List<GameObject> allies = new List<GameObject>(); //This the list of the NPCs friends

        if (myHealth > 3)
        {
            //Debug.Log(this.gameObject + " Health is greater than 3...");
            float smallestDistance = Mathf.Infinity;
            List<GameObject> tempList = new List<GameObject>(targets);
            foreach (GameObject obj in tempList)
            {
                float d = Vector3.Distance(transform.position, obj.transform.position);
               // Debug.Log("Distance between " + obj + " and " + this.gameObject);
                int objHealth = obj.GetComponent<Unit>().GetHealth();
               // Debug.Log("Object's Health:" + objHealth);
                int objDefense = obj.GetComponent<Unit>().GetDefense();
              //  Debug.Log("Object's Defense:" + objDefense);
                

                if (d <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                {
                    //add all enemies that are within the unit's potential range
                    posTarget.Add(obj);
                }
                
            }

            if (posTarget.Count != 0)
            {
                //compare the enemies' current health and defense to select the weakest enemy within range
                do
                {
                    /*List<GameObject> tempList = new List<GameObject>();
                    foreach(GameObject i in posTarget)
                    {
                        tempList.Add(i);
                    }*/
                     tempList = new List<GameObject>(posTarget);
                    //decide to avoid "foreach" since we have an issue with Enumeration that a for loop will side step
                    
                    foreach (GameObject i in tempList)
                    {
                        if (i.GetComponent<Unit>().health <= smallestHealth)
                        {
                            smallestHealth = i.GetComponent<Unit>().health;
                        }
                        else
                        {
                            posTarget.Remove(i); //was posTarget
                        }
                    }
                    //Debug.Log("(Health)Number of elements in the posTarget: " + posTarget.Count);
                    tempList = new List<GameObject>(posTarget);
                    foreach (GameObject i in tempList)
                    {
                        if (i.GetComponent<Unit>().defense <= smallestDefense)
                        {
                            smallestDefense = i.GetComponent<Unit>().defense;
                        }
                        else
                        {
                            posTarget.Remove(i);
                        }
                    }
                   // Debug.Log("(Def)Number of elements in the posTarget: " + posTarget.Count);
                    tempList = new List<GameObject>(posTarget);
                    foreach (GameObject i in tempList)
                    {
                        float d = Vector3.Distance(transform.position, i.transform.position);
                        if (d <= smallestDistance)
                        {
                            smallestDistance = d;
                        }
                        else
                        {
                            posTarget.Remove(i);
                        }
                    }
                    //Debug.Log("(Dist)Number of elements in the posTarget: " + posTarget.Count);

                    if (posTarget.Count > 1)
                    {
                        posTarget.RemoveAt(0);
                    }
                   // Debug.Log("(Remove(0))Number of elements in the posTarget: " + posTarget.Count);

                } while (posTarget.Count > 1);

                posTarget.ToArray(); //issue here, all the manipulatin we did was only to tempList which is now a different length
                nearest = posTarget[0];
                //Debug.Log(nearest + " is Target");
            }
            else
            {
                 tempList = new List<GameObject>(targets);
                foreach (GameObject obj in tempList)
                {
                    if (obj.GetComponent<Unit>().health <= smallestHealth)
                    {
                        smallestHealth = obj.GetComponent<Unit>().health;
                        posTarget.Add(obj);
                    }
                    else
                    {
                        posTarget.Remove(obj);
                    }
                }
                tempList = new List<GameObject>(posTarget);
                foreach (GameObject i in tempList)
                {
                    if (i.GetComponent<Unit>().health <= smallestHealth)
                    {
                        smallestHealth = i.GetComponent<Unit>().health;
                    }
                    else
                    {
                        posTarget.Remove(i);
                    }
                }
                tempList = new List<GameObject>(posTarget);
                foreach (GameObject i in tempList)
                {
                    if (i.GetComponent<Unit>().defense <= smallestDefense)
                    {
                        smallestDefense = i.GetComponent<Unit>().defense;
                    }
                    else
                    {
                        posTarget.Remove(i); //tried basic list copying, worked until we got to this line
                    }
                }
                tempList = new List<GameObject>(posTarget);
                foreach (GameObject i in tempList)
                {
                    float d = Vector3.Distance(transform.position, i.transform.position);
                    if (d <= smallestDistance)
                    {
                        smallestDistance = d;
                    }
                    else
                    {
                        posTarget.Remove(i);
                    }
                }

                posTarget.ToArray();
                nearest = posTarget[0];
               // Debug.Log(nearest + " is Target");
            }
            

        }//end of if health >3 
        else
        {
            //Debug.Log(this.gameObject + " Health is less than 3...");
            List<GameObject> tempList = new List<GameObject>(targets);
            if (this.GetComponent<Unit>().type == "Range")
            {
               // Debug.Log(this.gameObject + " Is Range");
                foreach (GameObject o in tempList)
                {
                    float d = Vector3.Distance(transform.position, o.transform.position);

                    if (d <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                    {
                        if(o.GetComponent<Unit>().health < 3)
                        {
                            posTarget.Add(o); 
                        }
                    }
                }
                
                if(posTarget.Count !=0)
                {
                    float d = Mathf.Infinity;
                    tempList = new List<GameObject>(posTarget);
                    foreach (GameObject o in tempList)
                    {
                        //float d = Vector3.Distance(transform.position, o.transform.position);
                        if (o.GetComponent<Unit>().health == smallestHealth)
                        {
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                smallestHealth = o.GetComponent<Unit>().health;
                            }

                        }
                       else if (o.GetComponent<Unit>().health < smallestHealth)
                        {
                            d = Vector3.Distance(transform.position, o.transform.position);
                            nearest = o;
                            smallestHealth = o.GetComponent<Unit>().health;

                        }
                        else
                        {
                           // Debug.Log("Didn't Lowest Health in Ranger.Health < 3");
                        }

                    }
                   // Debug.Log(nearest + " is Target");
                }
                else
                {
                    if (allies.Contains(this.gameObject))
                    {
                        allies.Remove(this.gameObject);
                    }
                    float d = Mathf.Infinity;
                    if (allies.Count != 0)
                    {
                        tempList = new List<GameObject>(allies);
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                //smallestHealth = o.GetComponent<Unit>().health;
                            }

                        }
                    }
                    else
                    {
                        tempList = new List<GameObject>(targets);
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                            }

                        }
                    }
                }
            }
            else if(this.GetComponent<Unit>().type == "Armour")
            {
                bool foundEnemy = false;
                float d = Mathf.Infinity;
                tempList = new List<GameObject>(targets);
                foreach (GameObject o in tempList)
                {
                    if(Vector3.Distance(transform.position, o.transform.position) <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                    {
                        float dist = Vector3.Distance(transform.position, o.transform.position);
                        if (dist <= d)
                        {
                            foundEnemy = true; //y
                            d = dist;
                            nearest = o;
                            //smallestHealth = o.GetComponent<Unit>().health;
                        }                    
                    }
                    else
                    {
                        foundEnemy = false; 
                       // Debug.Log("Armour unit health less than 3; No enemy target found");
                        
                    }
                }
                if (!foundEnemy)
                {
                    if (allies.Contains(this.gameObject))
                    {
                        allies.Remove(this.gameObject);
                    }
                    if (allies.Count != 0)
                    {
                        tempList = new List<GameObject>(allies);
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                //smallestHealth = o.GetComponent<Unit>().health;
                            }

                        }
                    }
                    else
                    {
                        tempList = new List<GameObject>(targets);
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                            }

                        }
                    }
                }
                
            }
            else if(this.GetComponent<Unit>().type == "Melee")
            {
                float d = Mathf.Infinity;
                bool foundEnemy = false;
                tempList = new List<GameObject>(targets);
                foreach (GameObject o in tempList)
                {
                    float dist = Vector3.Distance(transform.position, o.transform.position);

                    if (dist <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                    {
                        if (o.GetComponent<Unit>().health == smallestHealth)
                        {
                            //float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                smallestHealth = o.GetComponent<Unit>().health;
                            }

                        }
                        else if (o.GetComponent<Unit>().health < smallestHealth)
                        {
                            d = dist;
                            nearest = o;
                            smallestHealth = o.GetComponent<Unit>().health;
                        }

                        foundEnemy = true;
                    }
                }
                if (!foundEnemy)
                {
                    tempList = new List<GameObject>(targets);
                    foreach (GameObject o in tempList)
                    {
                        // float d = Vector3.Distance(transform.position, o.transform.position);
                        float dist = Vector3.Distance(transform.position, o.transform.position);
                        if (dist <= d)
                        {
                            d = dist;
                            nearest = o;
                            smallestHealth = o.GetComponent<Unit>().health;
                        }

                    }
                }
            }
            if (this.GetComponent<Unit>().type == "Commander")
            {
                tempList = new List<GameObject>(targets);
                foreach (GameObject o in tempList)
                {
                    float d = Vector3.Distance(transform.position, o.transform.position);

                    if (d <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                    {
                        if (o.GetComponent<Unit>().health < 6)
                        {
                            posTarget.Add(o);
                        }
                    }
                }

                if (posTarget.Count != 0)
                {
                    float d = Mathf.Infinity;
                    tempList = new List<GameObject>(posTarget);
                    foreach (GameObject o in tempList)
                    {
                        //float d = Vector3.Distance(transform.position, o.transform.position);
                        if (o.GetComponent<Unit>().health == smallestHealth)
                        {
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                smallestHealth = o.GetComponent<Unit>().health;
                            }

                        }
                        else if (o.GetComponent<Unit>().health < smallestHealth)
                        {
                            d = Vector3.Distance(transform.position, o.transform.position);
                            nearest = o;
                            smallestHealth = o.GetComponent<Unit>().health;

                        }
                        else
                        {
                            //Debug.Log("Didn't Lowest Health in commander.Health < 6");
                        }
                    }
                }
                else
                {
                    if (allies.Contains(this.gameObject))
                    {
                        allies.Remove(this.gameObject);
                    }
                    float d = Mathf.Infinity;
                    if (allies.Count != 0)
                    {
                        tempList = new List<GameObject>(allies);
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                //smallestHealth = o.GetComponent<Unit>().health;
                            }

                        }
                    }
                    else
                    {
                        tempList = new List<GameObject>(targets);
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                            }

                        }
                    }
                }
            }

        }
        if (smallestHealthObj == null)
        {
            // Set nearest player as target
            playerUnit = nearest;
            target = nearest;
            FindOffRangeTargetTile(target);
        }
        else
        {   // There is a player within my range.
            // Set it as target to attack.
            playerUnit = nearest;
            target = nearest;
            //Debug.Log("Set Player Tile attackable----");
            willAttackAfterMove = true;
            NPC_WillAttack = true;
            FindInRangeTargetTile(target);
        }

    }

    void FindOffRangeTargetTile(GameObject nearestPlayer)
    {
        if(nearestPlayer == null)
        {
            TurnManager.EndTurn();
        }
        float distance = Mathf.Infinity;
        // Find the closest selectable tile to nearest player
        foreach (Tile t in selectedTiles)
        {
            float d = Vector3.Distance(t.transform.position, nearestPlayer.transform.position);

            if (d < distance)
            {
                distance = d;
                actualTargetTile = t;
            }
        }
        actualTargetTile.target = true;
    }

    void FindInRangeTargetTile(GameObject nearestPlayer)
    {
        float distance = Mathf.Infinity;
        Tile playerTile = GetTargetTile(nearestPlayer);
        // Find the closest adjacent selectable tile to attackable player
        if(this.GetComponent<Unit>().range == 1)
        {
            foreach(Tile t in playerTile.adjacencyList)
            {
                if (!t.selectable)
                {
                    continue; 
                }
                actualTargetTile = t;
            }
            actualTargetTile.target = true;
        }
        foreach (Tile t in playerTile.adjacencyList)
        {
            if (!t.selectable) continue;

            float d = Vector3.Distance(t.transform.position, playerTile.transform.position);

            if (d < distance)
            {
                distance = d;
                actualTargetTile = t;
            }
        }
        actualTargetTile.target = true;
    }

    void NPCAttacksPlayer(GameObject target)
    {

        PlayerCombat pc = new PlayerCombat();

        int dmg = pc.AttackPhase(true, this.GetComponent<Unit>().GetAttack(), this.GetComponent<Unit>().GetDefense(), playerUnit.GetComponent<Unit>().GetAttack(), playerUnit.GetComponent<Unit>().GetDefense());
        playerUnit.GetComponent<Unit>().TakeDmg(dmg);
        Debug.Log("NPC hits for: " + dmg);
        Debug.Log("Player has: " + this.GetComponent<Unit>().GetHealth());
        attacking = false;
        willAttackAfterMove = false;
        NPC_WillAttack = false;
        TurnManager.EndTurn();

        // NPC Attacks nearest Player named target
        //Debug.Log("NPC attacking player ........");
        //attackCount++;      // Increase attack count for testing
        //if (attackCount < 5)
        //{
        //Debug.Log("NPC attacking " + attackCount);
        //}
        //else
        //{
        //attackCount = 0;

        // Run following when attack completes for turning to another unit
        //Debug.Log("NPC attacks ended....Turning to another unit");
        //attacking = false;
        //TurnManager.EndTurn();
        //}
    }

    public void SetAnimateAttack()
    {
        animateAttack = false;
    }
}
