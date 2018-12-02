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
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)  // Is it my turn
        {   // No, do nothing.
            return;
        }

        if (newUnitTurn)
        {   // Just got turn. Find path and start moving
            NPCMoving = false;
            selectedTiles = FindSelectableTiles(gameObject);  // Shows all potential target tile moves
            FindNearestTarget();    // Find nearest target "Player"
            CalculatePath();        // Calculate A* path to nearest Player
            selectedTiles = FindSelectableTiles(gameObject);  // Shows all potential target tile moves
            newUnitTurn = false;
            moving = true;
            return;
        }

        if (moving)
        {   // Continue moving to target tile
            Move();
            NPCMoving = true;
            PlayerMove.playerMoving = false;
            PlayerMove.playerAttacking = false;
            return;
        }

        if (attacking)
        {
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
        GameObject smallestHealthObj = null;
        float distance = Mathf.Infinity;
        int smallestHealth = 1000;
        int myHealth = this.GetComponent<Unit>().GetHealth();
        float myRange = this.GetComponent<Unit>().GetRange();

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);
            int objHealth = obj.GetComponent<Unit>().GetHealth();

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }

            if (objHealth < smallestHealth && d < myRange)
            {   // Found a Player within NPC reach
                smallestHealth = objHealth;
                smallestHealthObj = obj;
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
            playerUnit = smallestHealthObj;
            target = smallestHealthObj;
            //Debug.Log("Set Player Tile attackable----");
            willAttackAfterMove = true;
            NPC_WillAttack = true;
            FindInRangeTargetTile(target);
        }

    }

    void FindOffRangeTargetTile(GameObject nearestPlayer)
    {
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
}
