using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    static Animator playerAnim;

	// Use this for initialization
	void Start () {
        playerAnim = GetComponent <Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (PlayerMove.playerMoving == false)
        {
            playerAnim.SetBool("Running", false);
        }
        else
        {
            playerAnim.SetBool("Running", true);
        }

	}
}
