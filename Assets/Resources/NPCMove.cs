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
            this.animateMove = false;
        }
        else
        {
            //animateMove = true;
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
            this.animateMove = true;
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

        GameObject nearest = null;
        GameObject removeThis = null;
        GameObject smallestHealthObj = null;
        float distance = Mathf.Infinity;
        int smallestHealth = 1000;
        int smallestDefense = 1000;
        int myHealth = this.GetComponent<Unit>().GetHealth();
        Debug.Log("My health:" + myHealth);
        float myRange = this.GetComponent<Unit>().GetRange();
        Debug.Log("My range:" + myRange);
        bool unitOutOfRange = false;

        List<GameObject> posTarget = new List<GameObject>();
        List<GameObject> allies = new List<GameObject>(); //This the list of the NPCs friends

        //determine if the unit has low health or not
        if (myHealth > 3)
        {
            Debug.Log(this.gameObject + " Health is greater than 3...");
            float smallestDistance = Mathf.Infinity; //initialize the smallest distance to infinity
            List<GameObject> tempList = new List<GameObject>(targets); //create a copy of targets list for the foreach loops
            foreach (GameObject obj in tempList)
            {
                //find the distance from this unit to target unit
                float d = Vector3.Distance(transform.position, obj.transform.position);
                Debug.Log("Distance between " + obj + " and " + this.gameObject);

                //debug and verify that AI is correctly obtaining the health and defense of the enemy 
                int objHealth = obj.GetComponent<Unit>().GetHealth();
                Debug.Log("Object's Health:" + objHealth); 
                int objDefense = obj.GetComponent<Unit>().GetDefense();
                Debug.Log("Object's Defense:" + objDefense);
                

                if (d <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                {
                    //add all enemies that are within the unit's potential range
                    posTarget.Add(obj);
                }
                
            }

            //if there is at least one enemy within the potential range
            if (posTarget.Count != 0)
            {
                //compare the enemies' current health and defense to select the weakest enemy within range
                do
                {
                    //break the loop if there is one enemy in the list
                    if (posTarget.Count == 1)
                    {
                        break;
                    }

                    //set tempList to a copy of the possible targets
                    tempList = new List<GameObject>(posTarget);

                    //find the lowest health in the list and remove any enemy with health > lowest health
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

                    //break the loop if there is one enemy in the list
                    if(posTarget.Count == 1)
                    {
                        break;
                    }

                    //debug. make sure there are enemies still in the posTarget list
                    Debug.Log("(Health)Number of elements in the posTarget: " + posTarget.Count);

                    //reset the temp list to be the same as the posTarget list incase of changes
                    tempList = new List<GameObject>(posTarget);

                    //find the smallest defense and eliminate any enemy with defense > smallest def
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

                    //break the loop if there is one enemy in the list
                    if (posTarget.Count == 1)
                    {
                        break;
                    }

                    //debug. make sure there are enemies still in the posTarget List
                    Debug.Log("(Def)Number of elements in the posTarget: " + posTarget.Count);

                    //reset the temp list to be the same as the postTarget list in casse of changes
                    tempList = new List<GameObject>(posTarget);
                    
                    //find the closest enemy out of the remaining enemies 
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

                    //break the loop if there is one enemy in the list
                    if (posTarget.Count == 1)
                    {
                        break;
                    }

                    //debug. check to see if there are more or less than one enemy in the list
                    Debug.Log("(Dist)Number of elements in the posTarget: " + posTarget.Count);

                    //if there are more than one enemies within the list, remove the first enemy from the list
                    if (posTarget.Count > 1)
                    {
                        posTarget.RemoveAt(0);
                    }

                    //debug. check to see if there is at least one enemy remaining in the list
                    Debug.Log("(Remove(0))Number of elements in the posTarget: " + posTarget.Count);

                } while (posTarget.Count > 1); //continue to loop until there is one enemy left

                //change the posTarget to an array and set the first element in the array to nearest
                posTarget.ToArray(); 
                nearest = posTarget[0];
                Debug.Log(nearest + " is Target");

                //set will attack after move to true
                willAttackAfterMove = true;
            }
            else
            {
                unitOutOfRange = true;
                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(targets);

                //find the player unit with the lowest health
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

                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(posTarget);

                //search one more time to verify that the object in the list have the lowest health
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

                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(posTarget);

                //out of the remaining player units, find the one with the lowest defense
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

                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(posTarget);

                //out of the remaining player units, find the closest unit
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

                //change the posTarget list to an array and select the first unit in the array
                posTarget.ToArray();
                nearest = posTarget[0];
                Debug.Log(nearest + " is Target");
            }
            

        }//end of if health >3 
        else
        {
            //debug. make sure that this unit is making choices based off of having health < 3
            Debug.Log(this.gameObject + " Health is less than 3...");

            //reset the temp list to be the same as the postTarget list in casse of changes
            List<GameObject> tempList = new List<GameObject>(targets);

            //if this unit is a range unit
            if (this.GetComponent<Unit>().type == "Range")
            {
                
                //find all player units with health < 3 within this unit's potential range
                Debug.Log(this.gameObject + " Is Range");
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
                
                //if there are units in this unit's potential range whose health is < 3
                if(posTarget.Count !=0)
                {
                    float d = Mathf.Infinity;

                    //reset the temp list to be the same as the postTarget list in casse of changes
                    tempList = new List<GameObject>(posTarget);

                    //find the unit with the smallest health
                    foreach (GameObject o in tempList)
                    {
                        //if there are multiple units with the same lowest health, choose the closest of the units, otherwise choose the unit with the lowest health
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
                            Debug.Log("Didn't Lowest Health in Ranger.Health < 3");
                        }

                    }

                    //debug. verify a target was selected
                    Debug.Log(nearest + " is Target");
                }
                else //if there are no units with low health in this unit's potential range
                {
                    unitOutOfRange = true;
                    //make sure this unit is not in the list of npc allies
                    if (allies.Contains(this.gameObject))
                    {
                        allies.Remove(this.gameObject);
                    }

                    float d = Mathf.Infinity;

                    //make sure that there are other allies left on the field
                    if (allies.Count != 0)
                    {
                        //reset the temp list to be the same as the postTarget list in casse of changes
                        tempList = new List<GameObject>(allies);

                        //find the closest ally unit to move toward
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
                    else //if there are no npc allies left, attack
                    {
                        //reset the temp list to be the same as the postTarget list in casse of changes
                        tempList = new List<GameObject>(targets);

                        //find the nearest enemy unit
                        foreach (GameObject o in tempList)
                        {
                            // float d = Vector3.Distance(transform.position, o.transform.position);
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;

                                //if the unit is in this unit's potential range, attack after move
                                if(dist <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                                {
                                    willAttackAfterMove = true;
                                }

                                nearest = o;
                            }

                        }
                    }
                }
            }
            else if(this.GetComponent<Unit>().type == "Armour") //if this unit is an armour unit
            {
                bool foundEnemy = false;
                float d = Mathf.Infinity;

                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(targets);

                foreach (GameObject o in tempList)
                {
                    //check to see if there are any enemy units within this unit's potential range. If there is, attack it.
                    if(Vector3.Distance(transform.position, o.transform.position) <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                    {
                        float dist = Vector3.Distance(transform.position, o.transform.position);
                        if (dist <= d)
                        {
                            foundEnemy = true; 
                            d = dist;
                            nearest = o;
                            willAttackAfterMove = true;
                        }                    
                    }
                    else
                    {
                        foundEnemy = false; 
                        Debug.Log("Armour unit health less than 3; No enemy target found");
                        
                    }
                }
                if (!foundEnemy) //if no enemy is within this unit's potential range, travel towards the nearest ally
                {
                    unitOutOfRange = true;
                    //make sure that this unit is not within the npc ally list
                    if (allies.Contains(this.gameObject))
                    {
                        allies.Remove(this.gameObject);
                    }

                    //make sure this unit is not the only unit left on the npc team
                    if (allies.Count != 0)
                    {
                        //reset the temp list to be the same as the postTarget list in casse of changes
                        tempList = new List<GameObject>(allies);

                        //find the closest unit and move towards it
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
                    else //if this is the only unit left on the npc team 
                    {
                        //reset the temp list to be the same as the postTarget list in casse of changes
                        tempList = new List<GameObject>(targets);

                        //find the clossest enemy. 
                        foreach (GameObject o in tempList)
                        {
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;

                                //if the unit is in this unit's potential range, attack after move
                                if (dist <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                                {
                                    willAttackAfterMove = true;
                                }

                                nearest = o;
                            }

                        }
                    }
                }
                
            }
            else if(this.GetComponent<Unit>().type == "Melee") //if this unit is a melee unit (in other words a soldier)
            {
                float d = Mathf.Infinity;
                bool foundEnemy = false;

                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(targets);

                //search for units within this unit's potential range
                foreach (GameObject o in tempList)
                {
                    float dist = Vector3.Distance(transform.position, o.transform.position);
                    
                    //if there is a unit in this unit's potential range, find the unit with the lowest health
                    if (dist <= this.GetComponent<Unit>().range + this.GetComponent<Unit>().move)
                    {
                        //if the unit's health is the same as the smallest health, select the closest of the two
                        if (o.GetComponent<Unit>().health == smallestHealth)
                        {
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                smallestHealth = o.GetComponent<Unit>().health;
                                willAttackAfterMove = true;
                            }

                        }
                        else if (o.GetComponent<Unit>().health < smallestHealth)
                        {
                            d = dist;
                            nearest = o;
                            smallestHealth = o.GetComponent<Unit>().health;
                            willAttackAfterMove = true;
                        }

                        foundEnemy = true;
                    }
                }
                if (!foundEnemy) //if there is not an enemy in this unit's potential range
                {
                    unitOutOfRange = true;
                    //reset the temp list to be the same as the postTarget list in casse of changes
                    tempList = new List<GameObject>(targets);

                    //find the enemy that is the closest
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
            else if (this.GetComponent<Unit>().type == "Commander") //if this is the commanding unit
            {
                //reset the temp list to be the same as the postTarget list in casse of changes
                tempList = new List<GameObject>(targets);

                //find all enemy units that are in this unit's potential range with a health < 6
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

                //if there are any enemy units with a health < 6 within this unit's potential range, find the weakest and attack it
                if (posTarget.Count != 0)
                {
                    float d = Mathf.Infinity;

                    //reset the temp list to be the same as the postTarget list in casse of changes
                    tempList = new List<GameObject>(posTarget);

                    //find unit with the lowest health
                    foreach (GameObject o in tempList)
                    {
                        //if the units have the same lowest health, find the closest unit
                        if (o.GetComponent<Unit>().health == smallestHealth)
                        {
                            float dist = Vector3.Distance(transform.position, o.transform.position);
                            if (dist <= d)
                            {
                                d = dist;
                                nearest = o;
                                smallestHealth = o.GetComponent<Unit>().health;
                                willAttackAfterMove = true;
                            }

                        }
                        else if (o.GetComponent<Unit>().health < smallestHealth)
                        {
                            d = Vector3.Distance(transform.position, o.transform.position);
                            nearest = o;
                            smallestHealth = o.GetComponent<Unit>().health;
                            willAttackAfterMove = true;

                        }
                        else
                        {
                            Debug.Log("Didn't Lowest Health in commander.Health < 6");
                        }
                    }
                }
                else //no enemy units in potential range with a health < 6
                {
                    unitOutOfRange = true;

                    //make sure this unit is not in the npc ally list
                    if (allies.Contains(this.gameObject))
                    {
                        allies.Remove(this.gameObject);
                    }

                    float d = Mathf.Infinity;

                    //if there are other npc allies, move towards the closest one
                    if (allies.Count != 0)
                    {
                        //reset the temp list to be the same as the postTarget list in casse of changes
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
                    else //if this is the only unit left on npc team
                    {
                        //reset the temp list to be the same as the postTarget list in casse of changes
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
        if (unitOutOfRange)
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
            //willAttackAfterMove = true;
            if(willAttackAfterMove == true)
            {
                NPC_WillAttack = true;
            }
            
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
        if (this.GetComponent<Unit>().range == 1)
        {
            foreach (Tile t in playerTile.adjacencyList)
            {
                if (!t.selectable)
                {
                    continue;
                }
                actualTargetTile = t;
            }
            actualTargetTile.target = true;
        }
        else
        {
            for(int i = 0; i < this.GetComponent<Unit>().range; i++) { 
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
            }
            if (actualTargetTile != null)
            {
                actualTargetTile.target = true;
            }
            else
            {
                TurnManager.EndTurn();
            }
        }
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
