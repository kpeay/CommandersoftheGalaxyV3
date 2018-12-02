using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimR1 : MonoBehaviour {

    private Animator playerAnimR1;

    void Start()
    {
        playerAnimR1 = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerMove.playerMoving)
        {
            playerAnimR1.SetBool("Running", true);
        }
        else
        {
            playerAnimR1.SetBool("Running", false);
        }

        if (PlayerMove.playerAttacking)
        {
            playerAnimR1.SetBool("Attacking", true);
        }
        else
        {
            playerAnimR1.SetBool("Attacking", false);
        }

    }
}
