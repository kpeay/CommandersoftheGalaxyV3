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
            FindNearestTarget();    // Find nearest target "Player"
            CalculatePath();        // Calculate A* path to nearest Player
            FindSelectableTiles(gameObject);  // Shows all potential target tile moves
            actualTargetTile.target = true;
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
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);   // Perform A* search
    }

    void FindNearestTarget()
    {   // Player objects are enemies for NPC. Therefore,
        // create an array of targets with Player tags. 
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        // Simplest AI. Look for nearest player unit. Therefore, 
        // distance will be used to find an attackable player
        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        playerUnit = nearest;
        target = nearest;

        //Debug.Log("NPC distance to player " + distance);
        if (distance < 5.0)
        {
            //Debug.Log("Set Player Tile attackable----");
            willAttackAfterMove = true;
            NPC_WillAttack = true;
        }
    }

    void NPCAttacksPlayer(GameObject target)
    {

        PlayerCombat pc = new PlayerCombat();

        int dmg = pc.AttackPhase(true, this.GetComponent<Unit>().GetAttack(), this.GetComponent<Unit>().GetDefense(), playerUnit.GetComponent<Unit>().GetAttack(), playerUnit.GetComponent<Unit>().GetDefense());
        Debug.Log("NPC hits for: " + dmg);
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
