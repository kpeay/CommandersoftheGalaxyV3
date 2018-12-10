using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : TacticsMove {

    GameObject target;
    GameObject aiUnit;
    GameObject playerUnit;
    public bool animateMove = false;
    public bool animateAttack = false;
    public static bool playerMoving = false;
    public static bool playerAttacking = false;
    public static bool skipUnit = false;

    List<Tile> selectedTiles;

    private static int attackCount = 0;

    // Use this for initialization
    void Start ()
    {
        Init();

        //Anim = GetComponent<Animator>;

    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (newUnitTurn)
        {   // Just got turn. Find path and start moving
            selectedTiles = FindSelectableTiles(gameObject);
            CheckMouse();
            playerMoving = false;
            animateMove = false;
            newUnitTurn = false;
            moving = false;
            NPCMove.NPCMoving = false;
            return;
        }

        if (moving)
        {   // Continue moving to target tile
            //GR1Anim.letsMove = true;
            animateMove = true;
            Move(this.gameObject);
            playerMoving = true;
           // NPCMove.NPCMoving = false;
           // NPCMove.NPC_Attacking = false;
            return;
        }
        else
        {
            //animateMove = false;
            CheckMouse();
        }

        if (attacking)
        {
            playerAttacking = true;
            animateAttack = true;
            PlayerAttacksNPC(aiUnit);
            Debug.Log("Target unit; " + aiUnit);
        }

        if (skipUnit)
        {
            skipUnit = false;
            DontMove();
        }

        if (Input.GetKeyDown("w"))
        {
            SceneManager.LoadScene("WinScene");
        }

        if (Input.GetKeyDown("l"))
        {
            SceneManager.LoadScene("GameOverScene");
        }


    }

    // Check whether left mouse clicked. Here, if mouse click is on an enemy unit
    // in a reachable target, set playerAttacking flag.
    void CheckMouse()
    {
        NPCMove.NPCMoving = false;
        NPCMove.NPC_Attacking = false;

        bool skipUnit = false;
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(WaitTime(0.3f));
            skipUnit = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Mouse hit " + hit.collider.tag);
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    Debug.Log("Tile selectable " + t.selectable);
                    if (t.selectable)
                    {
                        moving = true;
                        Debug.Log("PlayerMove() t:" + t);
                        MoveToTile(t);
                    }
                }
                else if (hit.collider.tag == "NPC")
                {   // Will attack NPC pointed by mouse click. 
                    Tile t = GetTargetTile(hit.collider.gameObject);
                    target = hit.collider.gameObject;
                    aiUnit = t.GetUnitObject();

                    // Calculate the distance if it is less than range start moving
                    float npcDistance = Vector3.Distance(transform.position, target.transform.position);

                    if (npcDistance < this.GetComponent<Unit>().GetRange() + 1)
                    {
                        moving = true;
                        willAttackAfterMove = true;
                        DontMove();
                    }
                    else if (npcDistance <= this.GetComponent<Unit>().GetMove() + this.GetComponent<Unit>().GetRange())
                    {
                        moving = true;
                        // Find next selectable tile from adjacency list aand move to it
                        willAttackAfterMove = true;      // Set player attacking mode
                        MoveToSelectableNeighborTile(t, this.GetComponent<Unit>().GetRange(), this.gameObject);
                    }

                }
            }
        }
    }


    void PlayerAttacksNPC(GameObject aiUnit)
    {   // Player Attacks NPC pointed by mouse click
        //Debug.Log("Player attacking NPC........");
        //PlayerCombat.SetStats(soldier.Unit.GetAttack(), soldier.Unit.GetDefense, target.Unit.GetAttack(), target.Unit.GetDefense); FIX LATER*****
        PlayerCombat pc = new PlayerCombat();
        //pc.SetStats(5, 5, 5, 5);
        
        int dmg = pc.AttackPhase(true, this.GetComponent<Unit>().GetAttack(), this.GetComponent<Unit>().GetDefense(), aiUnit.GetComponent<Unit>().GetAttack(), aiUnit.GetComponent<Unit>().GetDefense());
        aiUnit.GetComponent<Unit>().TakeDmg(dmg);
        Debug.Log("Player hits for: " + dmg);
        Debug.Log("Player has: " + this.GetComponent<Unit>().GetHealth());

        attacking = false;
        willAttackAfterMove = false;
      
        StartCoroutine(WaitTime(1.0f));
        //Debug.Log("I done waited");
        //TurnManager.EndTurn();

        /* attackCount++;      // Increase attack count for testing
         if (attackCount < 5)
         {
             Debug.Log("Player hits for: " + dmg);
         }
         else
         {
             attackCount = 0;

             // Run following when attack completes for turning to another unit
             Debug.Log("Player attacks ended....Turning to another unit");
             attacking = false;
             TurnManager.EndTurn();
         }*/
    }

    IEnumerator WaitTime(float sec)
    {
        //print(Time.time);
        yield return new WaitForSeconds(sec); //This Command doesn't get activated; why not?
                                              // Time.timeScale = 0;
        animateAttack = false;
        TurnManager.EndTurn(); //Supposedly it must be called from the CoRountinue what ever it is you want to happen
    }

}
