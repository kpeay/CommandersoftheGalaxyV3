using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GR1Anim : MonoBehaviour {

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
            Debug.Log(transform.parent.gameObject.GetComponent<PlayerMove>().animateMove);
        }
        
        if (!transform.parent.gameObject.GetComponent<PlayerMove>().animateMove)
        {
            playerAnim.SetBool("Running", false);
            Debug.Log(transform.parent.gameObject.GetComponent<PlayerMove>().animateMove);
        }

        if (transform.parent.gameObject.GetComponent<PlayerMove>().animateAttack)
        {
            playerAnim.SetBool("Attacking", true);
            Debug.Log(transform.parent.gameObject.GetComponent<PlayerMove>().animateAttack);
        }
        else
        {
            playerAnim.SetBool("Attacking", false);
            Debug.Log(transform.parent.gameObject.GetComponent<PlayerMove>().animateAttack);
        }

    }

    public void SetAttackingPlayer()
    {
        PlayerMove.playerAttacking = false;
    }
}
