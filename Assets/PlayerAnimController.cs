using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    Animator playerAnim;
    PlayerMove pm = new PlayerMove();

    // Use this for initialization
    void Start () {
        playerAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool letsMove = pm.animateMove;
        bool letsAttack = pm.animateAttack;

        if (letsMove)
        {
            playerAnim.SetBool("Running", true);
            Debug.Log(letsMove);
        }
        else
        {
            playerAnim.SetBool("Running", false);
            Debug.Log(letsMove);
        }

        if (letsAttack)
        {
            playerAnim.SetBool("Attacking", true);
            Debug.Log(letsAttack);
        }
        else
        {
           playerAnim.SetBool("Attacking", false);
            Debug.Log(letsAttack);
        }

	}

    public void SetAttackingPlayer()
    {
        PlayerMove.playerAttacking = false;
    }
}
