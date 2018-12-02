using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    private Animator playerAnim;

	// Use this for initialization
	void Start () {
        this.playerAnim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (PlayerMove.playerMoving)
        {
            playerAnim.SetBool("Running", true);
        }
        else
        {
            playerAnim.SetBool("Running", false);
        }

        if (PlayerMove.playerAttacking)
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
