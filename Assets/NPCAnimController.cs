using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimController : MonoBehaviour {

    static Animator npcAnim;

    // Use this for initialization
    void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (NPCMove.NPCMoving == false)
        {
            npcAnim.SetBool("Running", false);
        }
        else
        {
           npcAnim.SetBool("Running", true);
        }

    }
}
