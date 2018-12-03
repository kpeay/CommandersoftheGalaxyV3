using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimController : MonoBehaviour {

    Animator npcAnim;

    // Use this for initialization
    void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.parent.gameObject.GetComponent<NPCMove>().animateMove)
        {
            npcAnim.SetBool("Running", true);
        }
        else
        {
            npcAnim.SetBool("Running", false);
        }

        if (transform.parent.gameObject.GetComponent<NPCMove>().animateAttack)
        {
            npcAnim.SetBool("Attacking", true);
        }
        else
        {
            npcAnim.SetBool("Attacking", false);
        }

    }
}
