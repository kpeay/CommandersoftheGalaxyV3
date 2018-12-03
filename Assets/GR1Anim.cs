using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GR1Anim : MonoBehaviour {

    Animator playerAnim;
    PlayerMove pmGR1 = new PlayerMove();

    // Use this for initialization
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool letsMove = pmGR1.animateMove;
        bool letsAttack = pmGR1.animateAttack;

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
