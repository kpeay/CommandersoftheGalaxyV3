using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    Animator playerAnim;
    //bool letsMove = this.PlayerMove.animateMove;
    //bool letsAttack = this.gameObject.GetComponent<PlayerMove>().animateAttack;

    // Use this for initialization
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        if (transform.parent.gameObject.GetComponent<PlayerMove>().animateMove)
        {
            playerAnim.SetBool("Running", true);
        }
        else
        {
            playerAnim.SetBool("Running", false);
        }

        if (transform.parent.gameObject.GetComponent<PlayerMove>().animateAttack)
        {
            playerAnim.SetBool("Attacking", true);
        }
        else
        {
            playerAnim.SetBool("Attacking", false);
        }

    }

    public void SetAttackingPlayer()
    {
        PlayerMove.playerAttacking = false;
    }
}
